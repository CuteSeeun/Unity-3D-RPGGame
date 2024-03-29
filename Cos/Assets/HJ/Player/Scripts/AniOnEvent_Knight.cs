using System.Collections;
using UnityEngine;

namespace HJ
{
    public class AniOnEvent_Knight : MonoBehaviour
    {
        [Header("Attack1")] //============================================================================
        [SerializeField] GameObject _knightSlash1Effect;
        [SerializeField] string _knightSlash1SoundName;
        [SerializeField] float _knightSlash1Delay;
        public void ATTACK1()
        {
            StartCoroutine(Effect(_knightSlash1Effect, _knightSlash1SoundName, _knightSlash1Delay));
        }
        [Header("Attack2")] //============================================================================
        [SerializeField] GameObject _knightSlash2Effect;
        [SerializeField] string _knightSlash2SoundName;
        [SerializeField] float _knightSlash2Delay;
        public void ATTACK2()
        {
            StartCoroutine(Effect(_knightSlash2Effect, _knightSlash2SoundName, _knightSlash2Delay));
        }
        [Header("Attack3")] //============================================================================
        [SerializeField] GameObject _knightSlash3Effect;
        [SerializeField] string _knightSlash3SoundName;
        [SerializeField] float _knightSlash3Delay;
        public void ATTACK3()
        {
            StartCoroutine(Effect(_knightSlash3Effect, _knightSlash3SoundName, _knightSlash3Delay));
        }
        [Header("Attack4")] //============================================================================
        [SerializeField] GameObject _knightSlash4Effect;
        [SerializeField] string _knightSlash4soundName;
        [SerializeField] float _delay4;
        public void ATTACK4()
        {
            StartCoroutine(Effect(_knightSlash4Effect, _knightSlash4soundName, _delay4));
        }
        [Header("Block Hit")] //============================================================================
        [SerializeField] GameObject _blockHitEffect;
        [SerializeField] string _blockHitSoundName;
        [SerializeField] float _blockHitDelay;
        public void Block_Hit()
        {
            StartCoroutine(Effect(_blockHitEffect, _blockHitSoundName, _blockHitDelay));
        }
        [Header("Block Attack")] //============================================================================
        [SerializeField] GameObject _blockAttackEffect;
        [SerializeField] string _blockAttackSoundName;
        [SerializeField] float _blockAttackDelay;
        public void Block_Attack()
        {
            StartCoroutine(Effect(_blockAttackEffect, _blockAttackSoundName, _blockAttackDelay));
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
