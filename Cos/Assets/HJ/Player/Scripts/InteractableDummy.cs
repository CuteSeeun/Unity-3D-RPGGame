using HJ;
using UnityEngine;

public class InteractableDummy : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject GreenLight;

    public void InteractableOn()
    {
        GreenLight.SetActive(true);
    }

    public void InteractableOff()
    {
        GreenLight.SetActive(false);
    }

    public void Interaction(GameObject interactor)
    {
        Debug.Log("꺼져");
    }
}
