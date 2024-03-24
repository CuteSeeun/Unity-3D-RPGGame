using System;
using UnityEngine;

namespace HJ
{
    public class Missile : MonoBehaviour
    {
        [SerializeField] float _speed;
        [SerializeField] float _timer;

        [SerializeField] LayerMask _attackLayerMask;
        [SerializeField] LayerMask _layerMaskWall;
        [SerializeField] bool _isPiercing;
        [SerializeField] bool _isPowerAttack;
        [SerializeField] bool _isExplosive;
        [SerializeField] float _explosionRadius;

        public void Start()
        {
            Invoke("TimeOut", _timer);
        }

        public void FixedUpdate()
        {
            transform.position += _speed * transform.forward * Time.fixedDeltaTime;
        }

        private void TimeOut()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);
            if (_isPiercing == false)
            {
                Destroy(gameObject);

                if(_isExplosive == false)
                {

                }
                else // (_isExplosive == true)
                {
                    RaycastHit[] hits = Physics.SphereCastAll(transform.position, _explosionRadius, transform.up, 0, _attackLayerMask);

                    foreach (RaycastHit hit in hits)
                    {
                        // 데미지 주고, 데미지, 공격 방향, 파워어택 여부 전달
                        if (hit.collider.TryGetComponent(out IHp iHp))
                        {
                            iHp.Hit(1, _isPowerAttack, transform.rotation);
                        }
                    }
                }
            }
            else
            {
                if (other.gameObject.TryGetComponent(out IHp iHp))
                {
                    iHp.Hit(1, _isPowerAttack, transform.rotation);
                }

                if (other.gameObject.layer == _layerMaskWall)
                {
                    Destroy(gameObject);
                }
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
