using System.Collections;
using System.Collections.Generic;
using HJ;
using UnityEngine;

public class BossArm : MonoBehaviour
{
    Animator animator;
    Transform player;
    public float attackRange;
    private float attackTimer = 0;
    public Color attackColor = Color.red;

    void Start()
    {
        //공격 타겟을 플레이어로 지정하고 10초후 오브젝트 파괴
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        Invoke("End", 10f);
    }

    void Update()
    {
        // 공격범위 내 플레이어 감지 시 플레이어 공격
        if (Vector3.Distance(transform.position, player.position) < attackRange && attackTimer == 0)
        {
            animator.SetTrigger("isAttack");
            attackTimer = 3;
            transform.LookAt(player.position);
        }

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else if (attackTimer < 0)
        {
            attackTimer = 0;
        }
    }
    void End()
    {
        animator.SetTrigger("isEnd");
        Invoke("Destroy", 1.5f);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // 공격 범위 시각화
        Gizmos.color = attackColor;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public LayerMask _attackLayerMask;
    float _attackAngleInnerProduct;
    public float _attackAngle = 180;
    void DamageA()
    {
        // 공격 거리 내 모든 적 탐색
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, 1, 0),
                                                  attackRange,
                                                  Vector3.up,
                                                  0,
                                                  _attackLayerMask);

        // 공격 각도에 따른 내적 계산
        _attackAngleInnerProduct = Mathf.Cos(_attackAngle * Mathf.Deg2Rad);

        // 내적으로 공격각도 구하기
        foreach (RaycastHit hit in hits)
        {
            if (Vector3.Dot((hit.transform.position - transform.position).normalized, transform.forward) > _attackAngleInnerProduct)
            {
                // 데미지 주고, 데미지, 공격 방향, 파워어택 여부 전달
                if (hit.collider.TryGetComponent(out IHp iHp))
                {
                    iHp.Hit(5, true, transform.rotation);
                }
            }
        }
    }
}
