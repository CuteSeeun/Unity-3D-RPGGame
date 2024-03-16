using UnityEngine;
using CharacterController = Assets.Player.Scripts.CharacterController;

namespace Assets.HJ.Player.Scripts.States
{
    public class AttackBAiming : StateMachineBehaviour
    {
        Animator animator;
        Transform transform;
        CharacterController characterController;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            transform = animator.transform;
            characterController = animator.GetComponent<CharacterController>();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (characterController.moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(characterController.moveDirection);
            }

            transform.position += characterController.moveDirection * characterController.velocity * characterController.speed * Time.fixedDeltaTime;
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
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // 애니메이션 IK(inverse kinematics)를 설정하는 코드 구현
        //}
    }
}
