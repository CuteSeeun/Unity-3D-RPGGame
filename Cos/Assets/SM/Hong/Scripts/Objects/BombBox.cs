using HJ;
using UnityEngine;
using UnityEngine.InputSystem.Switch;

/// <summary>
/// 체력을 1 보유하며 데미지를 받으면 2초 폭발하는 오브젝트.
/// </summary>
public class BombBox : MonoBehaviour, IHp
{
    Animator animator;
    public GameObject bombEffect;
    public GameObject hitEffect;
    public LayerMask _attackLayerMask;
    private float explosionRange = 3;
    private float damegeAmount = 5;
    private float _attackAngleInnerProduct;
    private float _attackAngle = 180;
    private bool isBomb;

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
    public float _hpMax = 1;

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
        onHpDepleted?.Invoke(amount);
    }

    public void RecoverHp(float amount)
    {

    }

    public void Hit(float damage, bool powerAttack, Quaternion hitRotation)
    {
        DepleteHp(damage);
    }

    public void Hit(float damage)
    {
        DepleteHp(damage);
    }

    private void Start()
    {
        _hp = _hpMax;
        animator = GetComponent<Animator>();       
    }
    private void Update()
    {
        //데미지를 받아 hp가 0이 되면 2초후 폭발. 데미지를 입을떄 이펙트를 발생함.
        if (_hp <= 0 && !isBomb)
        {
            isBomb = true;
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            animator.SetBool("isBomb", true);
            Invoke("Explosion", 2);
        }
    }
    
    /// <summary>
    /// 폭발 시 해당 범위 내 데미지를 부여 후 오브젝트 파괴.
    /// </summary>
    void Explosion()
    {
        Instantiate(bombEffect, transform.position, Quaternion.identity);
        // 공격 거리 내 모든 적 탐색
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, 1, 0),
                                                  explosionRange,
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
                    iHp.Hit(damegeAmount, true, Quaternion.identity);
                }
            }
        }
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        // 공격 범위 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,explosionRange);
    }
}