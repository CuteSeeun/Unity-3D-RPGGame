using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ;

public class G_Light : MonoBehaviour, IInteractable
{
    public GameObject LightWhite;
    public GameObject LightYellow;
    private Collider _collider;

    private bool _isOn;

    [SerializeField] List<GameObject> interactables;

    private void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
    }

    public void InteractableOn()
    {
        if (_isOn == false)
        {
            LightWhite.SetActive(true);
        }
    }

    public void InteractableOff()
    {
        if (_isOn == false)
        {
            LightWhite.SetActive(false);
        }
    }

    public void Interaction(GameObject interactor)
    {
        InteractableOff();
        _collider.enabled = false;
        if (_isOn == false)
        {
            _isOn = true;
            LightYellow.SetActive(true);
            SFX_Manager.Instance.VFX(49);


            foreach ( var item in interactables)
            {
                item.GetComponent<IInteractable>().Interaction(gameObject);
            }
        }
    }
}
