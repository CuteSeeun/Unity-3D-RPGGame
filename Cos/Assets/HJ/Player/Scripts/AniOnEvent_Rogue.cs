using System.Collections;
using UnityEngine;

namespace HJ
{
    public class AniOnEvent_Rogue : MonoBehaviour
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

        [Header("Slash1")] //============================================================================
        public GameObject _rogueSlash1Effect;
        [SerializeField] int _rogueSlash1SoundNum;
        [SerializeField] float _rogueSlash1Delay;
        public void SLASH1()
        {
            StartCoroutine(Effect(_rogueSlash1Effect, _rogueSlash1SoundNum, _rogueSlash1Delay));
        }

        [Header("Slash2")] //============================================================================
        public GameObject _rogueSlash2Effect;
        [SerializeField] int _rogueSlash2SoundNum;
        [SerializeField] float _rogueSlash2Delay;
        public void SLASH2()
        {
            StartCoroutine(Effect(_rogueSlash2Effect, _rogueSlash2SoundNum, _rogueSlash2Delay));
        }

        [Header("Slash3")] //============================================================================
        public GameObject _rogueSlash3_1Effect;
        public GameObject _rogueSlash3_2Effect;
        [SerializeField] int _rogueSlash3SoundNum;
        [SerializeField] float _rogueSlash3Delay;
        public void SLASH3_1()
        {
            StartCoroutine(Effect(_rogueSlash3_1Effect, _rogueSlash3SoundNum, _rogueSlash3Delay));
        }
        public void SLASH3_2()
        {
            StartCoroutine(Effect(_rogueSlash3_2Effect, _rogueSlash3SoundNum, _rogueSlash3Delay));
        }

        [Header("CrossbowLoading")] //===========================================================================
        public GameObject _rogueCrossbowLoadingEffect;
        [SerializeField] int _rogueCrossbowLoadingSoundNum;
        [SerializeField] float _rogueCrossbowLoadingDelay;
        public void CrossbowLoad()
        {
            StartCoroutine(Effect(_rogueCrossbowLoadingEffect, _rogueCrossbowLoadingSoundNum, _rogueCrossbowLoadingDelay));
        }

        [Header("CrossbowLoaded")] //============================================================================
        public GameObject _rogueCrossbowEffect;
        [SerializeField] int _rogueCrossbowSoundNum;
        [SerializeField] float _rogueCrossbow2Delay;
        public void USE_CROSSBOW()
        {
            StartCoroutine(Effect(_rogueCrossbowEffect, _rogueCrossbowSoundNum, _rogueCrossbow2Delay));
        }

        [Header("CrossbowShot")] //===========================================================================
        public GameObject _rogueCrossbowShotEffect;
        [SerializeField] int _rogueCrossbowShotSoundNum;
        [SerializeField] float _rogueCrossbowShotDelay;
        public void CrossbowShot()
        {
            StartCoroutine(Effect(_rogueCrossbowShotEffect, _rogueCrossbowShotSoundNum, _rogueCrossbowShotDelay));
        }

        IEnumerator Effect(GameObject effect, int soundNum, float delay)
        {
            GameObject effectInstanse = Instantiate(effect, transform.position, transform.rotation);
            SFX_Manager.Instance.VFX(soundNum);

            yield return new WaitForSeconds(delay);
            Destroy(effectInstanse);
        }

        /*
        Dash: "Dash" 10
        Potion: "Potion" 48

        Slash1~3: "Thief attack" 56
        Crossbow: "Shield_defending" 46
        */
    }
}
