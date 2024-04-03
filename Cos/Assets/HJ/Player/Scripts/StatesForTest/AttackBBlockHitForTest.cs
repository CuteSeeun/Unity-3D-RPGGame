using UnityEngine;
using CharacterControllerForTest = HJ.CharacterControllerForTest;

namespace HJ
{
    public class AttackBBlockingForTest : StateMachineBehaviour
    {
        Animator animator;
        Transform transform;
        CharacterControllerForTest _characterController;

        private bool _isPlayer;
        PlayerControllerForTest playerController;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _characterController = animator.GetComponent<CharacterControllerForTest>();
            animator.SetInteger("state", 4);
            _characterController.defending = true;

            _isPlayer = animator.TryGetComponent<PlayerControllerForTest>(out playerController);
            if (_isPlayer)
            {
                playerController.isSpRecover = false;
            }
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _characterController.defending = false;

            if (_isPlayer)
            {
                playerController.isSpRecover = true;
            }
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
