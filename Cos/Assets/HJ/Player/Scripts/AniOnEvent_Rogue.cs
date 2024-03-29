using System.Collections;
using UnityEngine;

namespace HJ
{
    public class AniOnEvent_Rogue : MonoBehaviour
    {
        [Header("Slash1")] //============================================================================
        public GameObject _rogueSlash1Effect;
        [SerializeField] string _rogueSlash1SoundName;
        [SerializeField] float _rogueSlash1Delay;
        public void SLASH1()
        {
            StartCoroutine(Effect(_rogueSlash1Effect, _rogueSlash1SoundName, _rogueSlash1Delay));
        }

        [Header("Slash2")] //============================================================================
        public GameObject _rogueSlash2Effect;
        [SerializeField] string _rogueSlash2SoundName;
        [SerializeField] float _rogueSlash2Delay;
        public void SLASH2()
        {
            StartCoroutine(Effect(_rogueSlash2Effect, _rogueSlash2SoundName, _rogueSlash2Delay));
        }

        [Header("Slash3")] //============================================================================
        public GameObject _rogueSlash3_1Effect;
        public GameObject _rogueSlash3_2Effect;
        [SerializeField] string _rogueSlash3_1SoundName;
        [SerializeField] string _rogueSlash3_2SoundName;
        [SerializeField] float _rogueSlash3_1Delay;
        [SerializeField] float _rogueSlash3_2Delay;
        public void SLASH3_1()
        {
            StartCoroutine(Effect(_rogueSlash3_1Effect, _rogueSlash3_1SoundName, _rogueSlash3_1Delay));
        }
        public void SLASH3_2()
        {
            StartCoroutine(Effect(_rogueSlash3_2Effect, _rogueSlash3_2SoundName, _rogueSlash3_2Delay));
        }

        [Header("Crossbow")] //============================================================================
        public GameObject _rogueCrossbowEffect;
        [SerializeField] string _rogueCrossbowSoundName;
        [SerializeField] float _rogueCrossbow2Delay;
        public void USE_CROSSBOW()
        {
            StartCoroutine(Effect(_rogueCrossbowEffect, _rogueCrossbowSoundName, _rogueCrossbow2Delay));
        }

        IEnumerator Effect(GameObject effect, string soundName, float delay)
        {
            GameObject effectInstanse = Instantiate(effect, transform.position, transform.rotation);
            //SFX_Manager.Instance.VFX(soundName);

            yield return new WaitForSeconds(delay);
            Destroy(effectInstanse);
        }
    }
}
