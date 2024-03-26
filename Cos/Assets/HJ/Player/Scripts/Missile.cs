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

        public float missileSpeed { set => _missileSpeed = value; }
        private float _missileSpeed;
        public float missileTimer { set => _missileTimer = value;}
        [SerializeField] float _missileTimer;

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
        }

        public bool isPiercing { get => _isPiercing; set => _isPiercing = value; }
        private bool _isPiercing;
        public bool isExplosive { get => _isExplosive; set => _isExplosive = value; }
        private bool _isExplosive;

        public float attack { set => _attack = value; }
        private float _attack;
        public float attackDamageRate { set => _attackDamageRate = value; }
        private float _attackDamageRate;

        public float attackRange { set => _attackRange = value; }
        private float _attackRange;
        public float attackAngle { set => _attackAngle = value; }
        private float _attackAngle;
        private float _attackAngleInnerProduct;
        public LayerMask attackLayerMask { set => _attackLayerMask = value; }
        private LayerMask _attackLayerMask;

        public bool isPowerAttack { set => _isPowerAttack = value; }
        private bool _isPowerAttack;

        [SerializeField] LayerMask _layerMaskWall;

        private void OnTriggerEnter(Collider coliderHit)
        {
            if (coliderHit.gameObject.layer == _attackLayerMask) // 공격 대상에 박으면 작동
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
                    RaycastHit[] hits = Physics.SphereCastAll(transform.position, _attackRange, transform.up, 0, _attackLayerMask);

                    _attackAngleInnerProduct = Mathf.Cos(_attackAngle * Mathf.Deg2Rad);

                    foreach (RaycastHit hit in hits)
                    {
                        Hit(hit.collider);
                    }
                }

                if (coliderHit.gameObject.layer == _layerMaskWall) // 벽에 박으면 터짐
                {

                }
            }
        }

        private void Hit(Collider coliderHit)
        {
            if (coliderHit.gameObject.TryGetComponent(out IHp iHp))
            {
                iHp.Hit( _attack * _attackDamageRate, _isPowerAttack, transform.rotation);
            }
        }

        /*
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
        }
        */
    }
}
