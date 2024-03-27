using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Item : MonoBehaviour
{
    //public GameObject Light;

    [SerializeField] private Light getLight;
    public bool flashOn;

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        if (Input.GetKeyDown(KeyCode.F))
    //        {
    //            Light.SetActive(true); 
    //        }
    //    }
    //}

    public void TurnOnOff()
    {
        if (flashOn)
            getLight.intensity = 0;
        else getLight.intensity = 10;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                flashOn = !flashOn;
            }
        }
    }
}
