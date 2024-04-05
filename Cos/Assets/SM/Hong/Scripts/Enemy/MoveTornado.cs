using System.Collections;
using System.Collections.Generic;
using HJ;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 보르 2페이즈가 Tornado 패턴 때 소환하는 오브젝트의 스크립트
/// 보스 방향을 바라보며 보스 방향으로 이동
/// 플레이어와 출돌 시 데미지를 부여하고 보스와 충돌 시 사라짐.
/// </summary>
public class MoveTornado : MonoBehaviour
{
    public float speed = 5f;
    Transform boss;
    NavMeshAgent agent;
    void Start()
    {
        Invoke("Move", 1.5f);
        boss = GameObject.FindWithTag("Boss").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        
    }

    void Move()
    {
        transform.LookAt(boss.position);
        agent.SetDestination(boss.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boss"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.layer != LayerMask.NameToLayer("Boss")) // 보스 레이어가 아닌 경우에만 데미지를 주도록 합니다.
        {
            if (other.gameObject.TryGetComponent(out IHp iHp))
            {
                iHp.Hit(5, true, Quaternion.identity);
            }
        }
    }
}
