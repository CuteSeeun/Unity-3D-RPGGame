using System.Collections;
using HJ;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class MimicEnemyAI : MonoBehaviour, IHp
{
    float chaseSpeed = 3f;
    public float attackRange;

    private SFX_Manager sound;
    private Animator m_Animator;
    private NavMeshAgent agent;
    private Transform player;
    private EnemyHealthBar healthBar;
    private GetItemManager getItem;
    private bool isChasing;
    private bool isDeath;
    private bool isOpen;
    private float attackTimer;

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
            healthBar.UpdateHealth(_hp, _hpMax, "미믹");
        }
        onHpDepleted?.Invoke(amount);
    }

    public void RecoverHp(float amount)
    {

    }

    public void Hit(float damage, bool powerAttack, Quaternion hitRotation)
    {
        if (!isDeath && isOpen)
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
            sound.VFX(28);
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
        isChasing = true;
        agent.isStopped = false;
        agent.stoppingDistance = 3;
        attackTimer = 3;
        _hp = _hpMax;
        healthBar = FindObjectOfType<EnemyHealthBar>();
        if (healthBar != null)
        {
            healthBar.UpdateHealth(_hp, _hpMax, "미믹");
        }
    }

    void Update()
    {
        if (!isDeath)
        {
            if (isOpen)
            {
                if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
                {
                    agent.isStopped = false;
                }
                if (isChasing)
                {
                    agent.speed = chaseSpeed;
                    if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    {
                        agent.isStopped = false;
                        m_Animator.SetInteger("state", 0);
                        transform.LookAt(player.position);
                        agent.SetDestination(player.position);
                    }
                    else
                    {
                        agent.isStopped = true;
                        agent.SetDestination(transform.position);
                        transform.LookAt(transform.position + transform.forward);
                    }
                    if (Vector3.Distance(transform.position, player.position) < attackRange)
                    {
                        agent.stoppingDistance = 2;
                        if (attackTimer == 0)
                        {
                            Attack();
                        }
                    }
                }
                if (_hp <= 0)
                {
                    sound.VFX(29);
                    m_Animator.SetTrigger("isDeath");
                    isDeath = true;
                    Invoke("Death", 2);
                    getItem.GetItem("향신료");
                    getItem.GetItem("하급강화석");                 
                }
                if (attackTimer > 0)
                {
                    Debug.Log(attackTimer);
                    attackTimer -= Time.deltaTime;
                }
                if (attackTimer < 0)
                {
                    attackTimer = 0;
                }
            }
        }
    }

    void Attack()
    {
        agent.isStopped = true;
        attackTimer = 3;
        m_Animator.SetInteger("state", 0);
        m_Animator.SetTrigger("isAttack");
        // 공격 동작 구현
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
    }

    public void Death()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// 플레이어가 트리거 범위 내에서 상호작용 시 행동 시작
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                m_Animator.SetTrigger("isOpen");
                isOpen = true;
            }
        }
    }
    public LayerMask _attackLayerMask;
    float _attackAngleInnerProduct;
    public float _attackAngle = 45;
    float attackDamage = 5;
    void Damage()
    {
        sound.VFX(27);
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

    void OpenSound()
    {
        sound.VFX(25);
    }

    void ReadySound()
    {
        sound.VFX(26);
    }
}