using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyS_AI : MonoBehaviour
{
    float chaseSpeed = 5f;
    public float detectionRange;
    public float attackRange;
    float detectionAngle = 360f;
    public int hp;

    private Animator m_Animator;
    private NavMeshAgent agent;
    private Transform player;
    private bool isChasing;
    private bool isDeath;
    private int attackStack = 0;
    private float attackTimer;

    // 추가된 코드: 감지 범위와 공격 범위를 시각화하기 위한 색상 변수
    public Color detectionColor = Color.yellow;
    public Color attackColor = Color.red;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;     
        agent.isStopped = true;
        agent.speed = chaseSpeed;
        agent.stoppingDistance = 3;
    }

    void Update()
    {
        if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Basic"))
        {
            m_Animator.SetInteger("state", 2);
            if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn"))
            {              
                isChasing = true;
                if (isChasing)
                {
                    agent.speed = chaseSpeed;
                    if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                    {
                        agent.isStopped = false;
                        m_Animator.SetInteger("state", 3);
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
                        m_Animator.SetInteger("state", 2);
                        if (attackStack < 2 && attackTimer == 0
                            && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1")
                            && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
                        {
                            Attack1();
                        }
                        else if (attackStack == 2 && attackTimer == 0
                            && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                        {
                            Attack2();        
                        }
                    }
                    if(attackTimer > 0)
                    {
                        attackTimer -= Time.deltaTime;
                    }
                    else if(attackTimer < 0)
                    {
                        attackTimer = 0;
                    }
                }
            }
        }
        if (hp <= 0 && !isDeath)
        {
            m_Animator.SetTrigger("isDeath");
            isDeath = true;
            Invoke("Death", 2);
        }
    }

    void Attack1()
    {
        agent.isStopped = true;
        m_Animator.SetInteger("state", 2);
        m_Animator.SetBool("isAttack",true);
        attackTimer = 3;
    }

    void Attack2()
    {
        agent.isStopped = true;
        m_Animator.SetInteger("state", 2);
        m_Animator.SetBool("isAttackS", true);
        attackTimer = 3;
    }

    void OnDrawGizmosSelected()
    {
        // 감지 범위 시각화
        Gizmos.color = detectionColor;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 공격 범위 시각화
        Gizmos.color = attackColor;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 playerDirection = other.transform.position - transform.position;
            float angle = Vector3.Angle(playerDirection, transform.forward);

            if (angle < detectionAngle && playerDirection.magnitude < detectionRange)
            {
                m_Animator.SetInteger("state", 1);
            }
        }
    }
    
    public void AttackEnd()
    {
        agent.isStopped = false;
        isChasing = true;
        attackStack++;
        if (attackStack > 2)
        {
            attackStack = 0;
            Debug.Log("초기화");
        }
        Debug.Log(attackStack);
        m_Animator.SetBool("isAttack", false);
        m_Animator.SetBool("isAttackS", false);

    }

    public void Death()
    {
        Destroy(gameObject);
    }

    void Hit()
    {
        //데미지 가하는 코드 구현
    }
}