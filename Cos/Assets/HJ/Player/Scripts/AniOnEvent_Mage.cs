using System.Collections;
using UnityEngine;

namespace HJ
{
    public class AniOnEvent_Mage : MonoBehaviour
    {
        [Header("Potion")] //============================================================================
        [SerializeField] GameObject _potionEffect;
        [SerializeField] string _potionSoundName;
        [SerializeField] float _potionDelay;
        public void Potion()
        {
            StartCoroutine(Effect(_potionEffect, _potionSoundName, _potionDelay));
        }

        [Header("Spell 1")] //============================================================================
        public GameObject _mageSpell1effect;
        [SerializeField] string _mageSpell1SoundName;
        [SerializeField] float _mageSpell1Delay;
        public void Spell1()
        {
            //StartCoroutine(Effect(_mageSpell1effect, _mageSpell1SoundName, _mageSpell1Delay));
        }

        [Header("Spell 2")] //============================================================================
        public GameObject _mageSpell2effect;
        [SerializeField] string _mageSpell2SoundName;
        [SerializeField] float _mageSpell2Delay;
        public void Spell2()
        {
            StartCoroutine(Effect(_mageSpell2effect, _mageSpell2SoundName, _mageSpell2Delay));
        }

        [Header("Spell 3")] //============================================================================
        public GameObject _mageSpell3effect;
        [SerializeField] string _mageSpell3SoundName;
        [SerializeField] float _mageSpell3Delay;
        public void Spell3()
        {
            StartCoroutine(Effect(_mageSpell3effect, _mageSpell3SoundName, _mageSpell3Delay));
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