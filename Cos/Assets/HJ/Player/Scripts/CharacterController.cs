using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Player.Scripts.FSM;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace Assets.Player.Scripts
{
    public abstract class CharacterController : MonoBehaviour
    {
        protected Transform transform;
        [SerializeField] Animator animator;

        private bool _isDirty = false;
        private int _state;
        private Vector3 _motionDirection;
        [SerializeField] int _type;

        private void Awake()
        {
            transform = GetComponent<Transform>();
        }

        protected virtual void Start()
        {
            animator.SetInteger("type", _type);
        }

        protected virtual void Update()
        {
            if (!_isDirty)
                MoveUpdate();
            else
            {
                switch (_state)
                {
                    case 2:
                        DodgeUpdate();
                        break;
                    case 3:
                        Attack1Update();
                        break;
                    case 4:
                        Attack2Update();
                        break;
                    case 5:
                        HitAUpdate();
                        break;
                    case 6:
                        HitBUpdate();
                        break;

                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if (!_isDirty)
                MoveFixedUpdate();
            else
            {
                switch (_state)
                {
                    case 2:
                        DodgeFixedUpdate();
                        break;
                    case 3:
                        Attack1FixedUpdate();
                        break;
                    case 4:
                        Attack2FixedUpdate();
                        break;
                    case 5:
                        HitAFixedUpdate();
                        break;
                    case 6:
                        HitBFixedUpdate();
                        break;
                }
            }
        }

        // Move--------------------------------------------------------------
        [SerializeField] float _speed = 5f;
        public virtual float horizontal { get; set; }
        public virtual float vertical { get; set; }
        public Vector3 moveDirection { get => _moveDirection; }
        protected Vector3 _moveDirection;
        private float _moveFloat;
        protected float _velocity = 1;
        protected void MoveUpdate()
        {
            _moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
            _moveFloat = _moveDirection.magnitude * _velocity;

            _state = 1;
            animator.SetInteger("state", _state);
            animator.SetFloat("moveFloat", _moveFloat);
        }

        protected void MoveFixedUpdate()
        {
            if (_moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(_moveDirection);
                transform.position += _moveDirection * _velocity * _speed * Time.fixedDeltaTime;
            }
        }

        // Dodge-----------------------------------------------------------------
        [SerializeField] float _dodgeSpeed = 10f;
        private float _dodgeSpeedLeft;
        [SerializeField] float _dodgeTime = 0.5f;
        [SerializeField] float _dodgeTimeInverse = 1f;
        private float _dodgeTimeLeft;

        protected void Dodge()
        {
            if (!_isDirty && _moveDirection.magnitude != 0)
            {
                _motionDirection = _moveDirection;
                _dodgeSpeedLeft = _dodgeSpeed;
                _dodgeTimeLeft = _dodgeTime;

                _isDirty = true;
                _state = 2;
                animator.SetInteger("state", _state);
            }
        }

        protected void DodgeUpdate()
        { }

        protected void DodgeFixedUpdate()
        {
            _dodgeSpeedLeft -= _dodgeSpeed * _dodgeTimeInverse * Time.fixedDeltaTime;
            _dodgeTimeLeft -= Time.fixedDeltaTime;
            transform.position += _motionDirection * _dodgeSpeedLeft * Time.fixedDeltaTime;

            if (_dodgeTimeLeft < 0)
            {
                _isDirty = false;
            }
        }

        // Attack1-----------------------------------------------------------------
        [SerializeField] float _attackTime;
        private float _attackTimeLeft;
        [SerializeField] int _attack1ComboMax;
        private int _attack1Combo;

        protected void Attack1()
        {
            if (!_isDirty)
            {
                _motionDirection = _moveDirection;
                _attackTimeLeft = _attackTime;

                _isDirty = true;
                _state = 3;
                animator.SetInteger("state", _state);
                animator.SetTrigger("attack1");

                animator.SetInteger("attack1Combo", _attack1Combo++);

                if (_attack1Combo > _attack1ComboMax - 1)
                    _attack1Combo = 0;
            }
        }

        protected void Attack1Update()
        { }

        protected void Attack1FixedUpdate()
        {
            _attackTimeLeft -= Time.fixedDeltaTime;

            if (_attackTimeLeft < 0)
            {
                _isDirty = false;
            }
        }

        // Attack2-----------------------------------------------------------
        protected void Attack2()
        {
            animator.SetTrigger("attack2");

            _isDirty = true;
            _state = 4;
            animator.SetInteger("state", _state);
        }

        protected void Attack2End()
        {
            _isDirty = false;
        }

        protected void Attack2Update()
        {

        }
        protected void Attack2FixedUpdate()
        {

        }

        // HitA-----------------------------------------------------------
        [SerializeField] float _hitATime = 0.5f;
        private float _hitATimeLeft;

        public void HitA()
        {
            _hitATimeLeft = _hitATime;
            _isDirty = true;
            _state = 5;
            animator.SetInteger("state", _state);
            animator.SetTrigger("hitA");
            Debug.Log("HitA");
        }

        protected void HitAUpdate()
        {

        }

        protected void HitAFixedUpdate()
        {
            _hitATimeLeft -= Time.fixedDeltaTime;

            if (_hitATimeLeft < 0)
            {
                _isDirty = false;
            }
        }

        // HitB-----------------------------------------------------------
        [SerializeField] float _hitBTime = 1f;
        private float _hitBTimeLeft;
        [SerializeField] float _hitBSpeed = 10f;
        private float _hitBSpeedLeft;

        public void HitB()
        {
            _hitBTimeLeft = _hitBTime;
            _hitBSpeedLeft = _hitBSpeed;
            _isDirty = true;
            _state = 6;
            _state = 6;
            animator.SetInteger("state", _state);
            animator.SetTrigger("hitB");
            Debug.Log("HitB");
        }

        protected void HitBUpdate()
        {

        }

        protected void HitBFixedUpdate()
        {
            _hitBTimeLeft -= Time.fixedDeltaTime;
            _hitBSpeedLeft -= _hitBSpeed * Time.fixedDeltaTime;
            transform.position += new Vector3(0,0,1) * _hitBSpeedLeft * Time.fixedDeltaTime;


            if (_hitBTimeLeft < 0)
            {
                _isDirty = false;
            }
        }
    }
}
