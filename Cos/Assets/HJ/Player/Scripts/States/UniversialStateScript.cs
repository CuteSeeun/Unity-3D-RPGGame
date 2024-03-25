using UnityEngine;
using CharacterController = HJ.CharacterController;

namespace HJ
{
    public class UniversialStateScript : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GetComponents(animator, stateInfo);
            Stamina();

            if (_staminaEnough)
            {
                ResetEnter();
                AttackEnter();
                Invincible();
            } 
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_staminaEnough)
            {
                MoveUpdate();
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_staminaEnough)
            {
                ResetExit();
            }
        }

        [Header("Get Components")] //======================================================================================================================================================
        CharacterController _characterController;
        PlayerController _playerController;
        Transform _transform;
        float _stateLength;
        private void GetComponents(Animator animator, AnimatorStateInfo stateInfo)
        {
            _characterController = animator.GetComponent<CharacterController>();
            _transform = _characterController.transform;
            if (_characterController.isPlayer)
                _playerController = _characterController.GetComponent<PlayerController>();
            _stateLength = stateInfo.length;
        }

        [Header("Stamina")]
        [SerializeField] bool _useStamina;
        private bool _staminaEnough;
        [SerializeField] float _staminaRequired;
        [Range(0, 1f)]
        [SerializeField] float _StaminaDelayTime;
        private void Stamina()
        {
            if (_useStamina && _playerController.sp > _staminaRequired)
            {
                _staminaEnough = true;
                _playerController.sp -= _staminaRequired;
                _playerController.StaminaRecoverStop();
                _playerController.Invoke("StaminaRecoverStart", _StaminaDelayTime * _stateLength);
            }
            else
            {
                _staminaEnough = false;
                _characterController.StateCancle();
            }
        }

        [Header("Reset Timing")] //========================================================================================================================================================
        [SerializeField] bool _resetStart;
        [SerializeField] bool _resetEnd;
        [SerializeField] bool _resetDelayed;
        [SerializeField] float _stateResetTime;
        private void ResetEnter()
        {
            if (_resetStart)
                _characterController.StateReset();

            if (_resetDelayed)
                _characterController.Invoke("StateReset", _stateResetTime * _stateLength);
        }
        private void ResetExit()
        {
            if (_resetEnd)
                _characterController.StateReset();
        }

        [Header("Move")] //================================================================================================================================================================
        [SerializeField] bool _canMove;
        [SerializeField] bool _canRotate;
        [SerializeField] float _moveSpeed;
        private void MoveUpdate()
        {
            if (_canMove)
            {
                _transform.position += _characterController.moveDirection * _moveSpeed * _characterController.speed * Time.fixedDeltaTime;
            }
            if (_canRotate && _characterController.moveDirection != Vector3.zero)
            {
                _transform.rotation = Quaternion.LookRotation(_characterController.moveDirection);
            }
        }

        [Header("Attack")] //==============================================================================================================================================================
        [SerializeField] float _attackDamageRate; // 데미지 배율
        [SerializeField] float _attackRange;
        [Range(0, 180f)]
        [SerializeField] float _attackAngle;
        [SerializeField] LayerMask _attackLayerMask;

        [Space(10f)]
        [SerializeField] bool _isRangedAttack; // 사격 여부
        [SerializeField] bool _isPowerAttack; // 넉백 여부
        [SerializeField] bool _isAttack; // 1타 여부
        [Range(0, 1f)]
        [SerializeField] float _attackDelayTime; // 1타 타이밍
        [SerializeField] bool _isDoubleAttack; // 2타 여부
        [Range(0, 1f)]
        [SerializeField] float _doubleAttackDelayTime; // 2타 타이밍
        [SerializeField] bool _isRepeatingAttack; // 연속공격 여부
        [Range(0, 1f)]
        [SerializeField] float _attackRepeatingTime; // 연속공격 간격

        private void AttackEnter()
        {
            _characterController.damageRate = _attackDamageRate;
            _characterController.attackRange = _attackRange;
            _characterController.attackAngle = _attackAngle;
            _characterController.attackLayerMask = _attackLayerMask;
            _characterController.powerAttack = _isPowerAttack;

            if (_isAttack)
            {
                if (_isRangedAttack == false)
                {
                    _characterController.Invoke("Attack", _attackDelayTime);

                    if (_isDoubleAttack)
                    {
                        _characterController.Invoke("Attack", _doubleAttackDelayTime);
                    }

                    _characterController.Invoke("Attack", _attackDelayTime * _stateLength);
                }
                else
                {
                    _characterController.Invoke("Shoot", _attackDelayTime * _stateLength);
                }
            }
            else if (_isRepeatingAttack)
            {
                // InvokeRepeating("Attack", _attackDelayTime, _attackRepeatingTime * _stateLength);
            }
        }
        private void AttackExit()
        {
            if (_isRepeatingAttack)
            {
                //CancleInvoke
            }
        }

        [Header("Invincible")] //==============================================================================================================================================================
        [SerializeField] bool _isInvincible; // 무적 여부
        [SerializeField] float _invincibleTime; // 무적 시간
        private void Invincible()
        {
            _characterController.InvincibleStart();
            _characterController.Invoke("InvincibleEnd", _invincibleTime);
        }
    }
}