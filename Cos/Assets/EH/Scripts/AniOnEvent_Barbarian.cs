using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniOnEvent_Barbarian : MonoBehaviour
{
    [Header("Swing1")] //============================================================================
    [SerializeField] GameObject _barvarianSwing1Effect;
    [SerializeField] string _barvarianSwing1SoundName;
    [SerializeField] float _barvarianSwing1Delay;
    public void SWING1()
    {
        StartCoroutine(Effect(_barvarianSwing1Effect, _barvarianSwing1SoundName, _barvarianSwing1Delay));
    }

    [Header("Swing2")] //============================================================================
    [SerializeField] GameObject _barvarianSwing2Effect;
    [SerializeField] string _barvarianSwing2SoundName;
    [SerializeField] float _barvarianSwing2Delay;
    public void SWING2()
    {
        StartCoroutine(Effect(_barvarianSwing2Effect, _barvarianSwing2SoundName, _barvarianSwing2Delay));
    }

    [Header("Swing3")] //============================================================================
    [SerializeField] GameObject _barvarianSwing3Effect;
    [SerializeField] string _barvarianSwing3SoundName;
    [SerializeField] float _barvarianSwing3Delay;
    public void SWING3()
    {
        StartCoroutine(Effect(_barvarianSwing3Effect, _barvarianSwing3SoundName, _barvarianSwing3Delay));
    }

    [Header("Spin1")] //============================================================================
    [SerializeField] GameObject _barvarianSpin1Effect;
    [SerializeField] string _barvarianSpin1SoundName;
    [SerializeField] float _barvarianSpin1Delay;
    public void Spin1()
    {
        StartCoroutine(Effect(_barvarianSpin1Effect, _barvarianSpin1SoundName, _barvarianSpin1Delay));
    }
    [Header("Spin2")] //============================================================================
    [SerializeField] GameObject _barvarianSpin2Effect;
    [SerializeField] string _barvarianSpin2SoundName;
    [SerializeField] float _barvarianSpin2Delay;
    public void Spin2()
    {
        StartCoroutine(Effect(_barvarianSpin2Effect, _barvarianSpin2SoundName, _barvarianSpin2Delay));
    }

    IEnumerator Effect(GameObject effect, string soundName, float delay)
    {
        GameObject effectInstanse = Instantiate(effect, transform.position, transform.rotation);
        //SFX_Manager.Instance.VFX(soundName);

        yield return new WaitForSeconds(delay);
        Destroy(effectInstanse);
    }
}
