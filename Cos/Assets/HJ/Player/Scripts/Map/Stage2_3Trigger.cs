using HJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_3Trigger : MonoBehaviour, IInteractable
{
    [SerializeField] Door1 _exitDoor;

    public void InteractableOff()
    {
    }

    public void InteractableOn()
    {
    }

    public void Interaction(GameObject interactor)
    {
        _exitDoor.isLocked = false;
    }
}
