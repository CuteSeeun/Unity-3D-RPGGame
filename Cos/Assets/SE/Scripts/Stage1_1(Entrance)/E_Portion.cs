using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ;

public class E_Portion : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject _potion;
    [SerializeField] GameObject _interactorLight;
    public Door1 _door;

    public void InteractableOn()
    {
        _interactorLight.SetActive(true);
    }

    public void InteractableOff()
    {
        _interactorLight.SetActive(false);
    }

    public void Interaction(GameObject interactor)
    {
        //interactor.GetComponent<PlayerControllerForTest>().potionNumber++;
        interactor.GetComponent<PlayerController>().potionNumber++;

        Destroy(_potion);
        _door.isLocked = false;
    }
}
