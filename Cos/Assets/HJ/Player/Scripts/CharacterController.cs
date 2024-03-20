using System;
using UnityEngine;

namespace Assets.Player.Scripts
{
    public abstract class CharacterController : MonoBehaviour, IHp
    {
        protected Transform transform;
        [SerializeField] Animator animator;

        private Vector3 _motionDirection;
        [SerializeField] int _type;

        public GameObject weapon1;
        public GameObject weapon2;
        public GameObject weapon3;

        public GameObject missile;

        public bool invincible;

        private void Awake()
        {
            transform = GetComponent<Transform>();
        }

        protected virtual void Start()
        {
            _hp = _hpMax;
            onHpMin += () => Death();

            animator.SetInteger("type", _type);
        }

        protected virtual void Update()
        {
            MoveUpdate();
        }

        protected virtual void FixedUpdate()
        {
        }

        // IHp -------------------------------------------------------------
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
        public void Hit(float damage)
        {
            if (invincible == false)
            {
                HitB();

                DepleteHp(damage);
            }
        }

        /*
        public void Hit(float damage, bool powerAttack, Vector3 direction)
        {
            if (invincible == false)
            {
                if (powerAttack == false)
                {
                    HitA();
                }
                else if (powerAttack == true)
                {
                    HitB();
                    transform.rotation = Quaternion.LookRotation(direction);
                }

                DepleteHp(damage);
            }
        }
        
        public void Hit(float damage)
        {
            DepleteHp(damage);
        }
        */

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

        // Type ------------------------------------------------------------
        /*
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
        */

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

        public void AttackAComboReset()
        {
            animator.SetInteger("state", 1);
        }

        // Dodge -----------------------------------------------------------------
        public float dodgeSpeed { get => _dodgeSpeed; }
        [SerializeField] float _dodgeSpeed = 10f;
        public float dodgeTime { get => _dodgeTime; }
        [SerializeField] float _dodgeTime = 0.5f;

        public float dodgeTimeInverse { get => _dodgeTimeInverse; }
        [SerializeField] float _dodgeTimeInverse = 1f;

        public float invincibleTime { get => _invincibleTime; }
        private float _invincibleTime = 0.5f;
        
        protected void Dodge()
        {
            animator.SetInteger("state", 2);
        }

        public void InvincibleEnd()
        {
            invincible = false;
        }

        // Attack --------------------------------------------------------------
        public float attackRange { set => _attackRange = value; }
        [SerializeField] float _attackRange;
        public float attackAngle { set => _attackAngle = value; }
        [SerializeField] float _attackAngle;
        [SerializeField] float _attackAngleInnerProduct;
        public LayerMask attackLayerMask { set => _attackLayerMask = value; }
        [SerializeField] LayerMask _attackLayerMask;

        public void Attack()
        {
            // 공격 거리 내 모든 적 탐색
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, 1, 0),
                                                      _attackRange,
                                                      Vector3.up,
                                                      0,
                                                      _attackLayerMask);

            // 공격 각도에 따른 내적 계산
            _attackAngleInnerProduct = Mathf.Cos(_attackAngle * Mathf.Deg2Rad);

            // 내적으로 공격각도 구하기
            foreach (RaycastHit hit in hits)
            {
                if (Vector3.Dot((hit.transform.position - transform.position).normalized, transform.forward) > _attackAngleInnerProduct)
                {
                    // 데미지 주고, 데미지, 공격 방향, 파워어택 여부 전달
                    Debug.Log(hit);
                }
            }
        }

        public void Shoot()
        {
            Instantiate(missile, transform.position + transform.forward, transform.rotation);
        }

        // AttackA -----------------------------------------------------------------

        protected void AttackA()
        {
            animator.SetInteger("state", 3);
            Attack();
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

        // HitA-----------------------------------------------------------
        public void HitA()
        {
            animator.SetInteger("state", 5);
        }

        // HitB-----------------------------------------------------------
        public float hitBTime { get => _hitBTime; }
        private float _hitBTime = 1f;

        public float hitBSpeed { get => _hitBSpeed; }

        private float _hitBSpeed = 5f;

        public void HitB()
        {
            animator.SetInteger("state", 6);
        }

        // Death
        public void Death()
        {
            animator.SetInteger("state", 7);
        }
    }
}
