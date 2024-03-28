using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Light : MonoBehaviour
{
    Light myLight;
    public GameObject Pillar;
    
    // Start is called before the first frame update
    void Start()
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

                if (myLight.enabled == true)
                {
                    Animator animator = Pillar.GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.SetTrigger("isOpen");
                    }
                }
            }
        }
    }
 }
