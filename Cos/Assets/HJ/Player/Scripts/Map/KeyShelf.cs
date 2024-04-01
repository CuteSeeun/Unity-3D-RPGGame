using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ;

public class KeyShelf : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject _interactorLight;
    [SerializeField] List<GameObject> interactables;
    [SerializeField] P_Gate gate;

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
        InteractableOff();

        gate.PlayAnimation();
        
        Destroy(gameObject);
    }
}
