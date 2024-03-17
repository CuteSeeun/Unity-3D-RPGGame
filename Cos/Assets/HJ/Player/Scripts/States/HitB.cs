using UnityEngine;
using CharacterController = Assets.Player.Scripts.CharacterController;

public class HitB : StateMachineBehaviour
{
    CharacterController characterController;
    Transform transform;
    private float _hitBTimeLeft;
    private float _hitBSpeedLeft;
    // private Vector3 적 방향

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterController = animator.GetComponent<CharacterController>();
        transform = animator.transform;

        _hitBTimeLeft = characterController.hitBTime;
        _hitBSpeedLeft = characterController.hitBSpeed;

        // 적 방향 계산
        // transform.rotation = Quaternion.LookRotation( 적 방향으로 회전 );
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _hitBTimeLeft -= Time.fixedDeltaTime;
        _hitBSpeedLeft -= characterController.hitBSpeed * Time.fixedDeltaTime;
        transform.position += -transform.forward * _hitBSpeedLeft * Time.fixedDeltaTime;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("state", 1);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
