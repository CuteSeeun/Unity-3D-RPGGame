using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

using CharacterController = HJ.CharacterController;
 
namespace HJ
{
    public class PlayerController : CharacterController
    {
        protected override void Start()
        {
            base.Start();
            sp = _spMax;
        }
        protected override void Update()
        {
            base.Update();

            if (_isSpRecover)
            {
                if (sp < _spMax)
                {
                    sp += _spRecovery * Time.deltaTime;
                }
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        // SP -------------------------------------------------------------
        private float _spMax = 100;

        /*
        public float sp 
        { 
            get => _sp;
            set
            {
                _sp = Mathf.Clamp(value, 0, _spMax);

                if (_sp == value)
                    return;
            }
        }
        private float _sp;
        */

        public float sp;

        public bool isSpRecover { get => _isSpRecover; set => _isSpRecover = value; }
        private bool _isSpRecover;
        private float _spRecovery = 35;
        public void StaminaRecoverStart()
        {
            _isSpRecover = true;
        }
        public void StaminaRecoverStop()
        {
            _isSpRecover = false;
        }

        #region InputSystem ===============================================
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();
            horizontal = inputVector.x * 0.707f + inputVector.y * 0.707f;
            vertical = inputVector.x * -0.707f + inputVector.y * 0.707f;
        }

        public void OnWalk(InputAction.CallbackContext context)
        {
            if (context.performed)
                _velocity = 0.5f;
            else
                _velocity = 1.0f;
        }

        public float dodgeSp { get => _dodgeSp; set => _dodgeSp = value; }
        private float _dodgeSp = 30;
        public void OnDodge(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (sp < _dodgeSp)
                {
                    return;
                }
                else
                {
                    Dodge();
                }
            }
        }

        public float attackASp { get => _attackASp; set => _attackASp = value;} 
        [SerializeField] float _attackASp;
        public void OnAttackA(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (sp < _attackASp)
                {
                    return;
                }
                else
                {
                    AttackA();
                }
            }
        }

        [SerializeField] float _attackBSp;
        public void OnAttackB(InputAction.CallbackContext context)
        {
            if (context.interaction is HoldInteraction)
            {
                AttackB();
            }
            else if (context.interaction is PressInteraction)
            {
                AttackBRelease();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Interact();
            }
        }

        public void OnUseItem(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                UseItem();
            }
        }
        #endregion ========================================================

        public override void Hit(float damage, bool powerAttack, Quaternion hitRotation)
        {
            if (defending == true && 180 - Quaternion.Angle(transform.rotation, hitRotation) < defendingAngle) // 방어중 && 방어 각도 성공
            {
                transform.rotation = hitRotation;
                transform.Rotate(0, 180, 0);
                
                // 방어력 대폭 상승
                //DepleteHp(damage);

                if (sp > damage)
                {
                    sp -= damage;
                    animator.SetInteger("state", 11);
                }
                else
                {
                    hp -= (damage-sp);
                    sp = 0;
                    animator.SetInteger("state", 3);
                }

                return;
            }

            base.Hit(damage, powerAttack, hitRotation);
        }
    }
}
