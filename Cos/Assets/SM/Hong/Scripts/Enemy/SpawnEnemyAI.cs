using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class SpawnEnemyAI : MonoBehaviour
{
    float chaseSpeed = 5f;
    public float attackRange;
    public int hp;

    private Animator m_Animator;
    private NavMeshAgent agent;
    private Transform player;
    private bool isChasing;
    private bool isDeath;
    private bool isSpawn;
    private float attackTimer;

    // 추가된 코드: 감지 범위와 공격 범위를 시각화하기 위한 색상 변수
    public Color attackColor = Color.red;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isChasing = true;
        agent.isStopped = false;
        Invoke("Spawn", 3);
    }

    void Update()
    {
        if (isSpawn)
        {
            agent.isStopped = false;           
            if (isChasing)
            {
                agent.speed = chaseSpeed;
                if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                {
                    agent.isStopped = false;
                    m_Animator.SetInteger("state", 2);
                    transform.LookAt(player.position);
                    agent.SetDestination(player.position);
                }
                else
                {
                    agent.isStopped = true;
                    agent.SetDestination(transform.position);
                    transform.LookAt(transform.position + transform.forward);
                }
                if (Vector3.Distance(transform.position, player.position) < attackRange)
                {
                    agent.stoppingDistance = 2;
                    if (attackTimer == 0)
                    {
                        Attack();
                    }
                }
            }
            if (hp <= 0 && !isDeath)
            {
                m_Animator.SetTrigger("isDeath");
                isDeath = true;
                Invoke("Death", 2);
            }
            if(attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            if (attackTimer < 0)
            {
                attackTimer = 0;
            }
        }
    }

    void Attack()
    {
        agent.isStopped = true;
        attackTimer = 3;
        m_Animator.SetInteger("state", 0);
        m_Animator.SetTrigger("isAttack");
        // 공격 동작 구현
    }

    void OnDrawGizmosSelected()
    {
        // 공격 범위 시각화
        Gizmos.color = attackColor;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void AttackEnd()
    {
        agent.isStopped = false;
        isChasing = true;
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    public void Spawn()
    {
        m_Animator.SetTrigger("isSpawn");
        isSpawn = true;
    }
    void Hit()
    {
        //데미지 가하는 코드 구현
    }
}