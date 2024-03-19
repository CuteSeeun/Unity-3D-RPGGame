using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //충돌한 대상이 플레이어가 아니라면 반환
        if (!collision.gameObject.tag.Equals("Player"))
        {
            return;
        }

        //충돌한 대상이 플레이어인지 확인하고 플레이어에게 데미지를 주는 코드 필요
    }
}
