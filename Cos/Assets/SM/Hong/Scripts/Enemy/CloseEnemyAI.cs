using System.Collections;
using HJ;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CloseEnemyAI : MonoBehaviour, IHp
{
    float patrolSpeed = 2f;
    float chaseSpeed = 4f;
    float patrolWaitTime = 3f;
    public float detectionRange;
    public float attackRange;

    private EnemyHealthBar healthBar;
    private Animator m_Animator;
    private NavMeshAgent agent;
    private Transform player;
    private GetItemManager getItem;
    private SFX_Manager sound;
    public GameObject attackEffect;
    private Vector3 patrolDestination;
    private bool isPatrolling;
    private bool isChasing;
    private bool isDeath;
    private float attackTimer;
    private bool isHit;
    public bool isAct;

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
    public float _hpMax = 30;

    public event System.Action<float> onHpChanged;
    public event System.Action<float> onHpDepleted;
    public event System.Action<float> onHpRecovered;
    public event System.Action onHpMin;
    public event System.Action onHpMax;

    public void DepleteHp(float amount)
    {
        if(amount <= 0)
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
        patrolDestination = GetRandomPatrolDestination();
        getItem = FindAnyObjectByType<GetItemManager>();
        attackEffect.SetActive(false);
        isPatrolling = true;
        agent.isStopped = false;
        agent.speed = patrolSpeed;
        _hp = _hpMax;
        healthBar = FindObjectOfType<EnemyHealthBar>();
        if(healthBar != null )
        {
            healthBar.UpdateHealth(_hp, _hpMax, "스켈레톤 미니언");
        }
    }

    void Update()
    {
        if (isAct)
        {
            if (!isDeath)
            {
                // 일정 범위 내에 Enemy 태그를 가진 오브젝트를 감지하는 OverlapSphere를 사용합니다.
                Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);

                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Player") && !isChasing)
                    {
                        isChasing = true;
                    }
                }

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
                    //플레이어를 공격중이지 않을때 플레이어를 바라보면서 state를 Run으로 유지.
                    if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                    {
                        agent.isStopped = false;
                        m_Animator.SetInteger("state", 2);
                        transform.LookAt(player.position);
                        agent.SetDestination(player.position);
                        agent.stoppingDistance = 2;
                    }
                    else //플레이어를 공격중일때 공격방향을 바라보고 플레이어에게 이동하는것을 멈춤.
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
        if (isHit)
        {
            agent.isStopped = true;
            agent.SetDestination(transform.position);
            m_Animator.SetInteger("state", 2);
            Invoke("Move", 0.5f);
        }
        if (_hp <= 0 && !isDeath)
        {
            m_Animator.SetTrigger("isDeath");
            sound.VFX(19);
            agent.isStopped = true;
            isDeath = true;
            Invoke("Death", 2);
            getItem.GetItem("뼈");
            getItem.GetItem("고기");
        }
    }

    void Move()
    {
        isHit = false;
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

    /// <summary>
    /// Patrol 상태일 때 랜덤한 좌표 생성 후 해당 위치로 이동
    /// 아래 함수는 랜덤한 좌표 생성 함수
    /// </summary>
    /// <returns></returns>
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
        isChasing = true;
        attackEffect.SetActive(false);
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    public LayerMask _attackLayerMask;
    float _attackAngleInnerProduct;
    public float _attackAngle = 45;
    float attackDamage = 5;
    void Damage()
    {
        attackEffect.SetActive(true);
        sound.VFX(21);
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