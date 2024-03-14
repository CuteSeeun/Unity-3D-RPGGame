using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingLight : MonoBehaviour
{
    public int healAmount = 100; // 플레이어에게 회복시킬 체력량

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(1);
        // 충돌한 객체가 플레이어인지 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log(2);
            // 플레이어의 HealthController 스크립트를 가져옴
            // HealthController playerHealth = other.GetComponent<HealthController>();

            // HealthController 스크립트가 존재하면
            //if (playerHealth != null)
            //{
            // 플레이어의 체력을 모두 회복
            //playerHealth.Heal(playerHealth.maxHealth);

            // 해당 객체를 파괴
            Destroy(gameObject);
            //}
        }
    }
}
