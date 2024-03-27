using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Gate : MonoBehaviour //확인해야함
{
    Animator animator;
    public GameObject wall;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("isOpen");
           
        }
        //if (!other.CompareTag("Enemy"))  //확인해야함
        //{
        //    Destroy(gameObject);
        //    if (wall != null)
        //    {
        //        Animator wallAnimator = wall.GetComponent<Animator>();
        //        wallAnimator.SetTrigger("isOpen");
        //    }
        //}
    }
}
