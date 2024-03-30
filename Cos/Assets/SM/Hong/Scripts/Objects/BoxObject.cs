using System.Collections;
using System.Collections.Generic;
using HJ;
using UnityEngine;

public class BoxObject : MonoBehaviour, IHp
{
    bool isDestroy;
    public GameObject potion;
    public GameObject item;
    public GameObject destroyEffect;

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
    void Start()
    {
        _hp = _hpMax;
    }

    void Update()
    {
        if(_hp <= 0 && !isDestroy)
        {
            isDestroy = true;
            ItemSpawn();
            Instantiate(destroyEffect,transform.position,Quaternion.identity);
        }
    }

    void ItemSpawn()
    {
        int spawnitem = Random.Range(0, 5);
        if(spawnitem == 0 )
        {
            Instantiate(potion, transform.position, Quaternion.identity);

        }
        else
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }
        Destroy();
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
