using System;
using UnityEngine;

namespace HJ
{
    public class Interactor : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                interactable.InteractableOn();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                interactable.InteractableOff();
            }
        }
    }
}