using HJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_4Trigger : MonoBehaviour, IInteractable
{
    private int triggerSwitch;
    [SerializeField] List<GameObject> interactables;
    [SerializeField] Artifact _artifact;
    [SerializeField] Door1 _doorExit;

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
            _doorExit.isLocked = false;
        }
    }
}
