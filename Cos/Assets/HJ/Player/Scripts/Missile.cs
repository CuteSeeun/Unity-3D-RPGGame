using System;
using UnityEngine;

namespace Assets.HJ.Player.Scripts
{
    public class Missile : MonoBehaviour
    {
        [SerializeField] float _speed;
        [SerializeField] float _timer;

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
    }
}
