using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public void OnTriggerStay(Collider other)
    {
        Debug.Log(1);
        StartCoroutine("Firework");
    }

    IEnumerator Firework()
    {
        yield return null; 

        //코루틴에 플레이어에게 데미지를 준다는 코드를 넣어야함(hit함수를 2초마다 호출하는 코드?)
    }
    void Hit()
    {
        //플레이어에게 데미지 주는 코드
    }
    public void OnTriggerExit(Collider other)
    {
        //플레이어에게 데미지 주는거 멈추는 코드
    }
}
