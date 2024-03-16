using UnityEngine;
using UnityEngine.EventSystems;
using CharacterController = Assets.Player.Scripts.CharacterController;

namespace Assets.HJ.Player.Scripts.States
{
    public class Move : StateMachineBehaviour
    {
        Animator animator;
        Transform transform;
        CharacterController characterController;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetInteger("state", 1);
            transform = animator.transform;
            characterController = animator.GetComponent<CharacterController>();
            characterController.moveDirection = Vector3.zero;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log(1);
            if (animator.GetInteger("state") == 1)
            {
                Debug.Log(2);
                if (characterController.moveDirection != Vector3.zero)
                {
                    Debug.Log(3);
                    transform.rotation = Quaternion.LookRotation(characterController.moveDirection);
                }

                transform.position += characterController.moveDirection * characterController.velocity * characterController.speed * Time.fixedDeltaTime;
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

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
}
