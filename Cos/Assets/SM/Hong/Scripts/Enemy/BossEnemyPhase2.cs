using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyPhase2 : MonoBehaviour
{
    Animator animator;
    Transform player;

    public GameObject missile;
    public GameObject explosion;
    private float missileSpeed;

    private bool isSpawn;
    private bool isAttack;
    private float attackTimer;
    private int attackStack;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn"))
        {
            isSpawn = true;
            if(isSpawn)
            {
                if(animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn"))
                {
                    transform.LookAt(player.position);
                }
                if(attackTimer == 0 && !isAttack)
                {
                    switch(attackStack)
                    {
                        case 0:
                            animator.SetTrigger("missile");
                            isAttack = true;
                            break;
                        case 1:
                            animator.SetTrigger("explosion");
                            isAttack = true;

                            break;
                        case 2:
                            isAttack = true;

                            break;
                        case 3:
                            isAttack = true;

                            break;
                        case 4:
                            isAttack = true;

                            break;
                        case 5:
                            isAttack = true;

                            break;
                    }
                }
                if(animator.GetCurrentAnimatorStateInfo(0).IsName("MissileAttack_Boss"))
                {
                    InvokeRepeating("Missile",0.5f, 1f);
                }
                if(animator.GetCurrentAnimatorStateInfo(0).IsName("Explosion_Boss"))
                {
                    transform.LookAt(transform.position + transform.forward);
                    InvokeRepeating("Explosion", 0f, 1f);
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
    }

    void Missile()
    {
        transform.LookAt(player.position);
        GameObject projectile;

        // 발사할 방향들을 배열에 저장합니다.
        Vector3[] directions =
        {
        transform.forward,
        -transform.forward,
        transform.right,
        -transform.right,
        transform.right + transform.forward,
        transform.right - transform.forward,
        -transform.right + transform.forward,
        -transform.right - transform.forward
        };

        // 저장된 방향들로 오브젝트를 발사합니다.
        foreach (Vector3 direction in directions)
        {
            projectile = Instantiate(missile, transform.position, Quaternion.identity);

            projectile.transform.LookAt(projectile.transform.position + direction);

            // 오브젝트를 방향으로 발사합니다.
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = direction.normalized * missileSpeed;
        }
    }

    void Explosion()
    {
        Instantiate(explosion, player.position, Quaternion.identity);
    }
}
