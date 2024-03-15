using UnityEngine;
using CharacterController = Assets.Player.Scripts.CharacterController;

namespace Assets.HJ.Player.Scripts.States
{
    public class AttackA : StateMachineBehaviour
    {
        Animator animator;
        Transform transform;
        CharacterController characterController;

        private Vector3 _AttackADirection;
        private int _attackACombo;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            transform = animator.transform;
            characterController = animator.GetComponent<CharacterController>();

            if (characterController.moveDirection.magnitude != 0)
            {
                _AttackADirection = characterController.moveDirection;
            }
            else
            {
                _AttackADirection = transform.forward;
            }

            _attackACombo++;

            if (_attackACombo > characterController.attackAComboMax)
                _attackACombo = 1;

            animator.SetInteger("attackACombo", _attackACombo);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetInteger("state", 1);
        }

        // OnStateMove : Animator.OnAnimatorMove() 바로 뒤에 호출됩니다.
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // 루트 모션을 처리하고 영향을 미치는 코드 구현
        //}

        // OnStateIK: Animator.OnAnimatorIK() 바로 뒤에 호출됩니다.
        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // 애니메이션 IK(inverse kinematics)를 설정하는 코드 구현
        }
    }
}
