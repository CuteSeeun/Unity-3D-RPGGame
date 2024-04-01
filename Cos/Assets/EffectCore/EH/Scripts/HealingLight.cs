using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingLight : MonoBehaviour
{
    public int heal = 100; // 플레이어에게 회복시킬 체력량

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(1);
        // 충돌한 객체가 플레이어인지 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log("Perfect Healing");
            
            // 플레이어의 체력을 모두 회복시키는 코드 필요
            

            // 해당 객체를 파괴
            Destroy(gameObject);
            
        }
    }
}
