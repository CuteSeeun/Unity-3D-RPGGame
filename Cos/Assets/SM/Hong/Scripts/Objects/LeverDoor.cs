using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverDoor : MonoBehaviour
{
    Animator d_Animator;
    public GameObject doorLock;
    void Start()
    {
        d_Animator = GetComponent<Animator>();
    }

    void Update()
    {
       
    }

    private void OnTriggerStay(Collider other)
    {      
        if (!doorLock.activeSelf)
        {
            Debug.Log("해제");
            if (other.CompareTag("Player"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Debug.Log("열림");
                    d_Animator.SetTrigger("isOpen");
                }
            }
        }
    }
}
