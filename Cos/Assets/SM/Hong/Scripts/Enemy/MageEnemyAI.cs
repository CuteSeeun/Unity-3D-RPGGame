using System.Collections;
using HJ;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum AI
{
    None,
    Idle,
    Patrol,
    FollowTarget,
    AttackTarget,
}

public class MageEnemyAI : MonoBehaviour, IHp
{
    float patrolSpeed = 2f;
    float chaseSpeed = 2f;
    float patrolWaitTime = 3f;
    public float detectionRange;
    public float attackRange;

    private SFX_Manager sound;
    private Animator m_Animator;
    private NavMeshAgent agent;
    private Transform player;
    private EnemyHealthBar healthBar;
    private GetItemManager getItem;
    private Vector3 patrolDestination;
    private bool isPatrolling;
    private bool isChasing;
    private bool isDeath;
    private bool isAttack;
    public bool isAct;

    public GameObject magic;
    public GameObject owner;
    [SerializeField] LayerMask targetMask;

    // 추가된 코드: 감지 범위와 공격 범위를 시각화하기 위한 색상 변수
    public Color detectionColor = Color.yellow;
    public Color attackColor = Color.red;

    public event System.Action<float> onHpChanged;
    public event System.Action<float> onHpDepleted;
    public event System.Action<float> onHpRecovered;
    public event System.Action onHpMin;
    public event System.Action onHpMax;

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

    public void DepleteHp(float amount)
    {
        if (amount <= 0)
            return;

        _hp -= amount;
        if (healthBar != null)
        {
            healthBar.UpdateHealth(_hp, _hpMax, "스켈레톤 마법사");
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
        patrolDestination = GetRandomPatrolDestination();
        isPatrolling = true;
        agent.isStopped = false;
        agent.speed = patrolSpeed;
        _hp = _hpMax;
        healthBar = FindObjectOfType<EnemyHealthBar>();
        if (healthBar != null)
        {
            healthBar.UpdateHealth(_hp, _hpMax, "스켈레톤 마법사");
        }
    }

    void Update()
    {
        if (isAct)
        {
            if (!isDeath)
            {
                // 일정 범위 내에 Enemy 태그를 가진 오브젝트를 감지하는 OverlapSphere를 사용합니다.
                Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, targetMask);

                if (colliders.Length > 0)
                {
                    foreach (Collider collider in colliders)
                    {
                        if (collider.CompareTag("Player") && !isChasing)
                        {
                            isChasing = true;
                            return;
                        }
                    }
                }


                if (isPatrolling)
                {
                    if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    {
                        m_Animator.SetInteger("state", 0);
                        isPatrolling = false;
                        StartCoroutine(Patrol());
                    }
                }
                else if (isChasing)
                {
                    agent.speed = chaseSpeed;
                    if (GameObject.FindWithTag("Magic") == true)
                        transform.LookAt(player.position);

                    if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !isAttack)
                    {
                        m_Animator.SetInteger("state", 1);
                        transform.LookAt(player.position);
                        agent.SetDestination(player.position);
                    }
                    else
                    {
                        agent.SetDestination(transform.position);
                        transform.LookAt(transform.position + transform.forward);
                    }
                    if ((Vector3.Distance(transform.position, player.position) < attackRange) && !isAttack)
                    {
                        Attack();
                        isAttack = true;
                    }
                }
            }
        }
        if (_hp <= 0 && !isDeath)
        {
            m_Animator.SetTrigger("isDeath");
            isDeath = true;
            Invoke("Death", 2);
            getItem.GetItem("뼈");
            getItem.GetItem("호박");
        }
    }

    IEnumerator Patrol()
    {
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
        agent.isStopped = true;
        agent.SetDestination(transform.position);
        m_Animator.SetInteger("state", 2);
        m_Animator.SetBool("isAttack",true);
        // 공격 동작 구현
    }

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
    }
    /// <summary>
    /// 발사할 MagicBall을 생성하여 타겟과 오너를 지정.
    /// 바라보는 방향으로 MagicBall을 발사
    /// </summary>
    public void Magic()
    {
        GameObject shootMagic = Instantiate(magic, transform.position + transform.forward, Quaternion.identity);
        MagicBall magicScript = shootMagic.GetComponent<MagicBall>();
        magicScript.target = GameObject.FindWithTag("Player");
        magicScript.owner = owner;
        transform.LookAt(transform.position + transform.forward);
        sound.VFX(23);
    }

    public void AttackEnd()
    {
        agent.isStopped = false;       
        m_Animator.SetBool("isAttack",false);
        Invoke("ReA",4);
    }

    void ReA()
    {
        isAttack = false;
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}