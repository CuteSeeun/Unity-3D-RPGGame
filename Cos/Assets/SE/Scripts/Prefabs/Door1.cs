using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ;

public class Door1 : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject _interactorLight;
    private Animator _animator; //애니메이터 컴포넌트 참조
    public bool isOpen; //문 초기 상태 닫힘

    private void Start()
    {
        _animator = GetComponent<Animator>(); //가져오기: 스크립트에서 애니메이션 제어.
    }

    public void InteractableOn()
    {
        _interactorLight.SetActive(true);
    }

    public void InteractableOff()
    {
        _interactorLight.SetActive(false);
    }

    public void Interaction(GameObject interactor)
    {
        isOpen = true;
        _animator.SetBool("isOpen", true);
    }
}
