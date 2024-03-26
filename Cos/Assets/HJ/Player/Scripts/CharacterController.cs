using System;
using UnityEngine;

namespace HJ
{
    public abstract class CharacterController : MonoBehaviour, IHp
    {
        private void Awake()
        {
            GetComponentAwake();
        }
        protected virtual void Start()
        {
            HealthStart();
            onHpMin += () => Death();

            CharacterInfoStart();
        }
        protected virtual void Update()
        {
            MoveUpdate();
        }
        protected virtual void FixedUpdate()
        {

        }

        [Header ("Get Component")] //======================================================================================================================================================
        protected Transform transform;
        protected Animator animator;
        private void GetComponentAwake()
        {
            transform = GetComponent<Transform>();
            animator = GetComponent<Animator>();
        }

        [Header ("CharacterInfo")] //======================================================================================================================================================
        [SerializeField] int _type;
        public GameObject weapon1;
        public GameObject weapon2;
        public GameObject weapon3;
        public Missile missile;
        public GameObject potion;

        public bool isPlayer;

        public float speed { get => _speed; }
        [SerializeField] float _speed = 5f;

        [SerializeField] float _armor;
        // 공격력
        // 무기공격력
        // 방어력
        // 갑옷방어력

        private void CharacterInfoStart()
        {
            animator.SetInteger("type", _type);
        }

        // [("IHp")]// ====================================================================================================================================================================
        public float hp
        {
            get
            {
                return _hp;
            }
            set
            {
                _hp = Mathf.Clamp(value, 0, _hpMax); // _hp를 value를 0~_hpMax 사잇값으로 변환해서 대입

                if (_hp == value) // 문제없이 들어가면 return
                    return;

                if (value < 1)
                {
                    onHpMin?.Invoke();
                }
                else if (value >= _hpMax)
                    onHpMax?.Invoke();
            }
        }
        private float _hp;
        public float hpMax { get => _hpMax; }
        private float _hpMax = 100;

        public event Action<float> onHpChanged;
        public event Action<float> onHpDepleted;
        public event Action<float> onHpRecovered;
        public event Action onHpMin;
        public event Action onHpMax;
        public void Hit(float damage)
        {
            DepleteHp(damage);
        }

        public virtual void Hit(float damage, bool powerAttack, Quaternion hitRotation)
        {
            if (_invincible == false)
            {
                transform.rotation = hitRotation;
                transform.Rotate(0, 180, 0);
                
                if (powerAttack ==  false)
                {
                    HitA();
                }
                else // (powerAttack ==  true)
                {
                    HitB();
                }

                DepleteHp(damage * (_hpMax / (_hpMax + _armor)));
            }
        }

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

        // [("Health")] ===================================================================================================================================================================
        private void HealthStart()
        {
            _hp = _hpMax;
        }

        // [("Defending")] ================================================================================================================================================================
        public bool defending { get => _defending; set => _defending = value; }
        private bool _defending;
        public bool defend { get => _defend; set => _defend = value; }
        private bool _defend;
        public float defendingAngle { get => _defendingAngle ; set => _defendingAngle = value; }
        private float _defendingAngle;

        // [("Attack")] ===================================================================================================================================================================
        public float attackRange { set => _attackRange = value; }
        private float _attackRange;
        public float attackAngle { set => _attackAngle = value; }
        private float _attackAngle;
        private float _attackAngleInnerProduct;
        public LayerMask attackLayerMask { set => _attackLayerMask = value; }
        private LayerMask _attackLayerMask;
        public float damageRate { set => _damageRate = value; }
        private float _damageRate;

        [SerializeField] float _attack;
        public float attackWeapon { get => _attackWeapon; set => _attackWeapon = value; }
        private float _attackWeapon;

        public bool powerAttack { get => _powerAttack; set => _powerAttack = value; }
        private bool _powerAttack;


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
                    if (hit.collider.TryGetComponent(out IHp iHp))
                    {
                        iHp.Hit(_attack * _damageRate, _powerAttack, transform.rotation);
                    }
                }
            }
        }

        public void Shoot()
        {
            Instantiate(missile, transform.position + transform.forward, transform.rotation);
        }

        // Legacy
        public Vector3 attackDirection { get => _attackDirection; set => _attackDirection = value; }
        private Vector3 _attackDirection;

        // [("Invincible")] ===============================================================================================================================================================
        public bool invincible { get => _invincible; set => _invincible = value; }
        private bool _invincible;
        public void InvincibleStart()
        {
            _invincible = true;
        }
        public void InvincibleEnd()
        {
            _invincible = false;
        }

        // [("States")] ===================================================================================================================================================================

        // states
        // 0 
        // 1 Move
        // 2 Dodge
        // 3 AttackA
        // 4 AttackB
        // 5 HitA
        // 6 HitB
        // 7 Death
        // 8 Raise?
        // 9 Interact
        // 10 UseItem
        // 11 Blocking
        // 12 필살기?

        // [("State Escape")] =============================================================================================================================================================
        public void StateCancle()
        {
            animator.SetInteger("state", 0);
        }

        public void StateReset()
        {
            animator.SetInteger("state", 1);
        }

        // [("State 1 Move")] =============================================================================================================================================================
        public virtual float horizontal { get; set; }
        public virtual float vertical { get; set; }
        public Vector3 moveDirection { get => _moveDirection; set => _moveDirection = value; }
        private Vector3 _moveDirection;
        public float moveFloat { get => _moveFloat;  }
        private float _moveFloat;

        protected void MoveUpdate()
        {
            _moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
            _moveFloat = _moveDirection.magnitude * _velocity;

            animator.SetFloat("moveFloat", _moveFloat);
        }

        // Legacy
        public float velocity { get => _velocity; }
        protected float _velocity = 1;

        // [("State 2 Dodge")] ============================================================================================================================================================
        protected void Dodge()
        {
            animator.SetInteger("state", 2);
        }

        // [("State 3 AttackA")] ==========================================================================================================================================================
        protected void AttackA()
        {
            animator.SetInteger("state", 3);
        }

        // [("State 4 AttackB")] ==========================================================================================================================================================
        protected void AttackB()
        {
            animator.SetInteger("state", 4);
        }

        protected void AttackBRelease()
        {
            animator.SetInteger("state", 1);
        }

        // [("State 5 HitA")] =============================================================================================================================================================
        public void HitA()
        {
            animator.SetInteger("state", 5);
        }

        // [("State 6 HitB")] =============================================================================================================================================================
        public float hitBTime { get => _hitBTime; }
        private float _hitBTime = 1f;
        public float hitBSpeed { get => _hitBSpeed; }
        private float _hitBSpeed = 5f;

        public void HitB()
        {
            animator.SetInteger("state", 6);
        }

        // [("State 7 Death")] ============================================================================================================================================================
        public void Death()
        {
            animator.SetInteger("state", 7);
        }

        // [("State 8")] ==================================================================================================================================================================

        // [("State 9 Interact")] =========================================================================================================================================================
        [SerializeField] LayerMask _layerMaskInteractable;

        public void Interact()
        {
            animator.SetInteger("state", 9);

            if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward, out RaycastHit hit, 2.6f, _layerMaskInteractable))
            {
                if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    interactable.Interaction(this.gameObject);
                }
            }
        }

        // [("State 10 UseItem)] ==========================================================================================================================================================
        public void UseItem()
        {
            animator.SetInteger("state", 10);
        }
    }
}
