using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    void Start()
    {
        #region 슬라이더를 이용해 로딩창에서 다음 씬으로 넘어가기.
        // LoadAsyncSceneCoroutine() 을 코루틴으로 시작.
        StartCoroutine(LoadAsyncSceneCoroutine());
        #endregion
        #region 로딩 동안 게임 스토리 텍스트 랜덤으로 생성
        // 리스트에 gameStory 컴포넌트 추가
        gameStoryList.Add(gameStory);
        gameStoryList.Add(gameStory2);
        gameStoryList.Add(gameStory3);

        // 랜덤 범위는 리스트 길이
        _randomNum = Random.Range(0, gameStoryList.Count);
        randomStory = gameStoryList[_randomNum];

        // 랜덤으로 선택된 텍스트만 활성화 나머지는 비활성화.
        foreach (var story in gameStoryList)
        {
            // 모든 스토리를 비활성화.
            story.gameObject.SetActive(false);
        }
        // 랜덤으로 선택된 스토리 활성화.
        randomStory.gameObject.SetActive(true);
        #endregion
    }
    #region 슬라이더를 이용해 로딩창에서 다음 씬으로 넘어가기. 
    [Header("Slider")]
    public Slider slider;
    public string sceneName;

    private float _time;


    IEnumerator LoadAsyncSceneCoroutine()
    {
        // sceneName 으로 비동기 형식으로 넘어가게 하는 operation 생성.
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        // operation 이 완료되어도 다음 씬으로 넘어가는걸 막음.
        operation.allowSceneActivation = false;

        // operation.isDone 이 false 일 동안 반복.
        while (!operation.isDone)
        {
            // 경과 시간을 정확하게 누적.
            _time += Time.deltaTime;
            // slider의 밸류값을 10초까지 진행상태 표시
            slider.value = _time / 10f;
            
            // 10초가 지나면
            if (_time > 10)
            {
                // 다음 씬으로 넘어가도록 활성화.
                operation.allowSceneActivation = true;
            }
            
            //다음 프레임까지 대기.
            yield return null;
        }
    }
    #endregion
    #region 로딩 동안 게임 스토리 텍스트 랜덤으로 생성
    [Header("RandomText")]
    public TMP_Text randomStory;
    public TMP_Text gameStory;
    public TMP_Text gameStory2;
    public TMP_Text gameStory3;

    private int _randomNum;

    List<TMP_Text> gameStoryList = new List<TMP_Text>();
    #endregion
}
