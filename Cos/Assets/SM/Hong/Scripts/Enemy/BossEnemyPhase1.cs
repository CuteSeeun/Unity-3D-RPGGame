using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemyPhase1 : MonoBehaviour
{
    float chaseSpeed = 8f;
    public float detectionRange;
    public float attackRange;
    float detectionAngle = 360f;
    public int maxHp;
    private int currentHp;
    public float skulSpeed;


    Animator animator;
    NavMeshAgent agent;
    Transform player;
    public GameObject skulMissile;
    public GameObject explosion;
    public ParticleSystem fire;
    private bool isChasing;
    private bool isDeath;
    private bool isAttack;
    private bool isSkul;
    private float attackTimer;
    private int attackStack = 0;

    public Color detectionColor = Color.yellow;
    public Color attackColor = Color.red;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        fire = GetComponentInChildren<ParticleSystem>();
        player = GameObject.FindWithTag("Player").transform;
        currentHp = maxHp;
        agent.stoppingDistance = 3;
        fire.Stop();
        explosion.SetActive(false);
    }

    void Update()
    {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Sit_Boss")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Stand_Boss")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Start_Boss"))
        {
            if(isChasing)
            {
                agent.speed = chaseSpeed;
                if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_Boss")
                    || animator.GetCurrentAnimatorStateInfo(0).IsName("Run_Boss"))
                {
                    agent.isStopped = false;
                    animator.SetInteger("state", 1);
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
                if(Vector3.Distance(transform.position, player.position) < attackRange
                    && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Boss"))
                {
                    agent.isStopped = true;
                    animator.SetInteger("state", 0);
                    if(attackTimer == 0 && !isAttack)
                    {
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
                                Charge();
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
                                Tel();
                                isAttack = true;
                                break;
                            case 6:
                                Tel();
                                isAttack = true;
                                break;
                        }
                    }
                }
                if(animator.GetCurrentAnimatorStateInfo(0).IsName("SkulMissile_Boss") && !isSkul)
                {
                    isSkul = true;
                    Invoke("SkulMissileCross", 1f);
                    Invoke("SkulMissileX", 2f);
                    Invoke("SkulMissileCross", 3f);
                    Invoke("SkulMissileX", 4f);
                }
            }
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
            animator.SetTrigger("isDeath");
            agent.isStopped = true;
            isDeath = true;
            Invoke("Death", 2);
        }      
    }

    void Attack()
    {
        animator.SetTrigger("isAttack");
        attackTimer = 3;
    }

    void Charge()
    {
        animator.SetTrigger("isCharge");
        attackTimer = 4;
        explosion.SetActive(true);
    }

    void Tel()
    {
        attackTimer = 5;
        animator.SetTrigger("isTel");
        if(attackStack == 5)
        {
            Invoke("Raid", 0.5f);
        }
        else if(attackStack == 6)
        {
            Invoke("Skul", 0.5f);
        }
    }

    void Raid()
    {
        transform.position = player.position - player.forward * 2f;
        transform.LookAt(player.position);
        animator.SetTrigger("isRaid");
    }

    void Skul()
    {
        transform.position = new Vector3(0, 0, 0);
        transform.LookAt(transform.position - Vector3.forward);
        animator.SetTrigger("isSkul");
        attackTimer = 7;
        Invoke("Fire", 0.4f);
    }

    void Fire()
    {
        fire.Play();
        Invoke("FireStop", 4f);
    }

    void FireStop()
    {
        fire.Stop();
    }

    void SkulMissileCross()
    {
        GameObject projectile;

        // 발사할 방향들을 배열에 저장합니다.
        Vector3[] directions =
            { transform.forward, -transform.forward, transform.right, -transform.right };

        // 저장된 방향들로 오브젝트를 발사합니다.
        foreach (Vector3 direction in directions)
        {
            projectile = Instantiate(skulMissile, transform.position, Quaternion.identity);

            projectile.transform.LookAt(projectile.transform.position + direction);

            // 오브젝트를 방향으로 발사합니다.
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = direction.normalized * skulSpeed;
        }
    }

    void SkulMissileX()
    {
        GameObject projectile;

        // 발사할 방향들을 배열에 저장합니다.
        Vector3[] directions = 
            { transform.right + transform.forward, transform.right - transform.forward,
            -transform.right + transform.forward, -transform.right - transform.forward };

        // 저장된 방향들로 오브젝트를 발사합니다.
        foreach (Vector3 direction in directions)
        {
            projectile = Instantiate(skulMissile, transform.position, Quaternion.identity);

            projectile.transform.LookAt(projectile.transform.position + direction);

            // 오브젝트를 방향으로 발사합니다.
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = direction.normalized * skulSpeed;

        }
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
        animator.SetInteger("state", 0);
        agent.isStopped = false;
        isChasing = true;
        isAttack = false;
        isSkul = false;
        explosion.SetActive(false);
        attackStack++;
        if (attackStack > 6)
        {
            attackStack = 0;
        }
        if (attackStack == 5 || attackStack == 6)
        {
            attackRange = 100;
        }
        else
        {
            attackRange = 4;
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
