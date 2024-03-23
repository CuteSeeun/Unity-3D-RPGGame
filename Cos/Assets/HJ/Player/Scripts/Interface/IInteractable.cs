using HJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJ
{
    public interface IInteractable
    {
        void InteractableOn();
        void InteractableOff();
        void Interaction(GameObject interactor);
    }
}
