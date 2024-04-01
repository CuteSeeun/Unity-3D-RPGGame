using HJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_2Trigger : MonoBehaviour, IInteractable
{
    private int triggerSwitch;
    [SerializeField] List<GameObject> interactables;
    [SerializeField] List<Door1> doors;
    [SerializeField] Artifact artifact;
    [SerializeField] Door1 _doorExit;
    [SerializeField] float _openTime;
    [SerializeField] float _artifactTime;

    public void InteractableOff()
    {
    }

    public void InteractableOn()
    {
    }

    public void Interaction(GameObject interactor)
    {
        if (triggerSwitch == 0)
        {
            StartCoroutine(DoorOpen());
        }
        else if (triggerSwitch >= 1)
        {
            _doorExit.isLocked = false;
        }
    }

    private IEnumerator DoorOpen()
    {
        foreach (var door in doors)
        {
            door.Interaction(gameObject);
            yield return new WaitForSeconds(_openTime);
        }
        yield return new WaitForSeconds(_artifactTime);
        artifact.LockOff();
        triggerSwitch++;
    }
}
