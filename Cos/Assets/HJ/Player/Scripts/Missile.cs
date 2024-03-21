using System;
using UnityEngine;

namespace HJ
{
    public class Missile : MonoBehaviour
    {
        [SerializeField] float _speed;
        [SerializeField] float _timer;

        [SerializeField] LayerMask _layerMask;
        [SerializeField] LayerMask _layerMaskWall;
        [SerializeField] bool _isPiercing;

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

        private void OnCollisionEnter(Collision collision)
        {
            if (_isPiercing == false)
            {
                Destroy(this);
            }
            else
            {
                if (collision.gameObject.TryGetComponent(out IHp iHp))
                {
                    iHp.Hit(1);
                    Destroy(this);
                }

                if (collision.gameObject.layer == _layerMaskWall)
                {
                    Destroy(this);
                }
            }
        }
    }
}
