using UnityEngine;
using CharacterController = HJ.CharacterController;

namespace HJ
{
    public class VoidBehaviourScript : StateMachineBehaviour
    {
        Animator _animator;
        CharacterController _characterController;
        Transform _transform;

        [SerializeField] bool _resetStart;
        [SerializeField] bool _resetEnd;
        [SerializeField] bool _resetDelayed;
        [SerializeField] float _stateResetTime;

        [SerializeField] bool _canMove;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _characterController = animator.GetComponent<CharacterController>();
            _transform = _characterController.transform;

            if(_resetStart)
            _characterController.StateReset();

            if (_resetDelayed)
                _characterController.Invoke("StateReset", _stateResetTime);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_canMove)
            {
                if (_characterController.moveDirection != Vector3.zero)
                {
                    _transform.rotation = Quaternion.LookRotation(_characterController.moveDirection);
                }

                _transform.position += _characterController.moveDirection * 0.5f * _characterController.speed * Time.fixedDeltaTime;
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if( _resetEnd)
            _characterController.StateReset();
        }
    }
}