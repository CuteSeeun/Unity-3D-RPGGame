using GSpawn;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Item : MonoBehaviour
{
    //3개의 조명이 enabled이여야 아이템 먹을 수 있음. 아이템 먹으면 문 애니메이션 재생.

    public GameObject Light1;
    public GameObject Light2;
    public GameObject Light3;
    public GameObject Door;

    private void Update()
    {
        if (Light1.GetComponent<Light>().enabled &&
            Light2.GetComponent<Light>().enabled &&
            Light3.GetComponent<Light>().enabled)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Destroy(gameObject);

                //문 열리는 애니메이션 실행.
                Animator animator = Door.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetBool("isOpen", true);
                }
            }
        }
    }

}
