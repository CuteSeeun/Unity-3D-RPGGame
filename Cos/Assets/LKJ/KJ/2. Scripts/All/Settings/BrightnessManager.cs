using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    [Header("Slider")]
    /* 슬라이더 참조 */
    public Slider brightnessSlider;
    [Header("PostProcessing")]
    /* 볼륨 참조 */
    public Volume volume;
    /* Volume 의 ColorAdjustments 참조 */
    private ColorAdjustments _colorAdjustments;

    void Start()
    {
        // 볼룸의 프로필에서 colorGrading 찾아옴.
        if (volume.profile.TryGet<ColorAdjustments>(out _colorAdjustments))
        {
            // 슬라이더 값이 변경되면, colorGrading 값이 변경되는 메서드를 호출.
            brightnessSlider.onValueChanged.AddListener(HandleSliderChanged);
        }
        
    }

    public void HandleSliderChanged(float value)
    {      
        _colorAdjustments.postExposure.value = value;
        Debug.Log(_colorAdjustments.postExposure.value);
        
    }
}
