using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ;

public class PillarDown : MonoBehaviour, IInteractable
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void InteractableOff()
    {
    }

    public void InteractableOn()
    {
    }

    public void Interaction(GameObject interactor)
    {
        _animator.SetTrigger("isDown");
        SFX_Manager.Instance.VFX(51);

    }
}
