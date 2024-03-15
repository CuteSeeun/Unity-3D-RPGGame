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
            SetType();
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
                        break;
                    case 3:
                        break;
                    case 4:
                        AttackBUpdate();
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
            {
            }
            else
            {
                switch (_state)
                {
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        AttackBFixedUpdate();
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

        // Type ------------------------------------------------------------
        private int _attackAComboMax;
        private void SetType()
        {
            switch (_type)
            {
                case 0:
                    _attackAComboMax = 2;
                    break;
                case 1:
                    _attackAComboMax = 4;
                    break;
                case 2:
                    _attackAComboMax = 2;
                    break;
                case 3:
                    _attackAComboMax = 2;
                    break;
                case 4:
                    _attackAComboMax = 1;
                    break;
            }
        }

        // Move --------------------------------------------------------------
        public float speed { get => _speed; }
        [SerializeField] float _speed = 5f;
        public virtual float horizontal { get; set; }
        public virtual float vertical { get; set; }
        public Vector3 moveDirection { get => _moveDirection; set => _moveDirection = value; }
        protected Vector3 _moveDirection;
        public float moveFloat { get => _moveFloat;  }
        private float _moveFloat;
        public float velocity { get => _velocity; }
        protected float _velocity = 1;

        protected void MoveUpdate()
        {
            _moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
            _moveFloat = _moveDirection.magnitude * _velocity;

            animator.SetFloat("moveFloat", _moveFloat);
        }

        // Dodge -----------------------------------------------------------------
        public float dodgeSpeed { get => _dodgeSpeed; }
        [SerializeField] float _dodgeSpeed = 10f;
        public float dodgeTime { get => _dodgeTime; }
        [SerializeField] float _dodgeTime = 0.5f;

        public float dodgeTimeInverse { get => _dodgeTimeInverse; }
        [SerializeField] float _dodgeTimeInverse = 1f;
        
        protected void Dodge()
        {
            animator.SetInteger("state", 2);
        }

        // AttackA -----------------------------------------------------------------
        public int attackAComboMax { get => _attackAComboMax; }

        protected void AttackA()
        {
            animator.SetInteger("state", 3);
        }

        // Attack2-----------------------------------------------------------
        protected void AttackB()
        {
            animator.SetTrigger("attack2");

            _isDirty = true;
            _state = 4;
            animator.SetInteger("state", _state);
        }

        protected void AttackBRelease()
        {
            _isDirty = false;
            _state = 1;
            animator.SetInteger("state", _state);
        }

        protected void AttackBEnd()
        {
            _isDirty = false;
        }

        protected void AttackBUpdate()
        {

        }
        protected void AttackBFixedUpdate()
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
