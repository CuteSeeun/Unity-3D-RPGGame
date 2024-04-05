using System;
using System.Collections;
using HJ;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyS_AI : MonoBehaviour, IHp
{
    float chaseSpeed = 5f;
    public float detectionRange;
    public float attackRange;

    private EnemyHealthBar healthBar;
    private Animator m_Animator;
    private NavMeshAgent agent;
    private Transform player;
    private GetItemManager getItem;
    private SFX_Manager sound;
    public GameObject[] attackEffect;
    private bool isChasing;
    private bool isDeath;
    private int attackStack = 0;
    private float attackTimer;
    private bool isAttack;
    private bool isHit;
    private bool isAwake;

    // 추가된 코드: 감지 범위와 공격 범위를 시각화하기 위한 색상 변수
    public Color detectionColor = Color.yellow;
    public Color attackColor = Color.red;


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
    public float _hpMax = 50;

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
            healthBar.UpdateHealth(_hp, _hpMax, "스켈레톤 광전사");
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
            transform.rotation = hitRotation;
            transform.Rotate(0, 180, 0);

            if (powerAttack == false)
            {
                m_Animator.SetTrigger("HitA");
                isHit = true;
                // 맞은 방향 뒤로 밀리기
                Vector3 pushDirection = -transform.forward * 2f;
                ApplyPush(pushDirection);
            }
            else
            {
                m_Animator.SetTrigger("HitB");
                isHit = true;
                // 맞은 방향 뒤로 밀리기
                Vector3 pushDirection = -transform.forward * 4f;
                ApplyPush(pushDirection);
            }

            sound.VFX(18);
            DepleteHp(damage);
        }
    }
    void ApplyPush(Vector3 pushDirection)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(pushDirection, ForceMode.Impulse);
    }

    public void Hit(float damage)
    {
        DepleteHp(damage);
    }

    void Start()
    {
        sound = FindObjectOfType<SFX_Manager>();
        m_Animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        getItem = FindAnyObjectByType<GetItemManager>();
        agent.isStopped = true;
        agent.speed = chaseSpeed;
        agent.stoppingDistance = 3;
        _hp = _hpMax;
        healthBar = FindObjectOfType<EnemyHealthBar>();
        if (healthBar != null)
        {
            healthBar.UpdateHealth(_hp, _hpMax, "스켈레톤 광전사");
        }
    }

    void Update()
    {
        if (!isDeath)
        {
            if (!isHit)
            {
                // 일정 범위 내에 Enemy 태그를 가진 오브젝트를 감지하는 OverlapSphere를 사용합니다.
                Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);

                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Player") && !isAwake)
                    {
                        m_Animator.SetInteger("state", 1);
                        isAwake = true;
                    }
                }

                
                if (isAwake && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Basic"))
                {
                    m_Animator.SetInteger("state", 2);
                    if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn"))
                    {
                        //일어나는 모션이 끝나고 추격 시작
                        isChasing = true;
                        if (isChasing)
                        {
                            agent.speed = chaseSpeed;
                            if (!isAttack)
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
                            //공격범위 내 플레이어 감지 시 attackStack에 따라 공격모션 변경
                            if (Vector3.Distance(transform.position, player.position) < attackRange)
                            {
                                m_Animator.SetInteger("state", 2);
                                if (attackStack < 2 && attackTimer == 0 && !isAttack)
                                {
                                    Attack1();
                                    isAttack = true;
                                }
                                else if (attackStack == 2 && attackTimer == 0 && !isAttack)
                                {
                                    Attack2();
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
                    }
                }
            }

            if (isHit) // 피격 모션 중 플레이어를 추격하는것을 방지
            {
                agent.isStopped = true;
                agent.SetDestination(transform.position);
                m_Animator.SetInteger("state", 2);
                Invoke("Move", 0.5f);
            }
        }

        if (_hp <= 0 && !isDeath)
        {
            m_Animator.SetTrigger("isDeath");
            isDeath = true;
            Invoke("Death", 2);
            getItem.GetItem("뼈");
            getItem.GetItem("향신료");
            sound.VFX(19);
        }
    }

    void Move()
    {
        isHit = false;
    }

    void Attack1()
    {
        agent.isStopped = true;
        m_Animator.SetInteger("state", 2);
        m_Animator.SetBool("isAttack",true);
        attackEffect[0].SetActive(true);
        attackEffect[1].SetActive(true);
        attackTimer = 3;
    }

    void Attack2()
    {
        agent.isStopped = true;
        m_Animator.SetInteger("state", 2);
        m_Animator.SetBool("isAttackS", true);
        attackEffect[2].SetActive(true);
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

        Vector3 direction = transform.forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -_attackAngle / 2, 0) * direction;
        Vector3 rightBoundary = Quaternion.Euler(0, _attackAngle / 2, 0) * direction;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + new Vector3(0, 1, 0), transform.position + new Vector3(0, 1, 0) + leftBoundary * attackRange);
        Gizmos.DrawLine(transform.position + new Vector3(0, 1, 0), transform.position + new Vector3(0, 1, 0) + rightBoundary * attackRange);
    }
    
    public void AttackEnd()
    {
        agent.isStopped = false;
        isAttack = false;
        isChasing = true;
        attackStack++;
        attackEffect[0].SetActive(false);
        attackEffect[1].SetActive(false);
        attackEffect[2].SetActive(false);
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

    public LayerMask _attackLayerMask;
    float _attackAngleInnerProduct;
    public float _attackAngle = 45;
    float attackDamage = 5;
    float attackDamageA = 8;
    void Damage()
    {       
        // 공격 거리 내 모든 적 탐색
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, 1, 0),
                                                  attackRange,
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
                if (hit.collider.TryGetComponent(out IHp iHp))
                {
                    iHp.Hit(attackDamage, false, transform.rotation);
                }
            }
        }
    }
    void DamageA()
    {
        sound.VFX(0);
        // 공격 거리 내 모든 적 탐색
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, 1, 0),
                                                  attackRange,
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
                if (hit.collider.TryGetComponent(out IHp iHp))
                {
                    iHp.Hit(attackDamageA, true, transform.rotation);
                }
            }
        }
    }

    void SoundA()
    {
        sound.VFX(8);
    }
}