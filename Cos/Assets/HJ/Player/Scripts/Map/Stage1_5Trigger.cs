using HJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_5Trigger : MonoBehaviour, IInteractable
{
    private int triggerSwitch;
    [SerializeField] List<GameObject> interactables;
    [SerializeField] Door1 _doorExit;

    public void InteractableOff()
    {
    }

    public void InteractableOn()
    {
    }

    public void Interaction(GameObject interactor)
    {
        _doorExit.isLocked = false;
    }
}
