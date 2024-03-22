using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraZoomManager : MonoBehaviour
{
    [Header("Slider")]
    public Slider zoomSlider;
    [Header("Camera")]
    public Camera orthographicCamera;

    void Start()
    {
        // 슬라이더의 초기값을 현재 orthographic SIze 값으로 설정 
        zoomSlider.value = orthographicCamera.orthographicSize;
        // 줌 인 최소값, 최대값
        zoomSlider.minValue = 5;
        zoomSlider.maxValue = 10;
        // 슬라이더 값이 변경될때 opthographicSize 값도 변하는 메서드 호출
        zoomSlider.onValueChanged.AddListener(AdjustZoom);
    }

    public void AdjustZoom(float newSize)
    {
        orthographicCamera.orthographicSize = newSize;
    }
}
