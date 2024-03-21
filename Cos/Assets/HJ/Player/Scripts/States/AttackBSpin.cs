using UnityEngine;
using CharacterController = HJ.CharacterController;

namespace HJ
{
    public class AttackBSpin : StateMachineBehaviour
    {
        Animator animator;
        Transform transform;
        CharacterController characterController;

        [SerializeField] float _damageRate;

        [SerializeField] float _attackRange;
        [SerializeField] float _attackAngle;
        [SerializeField] LayerMask _attackLayerMask;

        [SerializeField] float _attackDelayTime;
        [SerializeField] float _attackRepeatTime;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            transform = animator.transform;
            characterController = animator.GetComponent<CharacterController>();

            characterController.attackRange = _attackRange;
            characterController.attackAngle = _attackAngle;
            characterController.attackLayerMask = _attackLayerMask;

            characterController.InvokeRepeating("Attack", _attackDelayTime, _attackRepeatTime);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            transform.position += characterController.moveDirection * 0.5f * characterController.speed * Time.fixedDeltaTime;
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            characterController.CancelInvoke("Attack");
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
