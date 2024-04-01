using HJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_2Trigger : MonoBehaviour, IInteractable
{
    private int triggerSwitch;
    [SerializeField] List<GameObject> interactables;
    [SerializeField] Trapdoor _trapdoor;
    [SerializeField] float _trapdoorTime;
    [SerializeField] Animator _exitDoor;

    public void InteractableOff()
    {
    }

    public void InteractableOn()
    {
    }

    public void Interaction(GameObject interactor)
    {
        triggerSwitch++;
        if (triggerSwitch == 1)
        {
            StartCoroutine(Trapdoor());
        }
        if (triggerSwitch == 2)
        {
            _exitDoor.SetTrigger("isOpen");
        }
    }

    private IEnumerator Trapdoor()
    {
        yield return new WaitForSeconds(_trapdoorTime);
        StartCoroutine(_trapdoor.TrapdoorOn());
    }
}
