using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // 소리 볼륨 크기에 따른 이미지 변화
    // 슬라이더, 이미지( 기본이미지 음소거용, 중간용)
    // 배경음악과 효과음 둘다 조절해야함
    // 토글형식으로 효과음, 배경음악을 따로 음소거할 수 있음.
    [Header("Slider")]
    public Slider soundSlider;
    [Header("Volume Image")]
    /* 볼륨 이미지들 */
    public Image volumeImage;
    public Sprite muteImage;
    public Sprite lowVolumeImage;
    public Sprite highVolumeImage;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
