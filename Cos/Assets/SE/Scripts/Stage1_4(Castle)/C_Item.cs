using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Item : MonoBehaviour //안쓰는 스크립트
{
    public GameObject DoorLock;
    Light myLight;

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
                DoorLock.SetActive(false);
                myLight.enabled = !myLight.enabled;
            }
        }
    }
}
