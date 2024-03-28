using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ;
using System;

public class B_ItemGate : MonoBehaviour, IHp
{
    public float hp
    {
        get { return _hp; }
        set
        {
            _hp = Mathf.Clamp(value, 0, _hpMax); // _hp를 value를 0~_hpMax 사잇값으로 변환해서 대입

            if (_hp == value) // 문제없이 들어가면 return
                return;

            if (value < 1)
            {
                onHpMin?.Invoke();
            }
            else if (value >= _hpMax)
                onHpMax?.Invoke();
        }
    }
    private float _hp;

    public float hpMax { get => _hpMax;  }
    private float _hpMax = 1;

    public event Action<float> onHpChanged;
    public event Action<float> onHpDepleted;
    public event Action<float> onHpRecovered;
    public event Action onHpMin;
    public event Action onHpMax;

    public void DepleteHp(float amount)
    {
        hp -= amount;
    }

    public void Hit(float damage, bool powerAttack, Quaternion hitRotation)
    {
        DepleteHp(damage);
        Debug.Log(1);
    }

    public void Hit(float damage)
    {
        DepleteHp(damage);
    }

    public void RecoverHp(float amount)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        _hp = _hpMax;
        onHpMin += () => Die();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Die()
    {
        Destroy(this);
    }
}
