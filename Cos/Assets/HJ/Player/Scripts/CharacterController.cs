using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace Assets.Player.Scripts
{
    public abstract class CharacterController : MonoBehaviour
    {
        protected Transform transform;
        [SerializeField] Animator animator;

        private Vector3 _motionDirection;
        [SerializeField] int _type;

        private void Awake()
        {
            transform = GetComponent<Transform>();
        }

        protected virtual void Start()
        {
            animator.SetInteger("type", _type);
            animator.SetInteger("attackACombo", 1);
            SetType();
        }

        protected virtual void Update()
        {
            MoveUpdate();
        }

        protected virtual void FixedUpdate()
        {
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
        public int attackACombo { get => _attackACombo; set => _attackACombo = value; }
        private int _attackACombo = 1;

        protected void AttackA()
        {
            animator.SetInteger("state", 3);
        }

        // AttackB-----------------------------------------------------------
        protected void AttackB()
        {
            animator.SetInteger("state", 4);
        }

        protected void AttackBRelease()
        {
            animator.SetInteger("state", 1);
        }

        protected void AttackBEnd()
        {
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
            animator.SetInteger("state", 5);
            animator.SetTrigger("hitA");
            Debug.Log("HitA");
        }

        protected void HitAUpdate()
        {

        }

        protected void HitAFixedUpdate()
        {
            _hitATimeLeft -= Time.fixedDeltaTime;
        }

        // HitB-----------------------------------------------------------
        [SerializeField] float _hitBTime = 1f;
        private float _hitBTimeLeft;
        [SerializeField] float _hitBSpeed = 5f;
        private float _hitBSpeedLeft;

        public void HitB()
        {
            _hitBTimeLeft = _hitBTime;
            _hitBSpeedLeft = _hitBSpeed;
            animator.SetInteger("state", 6);
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
        }
    }
}
