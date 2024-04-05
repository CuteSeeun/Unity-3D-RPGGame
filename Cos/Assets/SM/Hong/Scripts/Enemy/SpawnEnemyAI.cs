using System.Collections;
using HJ;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// CloseEmeyAI 스크립트와 기본적으로 동일. Patrol state가 제외되고 Spawn모션이 추가
/// Spawn 후 즉시 Chasing state로 전환되어 플레이어를 추격
/// </summary>
public class SpawnEnemyAI : MonoBehaviour, IHp
{
    float chaseSpeed = 4f;
    public float attackRange;

    private Animator m_Animator;
    private NavMeshAgent agent;
    private Transform player;
    private EnemyHealthBar healthBar;
    private GetItemManager getItem;
    private SFX_Manager sound;
    public GameObject attackEffect;
    private bool isChasing;
    private bool isDeath;
    private bool isSpawn;
    private float attackTimer;
    private bool isHit;

    // 추가된 코드: 감지 범위와 공격 범위를 시각화하기 위한 색상 변수
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
    public float _hpMax = 30;

    public event System.Action<float> onHpChanged;
    public event System.Action<float> onHpDepleted;
    public event System.Action<float> onHpRecovered;
    public event System.Action onHpMin;
    public event System.Action onHpMax;

    public void DepleteHp(float amount)
    {
        if (amount <= 0)
            return;

        _hp -= amount;
        if (healthBar != null)
        {
            healthBar.UpdateHealth(_hp, _hpMax, "스켈레톤 미니언");
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
                // 맞은 방향 뒤로 밀리기
                Vector3 pushDirection = -transform.forward * 2f;
                ApplyPush(pushDirection);
            }
            else
            {
                m_Animator.SetTrigger("HitB");
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
        sound.VFX(24);
        m_Animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        getItem = FindAnyObjectByType<GetItemManager>();
        attackEffect.SetActive(false);
        isChasing = true;
        agent.isStopped = false;
        Invoke("Spawn", 3);
        _hp = _hpMax;
        healthBar = FindObjectOfType<EnemyHealthBar>();
        if (healthBar != null)
        {
            healthBar.UpdateHealth(_hp, _hpMax, "스켈레톤 미니언");
        }
    }

    void Update()
    {
        if (isSpawn)
        {
            if (isChasing)
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
                    {
                        Attack();
                    }
                }
            }
            if (_hp <= 0 && !isDeath)
            {
                m_Animator.SetTrigger("isDeath");
                sound.VFX(19);
                isDeath = true;
                Invoke("Death", 2);
                getItem.GetItem("뼈");
                getItem.GetItem("고기");
            }
            if(attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            if (attackTimer < 0)
            {
                attackTimer = 0;
            }
            if (isHit)
            {
                agent.isStopped = true;
                agent.SetDestination(transform.position);
                m_Animator.SetInteger("state", 2);
                Invoke("Move", 0.5f);
            }
        }
    }
    void Move()
    {
        isHit = false;
    }

    void Attack()
    {
        agent.isStopped = true;
        attackTimer = 3;
        m_Animator.SetInteger("state", 0);
        m_Animator.SetTrigger("isAttack");
        Invoke("EffectA", 0.5f);
        // 공격 동작 구현
    }

    void EffectA()
    {
        attackEffect.SetActive(true);
        sound.VFX(21);
    }

    void OnDrawGizmosSelected()
    {
        // 공격 범위 시각화
        Gizmos.color = attackColor;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void AttackEnd()
    {
        agent.isStopped = false;
        isChasing = true;
        attackEffect.SetActive(false);
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    public void Spawn()
    {
        m_Animator.SetTrigger("isSpawn");
        isSpawn = true;
    }
    public LayerMask _attackLayerMask;
    float _attackAngleInnerProduct;
    public float _attackAngle = 45;
    float attackDamage = 5;
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
}