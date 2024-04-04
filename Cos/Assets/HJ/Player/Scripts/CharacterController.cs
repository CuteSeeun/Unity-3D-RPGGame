using KJ;
using Scene_Teleportation_Kit.Scripts.player;
using System;
using UnityEngine;
using Attribute = KJ.Attribute;

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
            
            ItemDBManager itemDBManager = ItemDBManager.Instance;
            GameData gameData = NetData.Instance.gameData;
            Class classKnight = GetClass(ClassType.knight);
            Class classbabarian = GetClass(ClassType.barbarian);
            Class GetClass(ClassType classType)
            {
                return gameData.classes[classType];
            }
            _hpMax = classKnight.baseHp;
            for (int i = 0; i < itemDBManager._itemData.items.Count; i++)
            {
                if (itemDBManager._itemData.items[i].type == "weapon")
                {
                    attackItem = 10;
                }
                if (itemDBManager._itemData.items[i].type == "armor")
                {
                    armorItem = 10;
                }
            }
            for (int j = 0; j < itemDBManager._itemData.items.Count; j++)
            {
                if (itemDBManager._itemData.items[j].name == "귀걸이")
                {
                    //attackSkill = 0.3f;
                }
                else if (itemDBManager._itemData.items[j].name == "목걸이")
                {
                    //attackSkill = 0.2f;
                }
                else if (itemDBManager._itemData.items[j].name == "반지")
                {
                    //attackSkill = 0.1f;
                }
            }

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

        public float attack { get => (attackWeapon + attackItem) * ( 1 + attackSkill + attackFood); }
        public float attackWeapon;
        public float attackItem;
        public float attackSkill;
        public float attackFood;
        public float armor { get => (armorArmor +armorItem) * ( 1 + armorSkill + armorFood); }
        public float armorArmor;
        public float armorItem;
        public float armorSkill;
        public float armorFood;

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
                _hp = Mathf.Clamp(value, 0, hpMax); // _hp를 value를 0~_hpMax 사잇값으로 변환해서 대입

                if (_hp == value) // 문제없이 들어가면 return
                {
                    onHpChanged?.Invoke(_hp);
                    return;
                }

                if (value < 1)
                {
                    onHpMin?.Invoke();
                }
                else if (value >= _hpMax)
                    onHpMax?.Invoke();
                onHpChanged?.Invoke(_hp);
            }
        }
        
        [SerializeField] private float _hp;
        public float hpMax { get => (_hpMax + hpMaxItem) * (1 + hpSkill + hpFood); }
        private float _hpMax;
        public float hpMaxItem;
        public float hpSkill;
        public float hpFood;
        

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

                DepleteHp(damage * (10 / armor));
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
            _hp = hpMax;
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
        public float attackDamageRate { set => _attackDamageRate = value; }
        private float _attackDamageRate;

        public bool isPowerAttack { get => _isPowerAttack; set => _isPowerAttack = value; }
        private bool _isPowerAttack;

        public bool isPiercing { get => _isPiercing; set => _isPiercing = value; } 
        private bool _isPiercing;
        public bool isExplosive { get => _isExplosive; set => _isExplosive = value; }
        private bool _isExplosive;
        public float missileSpeed { get => _missileSpeed; set => _missileSpeed = value; }
        private float _missileSpeed;
        public float missileTimer { get => _missileTimer; set => _missileTimer = value; }
        private float _missileTimer;

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
                float dis = Vector3.Distance(this.transform.position, hit.transform.position);

                //if (dis < 3) { Debug.Log();  }
            

                if (Vector3.Dot((hit.transform.position - transform.position).normalized, transform.forward) > _attackAngleInnerProduct)
                {
                    // 데미지 주고, 데미지, 공격 방향, 파워어택 여부 전달
                    if (hit.collider.TryGetComponent(out IHp iHp))
                    {
                        float _random = UnityEngine.Random.Range(0.85f, 1.15f);
                        iHp.Hit(attack * _attackDamageRate * _random, _isPowerAttack, transform.rotation);
                    }
                }
            }
        }

        public void Shoot()
        {
            Missile _missile = Instantiate(missile, transform.position + transform.forward, transform.rotation);
            //_missile.missileSpeed = _missileSpeed;
            //_missile.missileTimer = missileTimer;
            //_missile.isPiercing = _isPiercing;
            //_missile.isExplosive = _isExplosive;
            _missile.attack = attack;
            //_missile.attackDamageRate = _attackDamageRate;
            //_missile.attackRange = _attackRange;
            //_missile.attackAngle = _attackAngle;
            //_missile.attackLayerMask = _attackLayerMask;
            //_missile.isPowerAttack = _isPowerAttack;
        }

        // Legacy
        public Vector3 attackDirection { get => _attackDirection; set => _attackDirection = value; }
        private Vector3 _attackDirection;

        // [("Invincible")] ===============================================================================================================================================================
        public bool invincible { get => _invincible; set => _invincible = value; }
        [SerializeField] private bool _invincible;
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
        public void HitB()
        {
            animator.SetInteger("state", 6);
        }

        // [("State 7 Death")] ============================================================================================================================================================
        public void Death()
        {
            animator.SetInteger("state", 7);
            Destroy(this);
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<CapsuleCollider>());
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
