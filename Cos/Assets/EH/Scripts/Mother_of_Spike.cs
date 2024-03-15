using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother_of_Spike : MonoBehaviour
{
    public Animator Spike;
    public void OnTriggerEnter(Collider other)
    {
        //플레이어가 트리거에 충돌하면 스파이크애니메이션이 널이 아닐 때 스파이크의 isWork 트리거를 실행한다.
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
