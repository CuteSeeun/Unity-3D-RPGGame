using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCollider : MonoBehaviour //자식
{


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 부모 오브젝트에서 Enemy 감지를 처리하도록 이벤트 호출
            transform.parent.GetComponent<G_Light>().OnEnemyDetected();
        }
    } 
}
