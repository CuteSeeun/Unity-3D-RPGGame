using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Gate : MonoBehaviour  //애니메이션 재생 스크립트
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void PlayAnimation()
    {
        animator.Play("P_Gate");
        SFX_Manager.Instance.VFX(51);

    }


}
