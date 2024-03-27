using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Light : MonoBehaviour //부모
{
    Light myLight;

    void Start()
    {
        myLight = GetComponent<Light>();
        myLight.enabled = false;

    }
    
    public void OnEnemyDetected()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            myLight.enabled = !myLight.enabled;
        }
    }

   
}
