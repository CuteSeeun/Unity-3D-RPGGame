using System;
using UnityEngine;

namespace HJ
{
    public class Missile : MonoBehaviour
    {
        public void Start()
        {
            MissileMoveStart();
        }

        public void FixedUpdate()
        {
            MissileMoveFixedUpdate();
        }

        [Header ("Missile Info")] //==================================================================
        [SerializeField] private float _missileSpeed;
        [SerializeField] private float _missileTimer;

        private void MissileMoveStart()
        {
            Invoke("TimeOut", _missileTimer);
        }
        private void MissileMoveFixedUpdate()
        {
            transform.position += _missileSpeed * transform.forward * Time.fixedDeltaTime;
        }
        private void TimeOut()
        {
            Destroy(gameObject);
            if (_isExplosive)
                Explosion();
        }

        [Header("Missile Attack")] //==================================================================
        [SerializeField] bool _isPiercing;
        [SerializeField] bool _isExplosive;
        [SerializeField] GameObject _explosionEffect;
        [SerializeField] float _explosionDestroyDelay;

        public float attack { set => _attack = value; }
        [SerializeField] float _attack;
        [SerializeField] float _attackDamageRate;

        [SerializeField] float _attackRange;
        [SerializeField] float _attackAngle;
        [SerializeField] float _attackAngleInnerProduct;
        [SerializeField] LayerMask _attackLayerMask;

        [SerializeField] bool _isPowerAttack;

        [SerializeField] LayerMask _layerMaskWall;

        private void OnTriggerEnter(Collider coliderHit)
        {
            if (coliderHit.gameObject.layer == 12)
            {
                if (_isPiercing == false)
                {
                    Destroy(gameObject);
                }


                if (_isExplosive == false)
                {
                    Hit(coliderHit);
                }
                else // (_isExplosive == true)
                {
                    Explosion();
                }
            }

            if (coliderHit.gameObject.layer == _layerMaskWall) // 벽에 박으면 터짐
            {
                Destroy(gameObject);

                if (_isExplosive)
                    Explosion();
            }
            
        }

        private void Explosion()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, _attackRange, transform.up, 0, _attackLayerMask);

            //_attackAngleInnerProduct = Mathf.Cos(_attackAngle * Mathf.Deg2Rad);

            foreach (RaycastHit hit in hits)
            {
                Hit(hit.collider);
            }

            GameObject effectInstanse = Instantiate(_explosionEffect, transform.position, transform.rotation);
            //SFX_Manager.Instance.VFX(soundName);
            Destroy(effectInstanse, _explosionDestroyDelay);
        }

        private void Hit(Collider coliderHit)
        {
            if (coliderHit.gameObject.TryGetComponent(out IHp iHp))
            {
                float _random = UnityEngine.Random.Range(0.75f, 1.25f);
                iHp.Hit( _attack * _attackDamageRate * _random, _isPowerAttack, transform.rotation);
            }
        }
    }
}
