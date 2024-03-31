using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ;

public class G_LightOn : MonoBehaviour, IInteractable
{
    public GameObject LightYellow;

    private void Start()
    {
    }

    public void InteractableOn()
    {
    }

    public void InteractableOff()
    {
    }

    public void Interaction(GameObject interactor)
    {
        LightYellow.SetActive(true);
    }
}
