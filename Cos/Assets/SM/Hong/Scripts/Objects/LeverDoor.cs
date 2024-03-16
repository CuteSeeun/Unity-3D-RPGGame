using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverDoor : MonoBehaviour
{
    Animator d_Animator;
    Animator l_Animator;
    bool isOpen;
    void Start()
    {
        d_Animator = GetComponent<Animator>();
        l_Animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(l_Animator.GetCurrentAnimatorStateInfo(0).IsName("LeverOn") && !isOpen)
        {
            isOpen = true;
            d_Animator.SetTrigger("isOpen");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("당김");
                l_Animator.SetTrigger("isLever");
            }
        }
    }
}
