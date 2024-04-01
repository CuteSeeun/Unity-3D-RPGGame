using HJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_1Trigger : MonoBehaviour, IInteractable
{
    private int triggerSwitch;
    [SerializeField] List<GameObject> interactables;
    [SerializeField] Artifact _artifact;
    [SerializeField] Animator _slideFloor;

    public void InteractableOff()
    {
    }

    public void InteractableOn()
    {
    }

    public void Interaction(GameObject interactor)
    {
        triggerSwitch++;
        if (triggerSwitch == 3)
        {
            _artifact.LockOff();
        }
        else if (triggerSwitch == 4)
        {
            _slideFloor.SetTrigger("isOpen");
        }
    }
}
