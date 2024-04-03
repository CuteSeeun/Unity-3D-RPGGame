using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

using CharacterController = HJ.CharacterController;
 
namespace HJ
{
    public class PlayerControllerForTest : CharacterControllerForTest
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
        public float spMax { get => ( _spMax + spMaxItem ) * ( 1 + spMaxSkill + spMaxFood ); }
        private float _spMax = 100;
        public float spMaxItem;
        public float spMaxSkill;
        public float spMaxFood;
        public float spRecovery { get => (_spRecovery + spRecoveryItem) * (1 + spRecoverySkill + spRecoveryFood); }
        private float _spRecovery = 35;
        public float spRecoveryItem;
        public float spRecoverySkill;
        public float spRecoveryFood;
        public bool isSpRecover { get => _isSpRecover; set => _isSpRecover = value; }
        private bool _isSpRecover;
        public float staminaRequired { set => _staminaRequired = value; }
        private float _staminaRequired;

        private void StaminaStart()
        {
            stamina = spMax;
        }
        private void StaminaUpdate()
        {
            if (_isSpRecover)
            {
                if (stamina < spMax)
                {
                    stamina += spRecovery * Time.deltaTime;
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
        [SerializeField] int maxPotion { get => _maxPotion + maxPotionItem;  }
        private int _maxPotion = 5;
        public int maxPotionItem;
        public int potionNumber { get => _potionNumber; set => _potionNumber = Mathf.Clamp(value, 0, maxPotion); }
        [SerializeField] int _potionNumber;
        public float potionHp { get => _potionHp + potionHpItem; }
        [SerializeField] float _potionHp = 35;
        public float potionHpItem;

        private void PotionStart()
        {
            _potionNumber = maxPotion;
        }
        protected void Potion()
        {
            potionNumber--;
            RecoverHp(potionHp);
            Debug.Log(potionHp);
        }
        public void PotionFull()
        {
            potionNumber = _maxPotion;
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

                if (stamina > damage * (10 / armor))
                {
                    stamina -= damage * (10 / armor);
                    animator.SetInteger("state", 11);
                }
                else
                {
                    hp -= ((damage * (10 / armor)) - stamina);
                    stamina = 0;
                    animator.SetInteger("state", 3);
                }

                return;
            }

            base.Hit(damage, powerAttack, hitRotation);
        }

    }
}
