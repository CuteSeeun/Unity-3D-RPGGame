using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class C_Light : MonoBehaviour
{
    Light myLight;
    public GameObject DoorLock;

    private void Start()
    {
        myLight = GetComponent<Light>();
        myLight.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                myLight.enabled = !myLight.enabled;
                DoorLock.SetActive(false);
            }
        }
    }
}
