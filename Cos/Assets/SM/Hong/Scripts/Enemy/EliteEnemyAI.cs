using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EliteEnemyAI : MonoBehaviour
{
    public float jumpForce = 50f;
    public float damageRadius = 5f;
    public int damageAmount = 10;
    public float fallSpeed = 100f;
    public int maxHealth;
    public int currentHealth;
          
    private Rigidbody rb;
    private Animator animator;
    private NavMeshAgent agent;
    private Transform player;
    private bool isWaiting = true;
    private bool isJumping;
    private bool isChasing;
    private bool isDeath;
    private float chaseSpeed = 5f;
    private float attackTimer;
    private float detectionRange;
    private float attackRange;
    private int attackStack = 0;

    public Color detectionColor = Color.yellow;
    public Color attackColor = Color.red;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if(isWaiting)
        {
            animator.SetInteger("state", 0);
            StartCoroutine("Waiting");
        }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn"))
        {
            isChasing = true;

            if (isChasing)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Golem"))
                {
                    agent.speed = chaseSpeed;
                    animator.SetInteger("state", 1);
                    agent.SetDestination(player.position);
                    transform.LookAt(player.position);
                }
                else
                {
                    agent.isStopped = true;
                    agent.SetDestination(transform.position);
                    transform.LookAt(transform.position + transform.forward);
                }
                if (Vector3.Distance(transform.position, player.position) < attackRange)
                {
                    animator.SetInteger("state", 0);
                    if (attackStack < 5 && attackStack != 2 && attackTimer == 0
                        && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Golem"))                       
                    {
                        Attack();
                    }
                    else if (attackStack == 5 && attackTimer == 0
                        && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Golem"))
                    {
                        Jump();
                    }
                }
                if(attackStack == 2 && attackTimer == 0)
                {
                    animator.SetTrigger("isRush");
                }
                if(attackTimer > 0)
                {
                    attackTimer -= Time.deltaTime;
                }
                if(attackTimer < 0)
                {
                    attackTimer = 0;
                }
            }
        }

        IEnumerator Waiting()
        {
            isWaiting = false;
            yield return new WaitForSeconds(3);

        }

        if (!isJumping )
        {
            animator.SetTrigger("Jump");
            isJumping = true;
            Invoke("Jump", 0.5f); // Update 메서드에서 호출하지 않습니다.
        }

        if(currentHealth <=  maxHealth / 2)
        {

        }

        if(currentHealth <= 0 && !isDeath)
        {
            isDeath = true;
        }
    }

    private void FixedUpdate()
    {
        // Fixed Update 메서드에서 공중에서 떨어지는 로직을 처리합니다.
        if (isJumping)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity = Vector3.down * fallSpeed;         
                animator.SetBool("Fall",true);
                
                if (transform.position.y <= 0)
                {
                    isJumping = false;
                    animator.SetBool("Fall", false);
                    Debug.Log("착지" + isJumping);
                }
            }
        }
        if(transform.position.y > 30)
        {
            rb.velocity = Vector3.down * fallSpeed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 공격 범위 시각화
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, damageRadius);

        Gizmos.color = detectionColor;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = attackColor;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    void Attack()
    {
        agent.isStopped = true;
        animator.SetTrigger("isAttack");
    }

    void AttackEnd()
    {
        agent.isStopped = false;
        attackStack++;
        isWaiting = true;
    }

    private void Jump()
    {       
        // 보스 몬스터를 공중으로 올리고 점프 상태로 설정
        rb.velocity = Vector3.up * jumpForce;
        Debug.Log("점프" + isJumping);
         // 점프 후 타이머 초기화
    }

    /* Animation Event로 호출됨
    public void DealDamage()
    {
        // 주변에 있는 플레이어에게 데미지를 줌
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
            }
        }
    }
    */
}
