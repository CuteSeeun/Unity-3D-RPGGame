using UnityEngine;
using CharacterController = HJ.CharacterController;

namespace HJ
{
    public class AttackA : StateMachineBehaviour
    {
        Animator animator;
        Transform transform;
        CharacterController characterController;

        private Quaternion _attackDirection;

        [SerializeField] float _damageRate;

        [SerializeField] float _attackRange;
        [SerializeField] float _attackAngle;
        [SerializeField] LayerMask _attackLayerMask;

        [SerializeField] float _attackDelayTime;

        [SerializeField] bool _isCombo;
        [SerializeField] float _comboResetTime;

        [SerializeField] bool _isDoubleAttack;
        [SerializeField] float _DoubleAttackDelayTime;

        [SerializeField] bool _isPowerAttack;

        [SerializeField] bool _isInvincible;
        [SerializeField] float _invincibleTime;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            transform = animator.transform;
            characterController = animator.GetComponent<CharacterController>();

            characterController.attackRange = _attackRange;
            characterController.attackAngle = _attackAngle;
            characterController.attackLayerMask = _attackLayerMask;
            characterController.powerAttack = _isPowerAttack;

            characterController.Invoke("Attack", _attackDelayTime);

            if (_isCombo)
            {
                characterController.Invoke("StateReset", _comboResetTime);
            }
            
            if (_isDoubleAttack)
                characterController.Invoke("Attack", _DoubleAttackDelayTime);

            if (_isInvincible)
            {
                characterController.invincible = true;
                characterController.Invoke("InvincibleEnd", _invincibleTime);
            }
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

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
