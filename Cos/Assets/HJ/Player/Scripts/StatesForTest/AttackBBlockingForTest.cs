using UnityEngine;
using CharacterControllerForTest = HJ.CharacterControllerForTest;

namespace HJ
{
    public class AttackBBlockHitForTest : StateMachineBehaviour
    {
        Animator animator;
        Transform transform;
        CharacterControllerForTest _characterController;

        [SerializeField] float _defendingAngle;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            transform = animator.transform;
            _characterController = animator.GetComponent<CharacterControllerForTest>();
            _characterController.defendingAngle = _defendingAngle;
            _characterController.defending = true;
            animator.SetInteger("state", 4);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_characterController.moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(_characterController.moveDirection);
            }

            transform.position += _characterController.moveDirection * 0.5f * _characterController.speed * Time.fixedDeltaTime;
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _characterController.defending = false;
            //animator.SetInteger("state", 1);
        }
    }
}
