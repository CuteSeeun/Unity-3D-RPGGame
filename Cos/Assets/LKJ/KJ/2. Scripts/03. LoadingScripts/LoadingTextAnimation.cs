using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingTextAnimation : MonoBehaviour
{
    [Header("LoadingText")]
    // text 컴포넌트
    public TextMeshProUGUI loadingText;
    // '.' 텍스트 속도
    public float dotSpeed = 0.5f;
    // 기본 문장
    public string baseText = "로딩 중";
    // 현재 '.' 텍스트 개수
    public int dotCount = 0;
    void Start()
    {
        StartCoroutine(AnimateDots());
    }

    IEnumerator AnimateDots()
    {
        while (true)
        {
            // '.' 의 개수를 0 ~ 4까지 반복
            dotCount = (dotCount + 1) % 5;
            // 기본 텍스트에 '.' 추가
            loadingText.text = baseText + new string('.', dotCount);
            // 지정된 시간 동안 대기
            yield return new WaitForSeconds(dotSpeed);
        }
    }
}
