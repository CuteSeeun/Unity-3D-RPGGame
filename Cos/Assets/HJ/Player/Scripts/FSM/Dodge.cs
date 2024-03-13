using UnityEngine;
using Assets.Player.Scripts;
using CharacterController = Assets.Player.Scripts.CharacterController;

namespace Assets.Player.Scripts.FSM
{
    public class Dodge : StateBase
    {
        public override State state => State.Dodge;
        private float _force = 5.0f;

        public override void OnEnter(Animator animator)
        {
            base.OnEnter(animator);
            Rigidbody rigidbody = animator.GetComponent<Rigidbody>();
            CharacterController controller = animator.GetComponent<CharacterController>();
            rigidbody.AddForce(controller.moveDirection * _force, ForceMode.Impulse);
        }

        public override State OnUpdate(Animator animator)
        {
            return base.OnUpdate(animator);
        }
    }
}