using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ;

public class Artifact : MonoBehaviour, IInteractable
{
    public bool _isLocked;
    [SerializeField] GameObject _interactorLight;
    [SerializeField] GameObject _LockedLight;
    [SerializeField] List<GameObject> interactables;

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
            interactor.GetComponent<PlayerController>().PotionFull();

            foreach (var item in interactables)
            {
                item.GetComponent<IInteractable>().Interaction(gameObject);
            }

            Destroy(gameObject);
        }
    }
}
