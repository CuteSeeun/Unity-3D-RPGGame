using System;
using System.Collections;
using HJ;
//using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BossEnemyPhase2 : MonoBehaviour, IHp
{
    Animator animator;
    Transform player;
    EnemyHealthBar healthBar;

    public GameObject missile;
    public Transform pos;
    public GameObject explosion;
    public GameObject arm;
    public GameObject lightning;
    public GameObject lightningRange;
    public GameObject tornado;
    public GameObject spawnEnemy;
    public GameObject deathEffect;
    private SFX_Manager sound;
    private float missileSpeed;
    private float spawnRange = 50f;

    private bool isSpawn;
    private bool isAttack;
    public bool isEnemy;
    private bool isDeath;
    private float attackTimer;
    private int attackStack;

    [SerializeField] GameObject _hitEffect;
    [SerializeField] int _hitSoundNum;
    [SerializeField] float _hitDelay;

    float IHp.hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = Mathf.Clamp(value, 0, _hpMax);

            if (_hp == value)
                return;

            if (value < 1)
            {
                onHpMin?.Invoke();
            }
            else if (value >= _hpMax)
                onHpMax?.Invoke();
        }
    }
    [SerializeField] public float _hp;

    public float hpMax { get => _hpMax; }
    public float _hpMax = 500;

    public event Action<float> onHpChanged;
    public event Action<float> onHpDepleted;
    public event Action<float> onHpRecovered;
    public event Action onHpMin;
    public event Action onHpMax;

    public void DepleteHp(float amount)
    {
        if (amount <= 0)
            return;

        _hp -= amount;
        if (healthBar != null)
        {
            healthBar.UpdateHealth(_hp, _hpMax, "타락한 마법사(폭주)");
        }
        onHpDepleted?.Invoke(amount);
    }

    public void RecoverHp(float amount)
    {

    }

    public void Hit(float damage, bool powerAttack, Quaternion hitRotation)
    {
        if (!isDeath)
        {
            StartCoroutine(Effect(_hitEffect, _hitSoundNum, _hitDelay, hitRotation));
            DepleteHp(damage);
        }
    }
    public void Hit(float damage)
    {
        DepleteHp(damage);
    }

    IEnumerator Effect(GameObject effect, int soundNum, float delay, Quaternion hitRotation)
    {
        GameObject effectInstanse = Instantiate(effect, transform.position, hitRotation);
        SFX_Manager.Instance.VFX(soundNum);

        yield return new WaitForSeconds(delay);
        Destroy(effectInstanse);
    }

    void Start()
    {
        sound = FindObjectOfType<SFX_Manager>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        lightningRange.SetActive(false);
        deathEffect.SetActive(false);
        _hp = _hpMax;
        healthBar = FindObjectOfType<EnemyHealthBar>();
        if (healthBar != null)
        {
            healthBar.UpdateHealth(_hp, _hpMax, "타락한 마법사(폭주)");
        }
    }

    void Update()
    {
        if (!isDeath)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn"))
            {
                isSpawn = true;
                if (isSpawn)
                {
                    //Idle 상태일때 플레이어를 바라봄
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    {
                        transform.LookAt(player.position);
                    }
                    if (attackTimer == 0 && !isAttack)
                    {
                        switch (attackStack) //attackStack에 의한 공격패턴 순회
                        {
                            case 0:
                                animator.SetTrigger("missile");
                                MissileAttack();
                                isAttack = true;
                                attackTimer = 8;
                                break;
                            case 1:
                                animator.SetTrigger("explosion");
                                transform.LookAt(transform.position + transform.forward);
                                ExplosionAttack();
                                isAttack = true;
                                attackTimer = 8;
                                break;
                            case 2:
                                animator.SetTrigger("armsUp");
                                transform.LookAt(transform.position + transform.forward);
                                SpawnArms(); SpawnArms(); SpawnArms(); SpawnArms(); SpawnArms();
                                isAttack = true;
                                attackTimer = 3;
                                break;
                            case 3:
                                animator.SetTrigger("lightning");
                                Invoke("Setcol", 3f);
                                transform.LookAt(transform.position + transform.forward);
                                lightningRange.SetActive(true);
                                Instantiate(lightning, transform.position, Quaternion.identity);
                                isAttack = true;
                                attackTimer = 8;
                                break;
                            case 4:
                                animator.SetTrigger("tornado");
                                transform.LookAt(transform.position + transform.forward);
                                SpawnTornado();
                                isAttack = true;
                                attackTimer = 8;
                                break;
                            case 5:
                                animator.SetTrigger("armsUp");
                                transform.LookAt(transform.position + transform.forward);
                                SpawnEnemy();
                                if (!isEnemy)
                                {
                                    attackStack = 0;
                                }
                                isAttack = true;
                                attackTimer = 3;
                                break;
                        }
                    }
                }
            }

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("LightningAttack_Boss"))
            {
                Collider col = GetComponent<SphereCollider>();
                col.enabled = false;
            }
            if (_hp <= 0)
            {
                animator.SetTrigger("isDeath");
                deathEffect.SetActive(true);
                isDeath = true;
                Invoke("Death", 2);
            }
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else if (attackTimer < 0)
            {
                attackTimer = 0;
            }
            // 일정 범위 내에 Enemy 태그를 가진 오브젝트를 감지하는 OverlapSphere를 사용합니다.
            Collider[] colliders = Physics.OverlapSphere(transform.position, spawnRange);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    // Enemy 태그를 가진 오브젝트가 감지되면 isEnemy를 true로 설정합니다.
                    isEnemy = true;
                    return;
                }
            }
            isEnemy = false;
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
    void Setcol()
    {
        Collider col = GetComponent<SphereCollider>();
        col.enabled = true;
        sound.VFX(43);
    }
    #region 보스 공격패턴
    void MissileAttack()
    {
        Invoke("Missile", 0.5f);
        Invoke("Missile", 1.5f);
        Invoke("Missile", 2.5f);
        Invoke("Missile", 3.5f);
        Invoke("Missile", 4.5f);
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
            projectile = Instantiate(missile, pos.position, Quaternion.identity);

            projectile.transform.LookAt(projectile.transform.position + direction);

            // 오브젝트를 방향으로 발사합니다.
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = direction.normalized * missileSpeed;
        }

        sound.VFX(41);
    }

    void ExplosionAttack()
    {
        Invoke("Explosion", 1f);
        Invoke("Explosion", 2f);
        Invoke("Explosion", 3f);
        Invoke("Explosion", 4f);
        Invoke("Explosion", 5f);
    }

    void Explosion()
    {
        Instantiate(explosion, player.position, Quaternion.identity);
        sound.VFX(38);
    }

    void SpawnArms()
    {
        Vector3 spawnPoint = GetRandomPatrolDestination();
        Instantiate(arm, spawnPoint, Quaternion.identity);
        sound.VFX(24);
    }

    Vector3 GetRandomPatrolDestination()
    {
        float spawnRadius = 20f;
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, 1);
        return hit.position;
    }

    void SpawnTornado()
    {
        float distance = 20f;
        sound.VFX(44);
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

        foreach (Vector3 direction in directions)
        {
            Vector3 spawnPosition = transform.position + direction.normalized * distance;

            Instantiate(tornado, spawnPosition, Quaternion.identity);
        }
    }

    void SpawnEnemy()
    {
        float distance = 10f;

        Vector3[] directions =
        {
        transform.right,
        -transform.right
        };

        foreach (Vector3 direction in directions)
        {
            Vector3 spawnPosition = transform.position + direction.normalized * distance;

            Instantiate(spawnEnemy, spawnPosition, Quaternion.identity);

        }
    }
    #endregion 보스 공격패턴
    //공격모션이 끝날 때 애니메이션 이벤트로 호출되는 함수
    void AttackEnd()
    {
        isAttack = false;
        if (!isEnemy)
        {
            attackStack++;
        }
        lightningRange.SetActive(false);
        if (attackStack > 5)
        {
            attackStack = 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }

    bool isDamage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IHp iHp))
        {
            if (!isDamage)
            {
                isDamage = true;
                StartCoroutine(Damage(iHp));
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isDamage = false;
    }

    public LayerMask _attackLayerMask;
    float _attackAngleInnerProduct;
    public float _attackAngle = 180;
    float attackDamage = 7;
    private IEnumerator Damage(IHp iHp)
    {
        while (isDamage)
        {
            // 공격 거리 내 모든 적 탐색
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, 1, 0),
                                                      3,
                                                      Vector3.up,
                                                      0,
                                                      _attackLayerMask);

            // 공격 각도에 따른 내적 계산
            _attackAngleInnerProduct = Mathf.Cos(_attackAngle * Mathf.Deg2Rad);

            // 내적으로 공격각도 구하기
            foreach (RaycastHit hit in hits)
            {
                if (Vector3.Dot((hit.transform.position - transform.position).normalized, transform.forward) > _attackAngleInnerProduct)
                {
                    // 데미지 주고, 데미지, 공격 방향, 파워어택 여부 전달                 
                    iHp.Hit(5);
                    
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
