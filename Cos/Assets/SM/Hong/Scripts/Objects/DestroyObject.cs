using HJ;
using UnityEngine;

/// <summary>
/// hp를 1을 가지고, 데미지를 입으면 파괴이펙트를 생성하며 파괴되는 오브젝트
/// </summary>
public class DestroyObject : MonoBehaviour, IHp
{
    bool isDestroy;
    public GameObject bombEffect;

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
    }
    private void Update()
    {
        if (_hp <= 0 && !isDestroy)
        {
            isDestroy = true;
            Instantiate(bombEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}