using System.Collections;
using UnityEngine;

namespace HJ
{
    public class AniOnEvent_Barbarian : MonoBehaviour
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

        [Header("Swing1")] //============================================================================
        [SerializeField] GameObject _barvarianSwing1Effect;
        [SerializeField] int _barvarianSwing1SoundNum;
        [SerializeField] float _barvarianSwing1Delay;
        public void SWING1()
        {
            StartCoroutine(Effect(_barvarianSwing1Effect, _barvarianSwing1SoundNum, _barvarianSwing1Delay));
        }

        [Header("Swing2")] //============================================================================
        [SerializeField] GameObject _barvarianSwing2Effect;
        [SerializeField] int _barvarianSwing2SoundNum;
        [SerializeField] float _barvarianSwing2Delay;
        public void SWING2()
        {
            StartCoroutine(Effect(_barvarianSwing2Effect, _barvarianSwing2SoundNum, _barvarianSwing2Delay));
        }

        [Header("Swing3")] //============================================================================
        [SerializeField] GameObject _barvarianSwing3Effect;
        [SerializeField] int _barvarianSwing3SoundNum;
        [SerializeField] float _barvarianSwing3Delay;
        public void SWING3()
        {
            StartCoroutine(Effect(_barvarianSwing3Effect, _barvarianSwing3SoundNum, _barvarianSwing3Delay));
        }

        [Header("Spin1")] //============================================================================
        [SerializeField] GameObject _barvarianSpin1Effect;
        [SerializeField] int _barvarianSpin1SoundNum;
        [SerializeField] float _barvarianSpin1Delay;
        public void Spin1()
        {
            StartCoroutine(Effect(_barvarianSpin1Effect, _barvarianSpin1SoundNum, _barvarianSpin1Delay));
        }
        [Header("Spin2")] //============================================================================
        [SerializeField] GameObject _barvarianSpin2Effect;
        [SerializeField] int _barvarianSpin2SoundNum;
        [SerializeField] float _barvarianSpin2Delay;
        public void Spin2()
        {
            StartCoroutine(Effect(_barvarianSpin2Effect, _barvarianSpin2SoundNum, _barvarianSpin2Delay));
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

        Swing1~3: "Player_barbarian_attack" 42
        Spin1 ~2: "Player_barbarian_spinningattack" 43
        */
    }
}
