using System.Collections;
using UnityEngine;

namespace HJ
{
    public class AniOnEvent_Knight : MonoBehaviour
    {
        [Header("Dash")] //==============================================================================
        [SerializeField] GameObject _dashEffect;
        [SerializeField] int _dashSoundNum;
        [SerializeField] float _dashDelay;
        public void Dash()
        {
            StartCoroutine(Effect(_dashEffect, _dashSoundNum, _dashDelay));
        }

        [Header("Potion")] //============================================================================
        [SerializeField] GameObject _potionEffect;
        [SerializeField] int _potionSoundNum;
        [SerializeField] float _potionDelay;
        public void Potion()
        {
            StartCoroutine(Effect(_potionEffect, _potionSoundNum, _potionDelay));
        }

        [Header("HitEffect")] //=========================================================================
        [SerializeField] GameObject _hitEffect;
        [SerializeField] int _hitSoundNum;
        [SerializeField] float _hitDelay;
        public void HitEffect()
        {
            StartCoroutine(Effect(_hitEffect, _hitSoundNum, _hitDelay));
        }

        [Header("DieEffect")] //=========================================================================
        [SerializeField] GameObject _dieEffect;
        [SerializeField] int _dieSoundNum;
        [SerializeField] float _dieDelay;
        public void DieEffect()
        {
            StartCoroutine(Effect(_dieEffect, _dieSoundNum, _dieDelay));
        }

        [Header("Attack1")] //============================================================================
        [SerializeField] GameObject _knightSlash1Effect;
        [SerializeField] int _knightSlash1SoundNum;
        [SerializeField] float _knightSlash1Delay;
        public void ATTACK1()
        {
            StartCoroutine(Effect(_knightSlash1Effect, _knightSlash1SoundNum, _knightSlash1Delay));
        }
        [Header("Attack2")] //============================================================================
        [SerializeField] GameObject _knightSlash2Effect;
        [SerializeField] int _knightSlash2SoundNum;
        [SerializeField] float _knightSlash2Delay;
        public void ATTACK2()
        {
            StartCoroutine(Effect(_knightSlash2Effect, _knightSlash2SoundNum, _knightSlash2Delay));
        }
        [Header("Attack3")] //============================================================================
        [SerializeField] GameObject _knightSlash3Effect;
        [SerializeField] int _knightSlash3SoundNum;
        [SerializeField] float _knightSlash3Delay;
        public void ATTACK3()
        {
            StartCoroutine(Effect(_knightSlash3Effect, _knightSlash3SoundNum, _knightSlash3Delay));
        }
        [Header("Attack4")] //============================================================================
        [SerializeField] GameObject _knightSlash4Effect;
        [SerializeField] int _knightSlash4soundNum;
        [SerializeField] float _knightSlash4delay;
        public void ATTACK4()
        {
            StartCoroutine(Effect(_knightSlash4Effect, _knightSlash4soundNum, _knightSlash4delay));
        }
        [Header("Block Hit")] //============================================================================
        [SerializeField] GameObject _blockHitEffect;
        [SerializeField] int _blockHitSoundNum;
        [SerializeField] float _blockHitDelay;
        public void Block_Hit()
        {
            StartCoroutine(Effect(_blockHitEffect, _blockHitSoundNum, _blockHitDelay));
        }
        [Header("Block Attack")] //============================================================================
        [SerializeField] GameObject _blockAttackEffect;
        [SerializeField] int _blockAttackSoundNum;
        [SerializeField] float _blockAttackDelay;
        public void Block_Attack()
        {
            StartCoroutine(Effect(_blockAttackEffect, _blockAttackSoundNum, _blockAttackDelay));
        }

        IEnumerator Effect(GameObject effect, int soundNum, float delay)
        {
            GameObject effectInstanse = Instantiate(effect, transform.position, transform.rotation);
            SFX_Manager.Instance.VFX(soundNum);

            yield return new WaitForSeconds(delay);
            Destroy(effectInstanse);
        }

        /*
        Potion: "Potion" 48
        Dash: "Dash" 10

        Attack1~4: "Player_Knight_attack" 44
        BlockHit: "Player_Shield_defending" 46
        BlockAttack: "Player_Shield_attack" 45
        */
    }
}
