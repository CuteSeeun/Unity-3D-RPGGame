using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother_of_Spike : MonoBehaviour
{
    public Animator Spike;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(Spike != null)
            {
                Spike.SetTrigger("isWork");
            }
        }
    }


    void Update()
    {
        
    }
}
