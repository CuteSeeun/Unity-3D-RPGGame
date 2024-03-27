using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Candle : MonoBehaviour
{
    Light myLight;

    private void Start()
    {
        myLight = GetComponent<Light>();

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //조명 끄기
                myLight.enabled = !myLight.enabled;
            }
        }
    }
}
