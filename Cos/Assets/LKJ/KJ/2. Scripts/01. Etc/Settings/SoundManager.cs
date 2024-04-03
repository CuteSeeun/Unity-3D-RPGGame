using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // 소리 볼륨 크기에 따른 이미지 변화
    // 슬라이더, 이미지( 기본이미지 음소거용, 중간용)
    // 배경음악과 효과음 둘다 조절해야함
    // 토글형식으로 효과음, 배경음악을 따로 음소거할 수 있음.
    [Header("Slider")]
    public Slider volumeSlider;
    //public TMP_Text volumeText;
    [Header("Volume Image")]
    /* 볼륨 이미지들 */
    public Image volumeImage;
    public Sprite muteSprite;
    public Sprite lowVolumeSprite;
    public Sprite highVolumeSprite;
    [Header("AudioSource")] // 배경음악, 효과음 오디오 믹서로 그룹화해서 관리할 생각. 지금은 테스트용.
    /* 오디오 소스 */
    public AudioSource audioSource;
    [Header("Mute Background Music")]
    /* 배경음악 음소거 */
    public Slider backgroundMusicSlider;

    void Start()
    {
        #region 사운드 설정
        // 슬라이더 최대값 최소값 설정.
        volumeSlider.minValue = 0;
        volumeSlider.maxValue = 10;

        // 오디오 소스 볼륨 값이 0.1~1.0 이므로 슬라이더의 값인 1~10 에 맞추기 위해 * 10 을 함
        // 슬라이더의 값을 최대값으로 설정.
        float initialVolumeValue = audioSource.volume * 10;
        volumeSlider.value = initialVolumeValue;

        /* 슬라이더 값이 변할 때 이미지 및 볼륨 변하는 메서드 호출. */
        volumeSlider.onValueChanged.AddListener(HandleVolumeChanged);
        #endregion
        #region 배경음악 음소거
        /* 음소거 */
        backgroundMusicSlider.minValue = 0;
        /* 소리 나옴. */
        backgroundMusicSlider.maxValue = 1;
        /* 슬라이더 값이 변할 때 볼륨도 같이 변하는 메서드 호출. */
        backgroundMusicSlider.onValueChanged.AddListener(BackgroundMusicSwitch);
        #endregion
    }

    // 볼륨에 따른 이미지들
    public void HandleVolumeChanged(float value)
    {
        /* 오디오 소스 볼륨 슬라이더의 값에 맞춤. */
        audioSource.volume = value * 0.1f;
        //volumeText.text = $"{value}";
        if (value <= 0)
        {
            volumeImage.sprite = muteSprite;
        }
        else if (value <= 5)
        {
            volumeImage.sprite = lowVolumeSprite;
        }
        else
        {
            volumeImage.sprite = highVolumeSprite;
        }
    }

    // 배경 음악 음소거 기능
    public void BackgroundMusicSwitch(float value)
    {
        // 슬라이더 값에 따라 볼륨 설정
        audioSource.volume = Mathf.Round(value);   
    }
}
