using System;
using System.Collections;
using HJ;
using UnityEditor;
//using UnityEditor.Experimental.Rendering;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EliteEnemy : MonoBehaviour, IHp
{
    float chaseSpeed = 5f;
    public float detectionRange;
    public float attackRange;
    private float jumpForce = 50;
    private float fallSpeed = 100;
    private float rushSpeed = 15;
    private float stopThreshold = 0.1f; // 오브젝트가 멈춘 것으로 간주하는 속도 임계값


    private EnemyHealthBar healthBar;
    private Animator m_Animator;
    private NavMeshAgent agent;
    private Transform player;
    private Rigidbody rb;
    private GetItemManager getItem;
    private SFX_Manager sound;
    public GameObject grounded;
    public GameObject jumpImpact;
    public GameObject rushImpact;
    public GameObject jumpEffect;
    public GameObject isCrushEffect;
    public GameObject effectR;
    public GameObject effectL;
    public GameObject effectB;
    public ParticleSystem isRush;
    private bool isChasing;
    private bool isDeath;
    private bool isAttack;
    private bool isJumping;
    private bool jump;
    private bool isStand;
    private float attackTimer;
    private int attackStack = 0;

    [SerializeField] GameObject _hitEffect;
    [SerializeField] int _hitSoundNum;
    [SerializeField] float _hitDelay;

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
    public float _hpMax = 300;

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
            healthBar.UpdateHealth(_hp, _hpMax, "스켈레톤 골렘");
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
            DepleteHp(damage);
            StartCoroutine(Effect(_hitEffect, _hitSoundNum, _hitDelay, hitRotation));
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
        m_Animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        getItem = FindAnyObjectByType<GetItemManager>();
        isChasing = true;
        agent.isStopped = false;
        jumpImpact.SetActive(false);
        rushImpact.SetActive(false);
        jumpEffect.SetActive(false);
        isCrushEffect.SetActive(false);
        isRush.Stop();
        _hp = _hpMax;
        healthBar = FindObjectOfType<EnemyHealthBar>();
        if (healthBar != null)
        {
            healthBar.UpdateHealth(_hp, _hpMax, "스켈레톤 골렘");
        }
    }

    void Update()
    {
        //골렘이 일어나는 모션이 끝났을때 행동 시작
        if (isStand && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("StandUp_Golem"))
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

                if (isChasing)
                {
                    agent.speed = chaseSpeed;
                    //플레이어를 공격중이지 않을 때 플레이어를 바라보고 walk state를 유지.
                    if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
                        || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    {
                        agent.isStopped = false;
                        m_Animator.SetInteger("state", 1);
                        transform.LookAt(player.position);
                        agent.SetDestination(player.position);
                        agent.stoppingDistance = 3;
                    }
                    //돌진 준비 모션일때도 플레이어를 계속 바라보게 함.
                    else if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("isRush_Golem"))
                    {
                        transform.LookAt(player.position);
                        agent.isStopped = true;
                    }
                    else // 플레이어를 공격중일때 공격방향을 바라보고 추격을 멈춤.
                    {
                        agent.isStopped = true;
                        agent.SetDestination(transform.position);
                        transform.LookAt(transform.position + transform.forward);
                    }
                    //공격 범위 내에 플레이어 감지 시 attackStack에 따라 공격패천 순회
                    if (Vector3.Distance(transform.position, player.position) < attackRange
                        && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Golem"))
                    {
                        agent.isStopped = true;
                        m_Animator.SetInteger("state", 0);
                        if (attackTimer == 0 && !isAttack && !jump)
                            switch (attackStack)
                            {
                                case 0:
                                    Attack();
                                    isCrushEffect.SetActive(false);
                                    isAttack = true;
                                    break;
                                case 1:
                                    Attack();
                                    isAttack = true;
                                    break;
                                case 2:
                                    Rush();
                                    isAttack = true;
                                    break;
                                case 3:
                                    Attack();
                                    rushImpact.SetActive(false);
                                    isAttack = true;
                                    break;
                                case 4:
                                    Attack();
                                    isAttack = true;
                                    break;
                                case 5:
                                    jump = true;
                                    agent.enabled = false;
                                    rb.isKinematic = false;
                                    m_Animator.SetTrigger("Jump");
                                    jumpEffect.SetActive(true);
                                    Invoke("Jump", 0.5f);
                                    isAttack = true;
                                    break;
                                case 6:
                                    Crush();
                                    jumpImpact.SetActive(false);
                                    isAttack = true;
                                    break;
                            }
                    }
                }
            }
        }
        if (isJumping)
        {
            if (rb.velocity.y < 0)
            {              
                rb.velocity = Vector3.down * fallSpeed;
                m_Animator.SetBool("Fall", true);

                if (transform.position.y <= 11)
                {
                    DamageJ();
                    isJumping = false;
                    jump = false;
                    isAttack = false;
                    agent.enabled = true;
                    m_Animator.SetBool("Fall", false);
                    Debug.Log("착지" + isJumping);
                    jumpImpact.SetActive(true);
                    jumpEffect.SetActive(false);
                }
            }
        }
        if (transform.position.y > 41)
        {
            transform.position = new Vector3((player.position  - (player.forward * 2)).x, 
                transform.position.y,(player.position - (player.forward * 2)).z);
            rb.velocity = Vector3.down * fallSpeed;
            sound.VFX(36);
        }
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else if (attackTimer < 0)
        {
            attackTimer = 0;
        }
        if (_hp <= 0 && !isDeath)
        {
            int acc = Random.Range(0, 3);
            m_Animator.SetTrigger("isDeath");
            agent.isStopped = true;
            isDeath = true;
            Invoke("Death", 2);
            getItem.GetItem("상급강화석");
            switch(acc)
            {
                case 0:
                    getItem.GetItem("반지");
                    break;
                case 1:
                    getItem.GetItem("목걸이");
                    break;
                case 2:
                    getItem.GetItem("귀걸이");
                    break;
            }
        }
        if(m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Rush_Golem"))
        {
            agent.isStopped = false;
            m_Animator.SetInteger("state", 0);
            rushImpact.SetActive(true);
            Debug.Log("돌진");
            isRush.Stop();
            Vector3 rush = transform.forward;
            rb.velocity = rush * rushSpeed;
            sound.VFX(17);
            transform.LookAt(transform.position + transform.forward);
        }
    }
    #region 보스 공격패턴
    void Attack()
    {
        m_Animator.SetTrigger("isAttack");
        attackTimer = 3;
        // 공격 동작 구현
    }

    void Rush()
    {
        m_Animator.SetTrigger("isRush");
        sound.VFX(34);
        isRush.Play();
        attackTimer = 6;
    }

    void Jump()
    {
        rb.velocity = Vector3.up * jumpForce;
        sound.VFX(35);
        Debug.Log("점프");
        isJumping = true;
        attackTimer = 3;
        attackStack++;
    }

    void Crush()
    {
        m_Animator.SetTrigger("isCrush");
        isCrushEffect.SetActive(true);
        attackTimer = 5;
        sound.VFX(34);
        Invoke("Grounded", 2f);
    }

    void Grounded()
    {
        grounded.SetActive(true);
        Invoke("Disgrounded", 0.7f);
    }

    void Disgrounded()
    {
        grounded.SetActive(false);
    }
    #endregion 보스 공격패턴
    void OnDrawGizmosSelected()
    {
        // 감지 범위 시각화
        Gizmos.color = detectionColor;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 공격 범위 시각화
        Gizmos.color = attackColor;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void AttackEnd()
    {
        m_Animator.SetInteger("state", 0);
        agent.isStopped = false;
        isChasing = true;
        isAttack = false;
        jump = false;
        effectR.SetActive(false);
        effectL.SetActive(false);
        effectB.SetActive(false);
        //rb.isKinematic = false;
        attackStack++;
        if(attackStack > 6)
        {
            attackStack = 0;
        }
        if(attackStack == 2 || attackStack == 5)
        {
            attackRange = 100;
        }
        else
        {
            attackRange = 5;
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
    #region 데미지 함수
    public LayerMask _attackLayerMask;
    float _attackAngleInnerProduct;
    public float _attackAngle = 45;
    float attackDamage = 7;
    float attackDamageA = 10;
    float attackDamageR = 20;
    float attackDamageJ = 15;
    float attackDamageC = 30;
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
    void DamageR()
    {
        // 공격 거리 내 모든 적 탐색
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, 1, 0),
                                                  attackRange,
                                                  Vector3.up,
                                                  0,
                                                  _attackLayerMask);

        // 공격 각도에 따른 내적 계산
        _attackAngleInnerProduct = Mathf.Cos(30 * Mathf.Deg2Rad);

        // 내적으로 공격각도 구하기
        foreach (RaycastHit hit in hits)
        {
            if (Vector3.Dot((hit.transform.position - transform.position).normalized, transform.forward) > _attackAngleInnerProduct)
            {
                // 데미지 주고, 데미지, 공격 방향, 파워어택 여부 전달
                if (hit.collider.TryGetComponent(out IHp iHp))
                {
                    iHp.Hit(attackDamageR, true, transform.rotation);
                }
            }
        }
    }
    void DamageJ()
    {
        // 공격 거리 내 모든 적 탐색
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, 1, 0),
                                                  6,
                                                  Vector3.up,
                                                  0,
                                                  _attackLayerMask);

        // 공격 각도에 따른 내적 계산
        _attackAngleInnerProduct = Mathf.Cos(180 * Mathf.Deg2Rad);

        // 내적으로 공격각도 구하기
        foreach (RaycastHit hit in hits)
        {
            if (Vector3.Dot((hit.transform.position - transform.position).normalized, transform.forward) > _attackAngleInnerProduct)
            {
                // 데미지 주고, 데미지, 공격 방향, 파워어택 여부 전달
                if (hit.collider.TryGetComponent(out IHp iHp))
                {
                    iHp.Hit(attackDamageJ, true, transform.rotation);
                }
            }
        }
    }
    void DamageC()
    {
        sound.VFX(36);
        // 공격 거리 내 모든 적 탐색
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, 1, 0),
                                                  16,
                                                  Vector3.up,
                                                  0,
                                                  _attackLayerMask);

        // 공격 각도에 따른 내적 계산
        _attackAngleInnerProduct = Mathf.Cos(45 * Mathf.Deg2Rad);

        // 내적으로 공격각도 구하기
        foreach (RaycastHit hit in hits)
        {
            if (Vector3.Dot((hit.transform.position - transform.position).normalized, transform.forward) > _attackAngleInnerProduct)
            {
                // 데미지 주고, 데미지, 공격 방향, 파워어택 여부 전달
                if (hit.collider.TryGetComponent(out IHp iHp))
                {
                    iHp.Hit(attackDamageC, true, transform.rotation);
                }
            }
        }
    }
    #endregion 데미지 함수
    void EffectR()
    {
        effectR.SetActive(true);
        sound.VFX(31);
    }

    void EffectL()
    {
        effectL.SetActive(true);
        sound.VFX(32);
    }

    void EffectB()
    {
        effectB.SetActive(true);
        sound.VFX(33);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isJumping)
        {
            
            if(m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Rush_Golem"))
            {               
                DamageR();

                if (rb.velocity.magnitude < stopThreshold)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    Debug.Log("충돌");
                }

                rb.isKinematic = true;
            }

            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collider col = GetComponent<SphereCollider>();

            m_Animator.SetTrigger("isStand");
            isStand = true;
            col.enabled = false;
        }
    }
}