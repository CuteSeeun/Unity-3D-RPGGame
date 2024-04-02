using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ;

public class Chest : MonoBehaviour, IInteractable
{
    private Animator animator;
    public bool _isLocked;
    private Collider _collider;
    [SerializeField] GameObject _interactorLight;
    [SerializeField] GameObject _LockedLight;
    [SerializeField] GameObject _yellowLight;
    [SerializeField] List<GameObject> interactables;

    private void Start()
    {
        animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
    }

    public void InteractableOn()
    {
        if (_isLocked == false)
        {
            _interactorLight.SetActive(true);
        }
        else
        {
            _LockedLight.SetActive(true);
        }
    }

    public void InteractableOff()
    {
        if (_isLocked == false)
        {
            _interactorLight.SetActive(false);
        }
        else
        {
            _LockedLight.SetActive(false);
        }
    }

    public void LockOff()
    {
        _isLocked = false;
        _LockedLight.SetActive(true);
    }

    public void Interaction(GameObject interactor)
    {
        if (_isLocked == false)
        {
            Destroy(_collider);
            InteractableOff();
            animator.SetBool("isBox", true);
            _yellowLight.SetActive(true);

            foreach (var item in interactables)
            {
                item.GetComponent<IInteractable>().Interaction(gameObject);
            }
        }
    }
}
