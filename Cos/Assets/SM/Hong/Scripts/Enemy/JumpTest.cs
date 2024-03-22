using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpTest : MonoBehaviour
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
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").transform;
        m_Animator.SetInteger("state", 0);
    }

    void Update()
    {
        if (jump)
        {
            jump = false;
            m_Animator.SetTrigger("Jump");
            Invoke("Jump", 0.5f);
        }
        if (isJumping)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity = Vector3.down * fallSpeed;
                m_Animator.SetBool("Fall",true);

                if (transform.position.y <= 0)
                {
                    isJumping = false;
                    m_Animator.SetBool("Fall", false);
                    Debug.Log("착지" + isJumping);
                }
            }
        }
        if (transform.position.y > 30)
        {
            rb.velocity = Vector3.down * fallSpeed;
        }
    }

    void Jump()
    {
        rb.velocity = Vector3.up * jumpForce;
        Debug.Log("점프");
        isJumping = true;
    }
}
