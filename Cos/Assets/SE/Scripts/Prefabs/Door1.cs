using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ;

public class Door1 : MonoBehaviour, IInteractable
{
    private Animator _animator; //애니메이터 컴포넌트 참조
    private Collider _collider;

    private bool isOpen; //문 초기 상태 닫힘
    public bool isLocked;
    [SerializeField] List<GameObject> _wallsToDestroy;
    [SerializeField] List<GameObject> _objectsToActivate;

    [SerializeField] GameObject _interactorLight;
    [SerializeField] GameObject _LockedLight;

    [SerializeField] bool _bgmChanger;
    [SerializeField] int _bgmNum;

    [SerializeField] List<GameObject> _enemies;
    [SerializeField] List<GameObject> interactables;


    private void Start()
    {
        _animator = GetComponent<Animator>(); //가져오기: 스크립트에서 애니메이션 제어.
        _collider = GetComponent<Collider>();
    }

    public void InteractableOn()
    {
        if (isOpen == false)
        {
            if (isLocked == false)
            {
                _interactorLight.SetActive(true);
            }
            else
            {
                _LockedLight.SetActive(true);
            }
        }
    }

    public void InteractableOff()
    {
        if (isOpen == false)
        {
            if (isLocked == false)
            {
                _interactorLight.SetActive(false);
            }
            else
            {
                _LockedLight.SetActive(false);
            }
        }
    }

    public void Interaction(GameObject interactor)
    {
        InteractableOff();
        if (isLocked == false && isOpen == false)
        {
            isOpen = true;
            _animator.SetBool("isOpen", true);
            SFX_Manager.Instance.VFX(45);
            _collider.enabled = false;

            if (_bgmChanger)
            {
                SFX_Manager.Instance.BGMPLAY(_bgmNum);
            }

            foreach (var wall in _wallsToDestroy)
            {
                Destroy(wall);
            }

            foreach (var activate in _objectsToActivate)
            {
                activate.SetActive(true);
            }

            foreach (var item in interactables)
            {
                item.GetComponent<IInteractable>().Interaction(gameObject);
            }

            foreach (var enemy in _enemies)
            {
                // 적들 깨우기
                if (enemy.TryGetComponent(out CloseEnemyAI closeEnemy))
                {
                    closeEnemy.isAct = true;
                }
                else if (enemy.TryGetComponent(out LongEnemyAI longEnemy))
                {
                    longEnemy.isAct = true;
                }
                else if (enemy.TryGetComponent(out MageEnemyAI mageEnemy))
                {
                    mageEnemy.isAct = true;
                }
                else if (enemy.TryGetComponent(out SpawnEnemyAI spawnEnemy))
                {
                    enemy.gameObject.SetActive(true);
                }
            }
        }
    }
}
