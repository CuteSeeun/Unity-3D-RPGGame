using UnityEngine;
using CharacterController = Assets.Player.Scripts.CharacterController;

public class Dodge : StateMachineBehaviour
{
    Animator animator;
    Transform transform;
    CharacterController characterController;

    private Vector3 _dodgeDirection;
    private float _dodgeSpeedLeft;
    private float _dodgeTimeLeft;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterController = animator.GetComponent<CharacterController>();
        transform = animator.transform;

        if (characterController.moveDirection.magnitude != 0)
        {
            _dodgeDirection = characterController.moveDirection;
        }
        else
        {
            _dodgeDirection = transform.forward;
        }

        transform.rotation = Quaternion.LookRotation(_dodgeDirection);

        _dodgeSpeedLeft = characterController.dodgeSpeed;
        _dodgeTimeLeft = characterController.dodgeTime;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("state", 2);
        _dodgeSpeedLeft -= characterController.dodgeSpeed * characterController.dodgeTimeInverse * Time.fixedDeltaTime;
        _dodgeTimeLeft -= Time.fixedDeltaTime;
        transform.position += _dodgeDirection * _dodgeSpeedLeft * Time.fixedDeltaTime;
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
