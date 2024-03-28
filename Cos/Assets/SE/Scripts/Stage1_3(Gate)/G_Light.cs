using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Light : MonoBehaviour
{
    public GameObject Light1;
    public GameObject Light2;
    

    // Start is called before the first frame update
    void Start()
    {
        //myLight = GetComponent<Light>();
        //myLight.enabled = false;

        //addLight = GetComponent<Light>();
        //addLight.enabled = false;

        Light lightComponent = Light1.GetComponent<Light>();
        if (lightComponent != null)
        {
            lightComponent.enabled = false; // Light 컴포넌트를 초기에 비활성화
        }

        Light lightComponent2 = Light2.GetComponent<Light>();
        if (lightComponent2 != null) 
        {
            lightComponent2.enabled = false;
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Light1.GetComponent<Light>().enabled = true;
                Light2.GetComponent<Light>().enabled = true;


            }
        }
    }
}
