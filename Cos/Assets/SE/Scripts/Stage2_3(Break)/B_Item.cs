using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Item : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Destroy(gameObject);
            }
        }
    }
}
