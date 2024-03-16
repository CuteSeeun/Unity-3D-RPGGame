using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class CloseEnemyAI : MonoBehaviour
{
    float patrolSpeed = 2f;
    float chaseSpeed = 5f;
    float patrolWaitTime = 3f;
    public float detectionRange;
    public float attackRange;
    float detectionAngle = 360f;
    public int hp;
    public int currentHp;
    

    private Animator m_Animator;
    private NavMeshAgent agent;
    private Transform player;
    private Vector3 patrolDestination;
    private bool isPatrolling;
    private bool isChasing;
    private bool isDeath;
    private float attackTimer;

    // 추가된 코드: 감지 범위와 공격 범위를 시각화하기 위한 색상 변수
    public Color detectionColor = Color.yellow;
    public Color attackColor = Color.red;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        patrolDestination = GetRandomPatrolDestination();
        isPatrolling = true;
        agent.isStopped = false;
        agent.speed = patrolSpeed;
        currentHp = hp;
    }

    void Update()
    {
        if (isPatrolling)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                m_Animator.SetInteger("state", 0);
                StartCoroutine(Patrol());
            }
        }
        else if (isChasing)
        {
            agent.speed = chaseSpeed;
            if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
            {
                agent.isStopped = false;
                m_Animator.SetInteger("state", 2);
                transform.LookAt(player.position);
                agent.SetDestination(player.position);
                agent.stoppingDistance = 2;
            }
            else
            {
                agent.isStopped = true;
                agent.SetDestination(transform.position);
                transform.LookAt(transform.position + transform.forward);
            }
            if (Vector3.Distance(transform.position, player.position) < attackRange
                && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
            {
                agent.isStopped = true;
                m_Animator.SetInteger("state", 0);
                if (attackTimer == 0)
                Attack();
            }
        }
        if(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            Debug.Log(attackTimer);
        }
        else if(attackTimer < 0)
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

    IEnumerator Patrol()
    {
        isPatrolling = false;
        yield return new WaitForSeconds(patrolWaitTime);
        if (!isChasing)
        {
            m_Animator.SetInteger("state", 1);
            patrolDestination = GetRandomPatrolDestination();
            agent.SetDestination(patrolDestination);
            transform.LookAt(patrolDestination);
            isPatrolling = true;
        }
    }

    void Attack()
    {
        m_Animator.SetTrigger("isAttack");
        attackTimer = 3;
        // 공격 동작 구현
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
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.speed = patrolSpeed;
            isChasing = false;
            isPatrolling = true;
        }
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

    void Hit()
    {
        //데미지 가하는 코드 구현
    }
}