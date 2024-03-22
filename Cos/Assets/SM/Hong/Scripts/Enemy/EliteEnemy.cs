using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EliteEnemy : MonoBehaviour
{
    float patrolSpeed = 2f;
    float chaseSpeed = 5f;
    float patrolWaitTime = 3f;
    public float detectionRange;
    public float attackRange;
    float detectionAngle = 360f;
    public int hp;
    public int currentHp;
    public float jumpForce;
    public float fallSpeed;


    private Animator m_Animator;
    private NavMeshAgent agent;
    private Transform player;
    private Rigidbody rb;
    private Vector3 patrolDestination;
    private bool isPatrolling;
    private bool isChasing;
    private bool isDeath;
    public bool isAttack;
    public bool isJumping;
    public bool jump;
    public float attackTimer;
    public int attackStack = 0;

    // 추가된 코드: 감지 범위와 공격 범위를 시각화하기 위한 색상 변수
    public Color detectionColor = Color.yellow;
    public Color attackColor = Color.red;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        patrolDestination = GetRandomPatrolDestination();
        isChasing = true;
        agent.isStopped = false;
        agent.speed = patrolSpeed;
        currentHp = hp;
    }

    void Update()
    {  
        if (isChasing)
        {
            agent.speed = chaseSpeed;
            if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
                || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                agent.isStopped = false;
                m_Animator.SetInteger("state", 1);
                transform.LookAt(player.position);
                agent.SetDestination(player.position);
                agent.stoppingDistance = 3;
            }
            else
            {
                agent.isStopped = true;
                agent.SetDestination(transform.position);
                transform.LookAt(transform.position + transform.forward);
            }
            if (Vector3.Distance(transform.position, player.position) < attackRange
                && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Golem"))
            {
                agent.isStopped = true;
                m_Animator.SetInteger("state", 0);
                if (attackTimer == 0 && !isAttack && !jump)
                    switch(attackStack)
                    {
                        case 0:
                            Attack();
                            isAttack = true;
                            break;
                        case 1:
                            Attack();
                            isAttack = true;
                            break;
                        case 2:
                            Rush();
                            isAttack = true;
                            break;
                        case 3:
                            Attack();
                            isAttack = true;
                            break;
                        case 4:
                            Attack();
                            isAttack = true;
                            break;
                        case 5:
                            jump = true;
                            agent.enabled = false;
                            m_Animator.SetTrigger("Jump");
                            Invoke("Jump", 0.5f);
                            isAttack = true;
                            break;
                        case 6:
                            Crush();
                            isAttack = true;
                            break;
                    }
            }
        }
        if (isJumping)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity = Vector3.down * fallSpeed;
                m_Animator.SetBool("Fall", true);

                if (transform.position.y <= 0)
                {
                    isJumping = false;
                    jump = false;
                    agent.enabled = true;
                    isAttack = false;
                    m_Animator.SetBool("Fall", false);
                    Debug.Log("착지" + isJumping);
                }
            }
        }
        if (transform.position.y > 30)
        {
            rb.velocity = Vector3.down * fallSpeed;
        }
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            Debug.Log(attackTimer);
        }
        else if (attackTimer < 0)
        {
            attackTimer = 0;
            Debug.Log(attackTimer);
        }
        if (currentHp <= 0 && !isDeath)
        {
            m_Animator.SetTrigger("isDeath");
            agent.isStopped = true;
            isDeath = true;
            Invoke("Death", 2);
        }
    }

    void Attack()
    {
        m_Animator.SetTrigger("isAttack");
        attackTimer = 3;
        // 공격 동작 구현
    }

    void Rush()
    {
        m_Animator.SetTrigger("isRush");
        agent.isStopped = true;
        attackTimer = 6;
    }

    void Jump()
    {
        rb.velocity = Vector3.up * jumpForce;
        Debug.Log("점프");
        isJumping = true;
        attackTimer = 3;
        attackStack++;
    }

    void Crush()
    {
        m_Animator.SetTrigger("isCrush");
        attackTimer = 5;
    }

    Vector3 GetRandomPatrolDestination()
    {
        float patrolRadius = 10f;
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1);
        return hit.position;
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
                isChasing = true;
            }
        }
    }

    public void AttackEnd()
    {
        agent.isStopped = false;
        isChasing = true;
        isAttack = false;
        jump = false;
        attackStack++;
        if(attackStack > 6)
        {
            attackStack = 0;
        }
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