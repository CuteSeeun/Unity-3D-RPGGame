using System;
using System.Collections;
using HJ;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EliteEnemy : MonoBehaviour, IHp
{
    float chaseSpeed = 5f;
    public float detectionRange;
    public float attackRange;
    float detectionAngle = 360f;
    private int currentHp;
    private float jumpForce = 50;
    private float fallSpeed = 100;
    private float rushSpeed = 15;
    private float stopThreshold = 0.1f; // 오브젝트가 멈춘 것으로 간주하는 속도 임계값


    private Animator m_Animator;
    private NavMeshAgent agent;
    private Transform player;
    private Rigidbody rb;
    public GameObject grounded;
    public GameObject jumpImpact;
    private bool isChasing;
    private bool isDeath;
    private bool isAttack;
    private bool isJumping;
    private bool jump;
    private float attackTimer;
    private int attackStack = 0;

    // 추가된 코드: 감지 범위와 공격 범위를 시각화하기 위한 색상 변수
    public Color detectionColor = Color.yellow;
    public Color attackColor = Color.red;

    public event Action<float> onHpChanged;
    public event Action<float> onHpDepleted;
    public event Action<float> onHpRecovered;
    public event Action onHpMin;
    public event Action onHpMax;

    public float hp { get; set; }
    public float hpMax { get; }

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isChasing = true;
        agent.isStopped = false;
        jumpImpact.SetActive(false);
        hp = hpMax;
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
            else if(m_Animator.GetCurrentAnimatorStateInfo(0).IsName("isRush_Golem"))
            {
                transform.LookAt(player.position);
                agent.isStopped = true;
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
                            jumpImpact.SetActive(false);
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
                    jumpImpact.SetActive(true);
                }
            }
        }
        if (transform.position.y > 30)
        {
            transform.position = new Vector3(player.position.x, transform.position.y,player.position.z);
            rb.velocity = Vector3.down * fallSpeed;
        }
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else if (attackTimer < 0)
        {
            attackTimer = 0;
        }
        if (currentHp <= 0 && !isDeath)
        {
            m_Animator.SetTrigger("isDeath");
            agent.isStopped = true;
            isDeath = true;
            Invoke("Death", 2);
        }
        if(m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Rush_Golem"))
        {
            agent.isStopped = false;
            m_Animator.SetInteger("state", 0);
            Debug.Log("돌진");
            Vector3 rush = transform.forward;
            rb.velocity = rush * rushSpeed;
            transform.LookAt(transform.position + transform.forward);
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
        //transform.LookAt(player.position);
        //agent.isStopped = true;
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
        Invoke("Grounded", 2f);
    }

    void Grounded()
    {
        grounded.SetActive(true);
        Invoke("Disgrounded", 1.5f);
    }

    void Disgrounded()
    {
        grounded.SetActive(false);
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
        m_Animator.SetInteger("state", 0);
        agent.isStopped = false;
        isChasing = true;
        isAttack = false;
        jump = false;
        rb.isKinematic = false;
        attackStack++;
        if(attackStack > 6)
        {
            attackStack = 0;
        }
        if(attackStack == 2 || attackStack == 5)
        {
            attackRange = 100;
        }
        else
        {
            attackRange = 5;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isJumping)
        {
            if (rb.velocity.magnitude < stopThreshold)
            {
                Debug.Log("충돌");
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            rb.isKinematic = true;
        }
    }

    public void DepleteHp(float amount)
    {
        hp -= amount;
        onHpChanged?.Invoke(hp);
        if (hp <= 0)
        {
            hp = 0;
            onHpDepleted?.Invoke(amount);
            onHpMin?.Invoke();
        }
    }

    public void RecoverHp(float amount)
    {
        throw new NotImplementedException();
    }

    public void Hit(float damage, bool powerAttack, Quaternion hitRotation)
    {
        DepleteHp(damage);
    }

    public void Hit(float damage)
    {
        DepleteHp(damage);
    }
}