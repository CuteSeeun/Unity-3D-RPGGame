using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJ
{
    public class Sandbag : MonoBehaviour, IHp
    {
        // IHp -------------------------------------------------------------------------------------------------------
        public float hp
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

                _hp = value;

                if (value <= 0)
                    onHpMin?.Invoke();
                else if (value >= _hpMax)
                    onHpMax?.Invoke();
            }
        }

        [SerializeField] private float _hp;

        public float hpMax { get => _hpMax; }
        [SerializeField] private float _hpMax = 100;

        public event Action<float> onHpChanged;
        public event Action<float> onHpDepleted;
        public event Action<float> onHpRecovered;
        public event Action onHpMin;
        public event Action onHpMax;

        [SerializeField] GameObject _hitALight;
        [SerializeField] GameObject _hitBLight;

        public void DepleteHp(float amount)
        {
            if (amount <= 0)
                return;

            hp -= amount;
            onHpDepleted?.Invoke(amount);
        }

        public void RecoverHp(float amount)
        {
            hp += amount;
            onHpRecovered?.Invoke(amount);
        }

        public void Hit(float damage)
        {
            _hitALight.SetActive(true);
            Invoke("HitOff", 0.1f);
        }

        public void Hit(float damage, bool powerAttack, Quaternion hitRotation)
        {
            if (powerAttack == false)
            {
                _hitALight.SetActive(true);
                Invoke("HitAOff", 0.1f);
            }
            else
            {
                _hitBLight.SetActive(true);
                Invoke("HitBOff", 0.1f);
            }
        }
        // ----------------------------------------------------------------------------------------------------

        // Start is called before the first frame update
        void Start()
        {
            _hp = _hpMax;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void HitAOff()
        {
            _hitALight.SetActive(false);
        }

        private void HitBOff()
        {
            _hitBLight.SetActive(false);
        }
    }
}
