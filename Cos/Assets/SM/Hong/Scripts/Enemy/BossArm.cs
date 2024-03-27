using System.Collections;
using System.Collections.Generic;
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
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        Invoke("End", 10f);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            animator.SetTrigger("isAttack");
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
}
