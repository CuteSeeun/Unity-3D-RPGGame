using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SliderFloor : MonoBehaviour //조건이 맞으면 애니메이션 재생만 하면 됨
{
    public GameObject Light1;
    public GameObject Light2;
    public GameObject Light3;
    public GameObject Item;
    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!Light1.GetComponent<Light>().enabled &&
            !Light2.GetComponent<Light>().enabled &&
            !Light3.GetComponent<Light>().enabled &&
            !Item)
        {
            animator.SetTrigger("isOpen");
            SFX_Manager.Instance.VFX(51);


        }
    }


    //public void PlayAnimation()
    //{
    //animator.Play("P_SlideFloor");
    //}
}
