using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Item : MonoBehaviour
{
    public GameObject ItemLock;
    

        void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ItemLock.SetActive(false);
            }
        }

    }
}
