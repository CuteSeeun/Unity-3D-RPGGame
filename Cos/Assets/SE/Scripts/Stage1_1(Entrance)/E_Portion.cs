using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ;

public class E_Portion : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject _interactorLight;

    public GameObject DoorLock;
    public GameObject Portion;
    public GameObject Gate;

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
        interactor.GetComponent<PlayerController>().potionNumber++;
        Destroy(Portion);

        DoorLock.SetActive(false);
        Animator animator = Gate.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("isOpen");
        }
    }
}
