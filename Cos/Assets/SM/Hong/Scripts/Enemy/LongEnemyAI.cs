using System.Collections;
using HJ;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class LongEnemyAI : MonoBehaviour, IHp
{
    float patrolSpeed = 2f;
    float patrolWaitTime = 3f;
    public float detectionRange;
    public float attackRange;
    float detectionAngle = 360f;

    public GameObject arrow;
    public GameObject owner;
    private Animator m_Animator;
    private NavMeshAgent agent;
    private Transform player;
    private EnemyHealthBar healthBar;
    private GetItemManager getItem;
    private SFX_Manager sound;
    private Vector3 patrolDestination;
    private bool isPatrolling;
    private bool isAiming;
    private bool isDeath;
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
        if (amount <= 0)
            return;

        _hp -= amount;
        if (healthBar != null)
        {
            healthBar.UpdateHealth(_hp, _hpMax, "스켈레톤 궁수");
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
            sound.VFX(18);

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
            healthBar.UpdateHealth(_hp, _hpMax, "스켈레톤 궁수");
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
                    if (collider.CompareTag("Player") && !isAiming)
                    {
                        isAiming = true;
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
                else if (isAiming)
                {
                    if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
                    {
                        agent.isStopped = false;
                        m_Animator.SetInteger("state", 2);
                        transform.LookAt(player.position);
                        Attack();
                    }
                    else
                    {
                        agent.isStopped = true;
                        agent.SetDestination(transform.position);
                        transform.LookAt(player.position);
                    }
                    if (Vector3.Distance(transform.position, player.position) < attackRange)
                    {

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
            getItem.GetItem("야채 바구니");
            sound.VFX(19);
        }
    }

    IEnumerator Patrol()
    {
        isPatrolling = false;
        yield return new WaitForSeconds(patrolWaitTime);
        if (!isAiming)
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
        transform.LookAt(player.position);
        m_Animator.SetInteger("state", 2);
        m_Animator.SetTrigger("isAttack");
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

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 playerDirection = other.transform.position - transform.position;
            float angle = Vector3.Angle(playerDirection, transform.forward);

            if (angle < detectionAngle && playerDirection.magnitude < detectionRange)
            {
                isAiming = true;
            }
        }
    } 

    public void AttackEnd()
    {
        agent.isStopped = false;
        isAiming = true;
    }
    /// <summary>
    /// 화살 오브젝트를 발사하는 함수. 화살을 발사하면 화살의 타겟과 화살의 오너를 지정.
    /// 바라보는 방향으로 화살을 발사
    /// </summary>
    public void Shoot()
    {
        //Instantiate(arrow,pos.position,Quaternion.identity);
        GameObject shootArrow = Instantiate(arrow, transform.position + transform.forward, Quaternion.identity);
        Arrow arrowScript = shootArrow.GetComponent<Arrow>();
        arrowScript.target = GameObject.FindWithTag("Player");
        arrowScript.owner = owner;
        transform.LookAt(transform.position + transform.forward);
        sound.VFX(22);
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}