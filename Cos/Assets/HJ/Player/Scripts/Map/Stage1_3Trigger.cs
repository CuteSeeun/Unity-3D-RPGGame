using HJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_3Trigger : MonoBehaviour, IInteractable
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
        triggerSwitch++;
        if (triggerSwitch >= 2)
        {
            _doorExit.isLocked = false;

            foreach (var item in interactables)
            {
                item.GetComponent<IInteractable>().Interaction(gameObject);
            }
        }
    }
}
