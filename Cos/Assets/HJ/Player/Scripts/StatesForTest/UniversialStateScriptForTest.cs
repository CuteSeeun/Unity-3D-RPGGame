using UnityEngine;
using CharacterControllerForTest = HJ.CharacterControllerForTest;

namespace HJ
{
    public class UniversialStateScriptForTest : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GetComponents(animator, stateInfo);
            ResetEnter();
            StaminaEnter(stateInfo);
            AdvanceEnter();
            AttackEnter();
            InvincibleEnter();
            ItemEnter();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MoveUpdate();
            AdvanceUpdate();
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ResetExit();
            AttackExit();
            StaminaExit();
            InvincibleExit();
        }

        [Header("Get Components")] //======================================================================================================================================================
        protected CharacterControllerForTest _characterController;
        protected PlayerControllerForTest _playerController;
        protected Transform _transform;
        protected float _stateLength;
        private void GetComponents(Animator animator, AnimatorStateInfo stateInfo)
        {
            _characterController = animator.GetComponent<CharacterControllerForTest>();
            _transform = _characterController.transform;
            if (_characterController.isPlayer)
                _playerController = _characterController.GetComponent<PlayerControllerForTest>();
            _stateLength = stateInfo.length;
        }

        [Header("Stamina")] //========================================================================================================================
        [SerializeField] bool _useStamina;
        private bool _staminaEnough;
        [SerializeField] float _staminaRequired;
        [SerializeField] bool _isStaminaDelayEnd;
        [SerializeField] bool _isStaminaDelayTime;
        [Range(0, 2f)]
        [SerializeField] float _StaminaDelayTime;
        private void StaminaEnter(AnimatorStateInfo stateInfo)
        {
            if (_useStamina)
            {
                _playerController.staminaRequired = _staminaRequired;

                if (_isRepeatingAttack == false)
                {
                    _staminaEnough = _playerController.StaminaUse();

                    if (_staminaEnough)
                    {
                        _playerController.StaminaRecoverStop();

                        if (_isStaminaDelayTime)
                        {
                            _playerController.Invoke("StaminaRecoverStart", _StaminaDelayTime * _stateLength);
                        }
                    }
                }
                else // (_isRepeatingAttack == true)
                {
                    _playerController.StaminaRecoverStop();
                    _playerController.InvokeRepeating("StaminaUse", 0, _attackRepeatingTime * _stateLength);
                }
            }
            else
            {
                _staminaEnough = true;
            }
        }

        private void StaminaExit()
        {
            if (_isRepeatingAttack)
            {
                _playerController.StaminaRecoverStart();
                _playerController.CancelInvoke("StaminaUse");
            }

            if (_isStaminaDelayEnd)
            {
                _playerController.StaminaRecoverStart();
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

        [Header("Advance")] //=============================================================================================================================================================
        // 닷지나 HitB 보고 쓰기
        [SerializeField] bool _isAdvance;
        [SerializeField] bool _canTurn;
        [SerializeField] float _advanceSpeed;
        [SerializeField] float _advanceSpeedReduce;
        private float _advanceSpeedLeft;

        private void AdvanceEnter()
        {
            if (_isAdvance)
            {
                if (_canTurn && _characterController.moveDirection != Vector3.zero)
                {
                    _transform.rotation = Quaternion.LookRotation(_characterController.moveDirection);
                }

                _advanceSpeedLeft = _advanceSpeed;
            }
        }
        private void AdvanceUpdate()
        {
            if(_isAdvance)
            {
                _characterController.transform.position += _advanceSpeedLeft * _characterController.transform.forward * Time.fixedDeltaTime;
                _advanceSpeedLeft -= _advanceSpeedReduce * Time.fixedDeltaTime;
            }
        }

        [Header("Attack")] //==============================================================================================================================================================
        [SerializeField] float _attackDamageRate; // 데미지 배율
        [SerializeField] float _attackRange;
        [Range(0, 180f)]
        [SerializeField] float _attackAngle;
        [SerializeField] LayerMask _attackLayerMask;

        [Space(10f)]
        [SerializeField] bool _isAttack; // 1타 여부
        [SerializeField] bool _isPowerAttack; // 넉백 여부

        [Space(10f)]
        [SerializeField] bool _isRangedAttack; // 사격 여부
        [SerializeField] Missile _missile; // 미사일
        //[SerializeField] float _missileSpeed;
        //[SerializeField] float _missileTimer;
        //[SerializeField] bool _isPiercing;
        //[SerializeField] bool _isExplosive;

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
            _characterController.attackDamageRate = _attackDamageRate;
            _characterController.attackRange = _attackRange;
            _characterController.attackAngle = _attackAngle;
            _characterController.attackLayerMask = _attackLayerMask;
            _characterController.isPowerAttack = _isPowerAttack;

            if (_isAttack)
            {
                if (_isRepeatingAttack == false)
                {
                    _characterController.Invoke("Attack", _attackDelayTime * _stateLength);

                    if (_isDoubleAttack)
                    {
                        _characterController.Invoke("Attack", _doubleAttackDelayTime * _stateLength);
                    }
                }
                else // (_isRepeatingAttack == true)
                {
                    _characterController.InvokeRepeating("Attack", _attackDelayTime * _stateLength, _attackRepeatingTime * _stateLength);
                }
            }

            if (_isRangedAttack)
            {
                _characterController.missile = _missile;

                //_characterController.missileSpeed = _missileSpeed;
                //_characterController.missileTimer = _missileTimer;
                //_characterController.isPiercing = _isPiercing;
                //_characterController.isExplosive = _isExplosive;

                _characterController.Invoke("Shoot", _attackDelayTime * _stateLength);
            }
        }
        private void AttackExit()
        {
            if (_isRepeatingAttack)
            {
                _characterController.CancelInvoke("Attack");
            }
        }

        [Header("Invincible")] //==============================================================================================================================================================
        [SerializeField] bool _isInvincible; // 무적 여부
        [SerializeField] float _invincibleTime; // 무적 시간
        private void InvincibleEnter()
        {
            _characterController.InvincibleStart();
            _characterController.Invoke("InvincibleEnd", _invincibleTime);
        }
        private void InvincibleExit()
        {
            if (_isInvincible)
            {
                _characterController.InvincibleEnd();
            }
        }

        [Header("Item")] //==============================================================================================================================================================
        [SerializeField] bool _weapon1;
        [SerializeField] bool _weapon2;
        [SerializeField] bool _weapon3;
        [SerializeField] bool _potion;
        private void ItemEnter()
        {
            if (_weapon1)
                _characterController.weapon1.SetActive(true);
            else
                _characterController.weapon1.SetActive(false);
            
            if (_weapon2)
                _characterController.weapon2.SetActive(true);
            else
                _characterController.weapon2.SetActive(false);
            
            if (_weapon3)
                _characterController.weapon3.SetActive(true);
            else
                _characterController.weapon3.SetActive(false);

            if (_potion)
                _characterController.potion.SetActive(true);
            else
                _characterController.potion.SetActive(false);
        }
    }
}