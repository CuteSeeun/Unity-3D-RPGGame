using UnityEngine;
using Assets.Player.Scripts.FSM;
using CharacterController = Assets.Player.Scripts.CharacterController;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Interactions;

namespace Assets.Player.Scripts
{
    internal class PlayerController : CharacterController
    {
        protected override void Start()
        {
            base.Start();
        }
        protected override void Update()
        {
            base.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
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
            Dodge();
        }

        public void OnAttack1(InputAction.CallbackContext context)
        {
            Attack1();
        }

        public void OnAttack2(InputAction.CallbackContext context)
        {
            Attack2();
            if (context.interaction is HoldInteraction)
            {
            }
            else if (context.interaction is PressInteraction)
            {
                Attack2End();
            }
        }
        #endregion ========================================================
    }
}
