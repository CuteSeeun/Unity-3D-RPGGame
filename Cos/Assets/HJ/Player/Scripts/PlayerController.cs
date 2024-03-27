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
            StaminaStart();
            PotionStart();
        }
        protected override void Update()
        {
            base.Update();
            StaminaUpdate();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        // SP =============================================================================================================================================================================

        public float stamina
        {
            get => _stamina;
            set { _stamina = Mathf.Clamp(value, 0, _spMax); }
        }
        [SerializeField] float _stamina;
        private float _spMax = 100;
        private float _spRecovery = 35;
        public bool isSpRecover { get => _isSpRecover; set => _isSpRecover = value; }
        private bool _isSpRecover;
        public float staminaRequired { set => _staminaRequired = value; }
        private float _staminaRequired;

        private void StaminaStart()
        {
            stamina = _spMax;
        }
        private void StaminaUpdate()
        {
            if (_isSpRecover)
            {
                if (stamina < _spMax)
                {
                    stamina += _spRecovery * Time.deltaTime;
                }
            }
        }
        public void StaminaRecoverStart()
        {
            _isSpRecover = true;
        }
        public void StaminaRecoverStop()
        {
            _isSpRecover = false;
        }

        public bool StaminaUse()
        {
            if (stamina > _staminaRequired)
            {
                stamina -= _staminaRequired;
                StaminaRecoverStop();
                return true;
            }
            else
            {
                StateCancle();
                return false;
            }
        }

        // [Potion] ====================================================================================================================================================================
        [SerializeField] int _maxPotion;
        public int potionNumber { get => _potionNumber; set => _potionNumber = value; }
        [SerializeField] int _potionNumber;
        [SerializeField] float _potionHp;

        private void PotionStart()
        {
            _potionNumber = _maxPotion;
        }
        protected void UsePotion()
        {
            potionNumber--;
            RecoverHp(_potionHp);
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

        public void OnDodge(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Dodge();
            }
        }

        public void OnAttackA(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                AttackA();
            }
        }

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
                if (_potionNumber > 0 && hp <= hpMax -1)
                {
                    UseItem();
                }
            }
        }
        #endregion ========================================================

        // 개편 필요
        public override void Hit(float damage, bool powerAttack, Quaternion hitRotation)
        {
            if (defending == true && 180 - Quaternion.Angle(transform.rotation, hitRotation) < defendingAngle) // 방어중 && 방어 각도 성공
            {
                transform.rotation = hitRotation;
                transform.Rotate(0, 180, 0);

                if (stamina > damage * (10 / _armor))
                {
                    stamina -= damage * (10 / _armor);
                    animator.SetInteger("state", 11);
                }
                else
                {
                    hp -= ((damage * (10 / _armor)) - stamina);
                    stamina = 0;
                    animator.SetInteger("state", 3);
                }

                return;
            }

            base.Hit(damage, powerAttack, hitRotation);
        }
    }
}
