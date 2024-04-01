using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KJ
{
    public class LoadingSceneManager : MonoBehaviour
    {
        void Start()
        {
            #region 로딩 씬 데이터 로드 및 shortUID 체크 후 상황에 따라 씬 변경. (슬라이더) 
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

        void Update()
        {
            currentProgress = Mathf.Lerp(currentProgress, targetProgress, Time.deltaTime * 10);
            /* 슬라이더 값 업데이트 */
            slider.value = currentProgress;
        }

        #region 로딩 씬 데이터 로드 및 shortUID 체크 후 상황에 따라 씬 변경. (슬라이더) 
        [Header("Slider")]
        public Slider slider;
        public string sceneName;
        /* 현재 진행 상태 */
        float currentProgress = 0f;
        /* 목표 진행 상태 */
        float targetProgress = 0f;

        IEnumerator LoadAsyncSceneCoroutine()
        {
            /* 플레이어 데이터 로드 */
            yield return  PlayerDBManager.Instance.LoadPlayerDB();
            targetProgress += 0.5f;
            /* 아이템 데이터 로드 */
            yield return ItemDBManager.Instance.LoadItemDB();
            targetProgress += 0.5f;

            /* 저장된 데이터 확인 */
            bool hasSavedData = PlayerDBManager.Instance.CheckPlayerData(PlayerDBManager.Instance.CurrentShortUID);

            if (hasSavedData)
            {
                /* 저장된 데이터가 있으면 로딩씬을 통해 게임 진행. */
                PlayerDBManager.Instance.LoadGameData(PlayerDBManager.Instance.CurrentShortUID);
                sceneName = "VillageScene";
            }
            else
            {
                /* 없으면 캐릭터 선택창으로 이동. */
                sceneName = "CharacterSelectScene";
            }

            targetProgress = 1f;

            // sceneName 으로 비동기 형식으로 넘어가게 하는 operation 생성.
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            // operation 이 완료되어도 다음 씬으로 넘어가는걸 막음.
            operation.allowSceneActivation = false;

            // operation.isDone 이 false 일 동안 반복.
            while (!operation.isDone)
            {
                if (targetProgress >= 1f)
                {
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

}
