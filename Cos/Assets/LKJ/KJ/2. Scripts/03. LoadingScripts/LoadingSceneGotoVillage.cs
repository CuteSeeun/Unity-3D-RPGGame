using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KJ
{
    public class LoadingSceneGotoVillage : MonoBehaviour
    {
        Transform characterSelectManager = GameObject.FindWithTag("Player").transform;

        void Start()
        {
            #region 마을 -> 던전 로딩 씬 
            // LoadAsyncSceneCoroutine() 을 코루틴으로 시작.
            StartCoroutine(LoadAsyncSceneCoroutine());
            #endregion
        }

        void Update()
        {
            currentProgress = Mathf.Lerp(currentProgress, targetProgress, Time.deltaTime * 10);
            /* 슬라이더 값 업데이트 */
            slider.value = currentProgress;
        }

        #region 마을 -> 던전 로딩 씬 
        [Header("Slider")]
        public Slider slider;
        public string sceneName;
        /* 현재 진행 상태 */
        float currentProgress = 0f;
        /* 목표 진행 상태 */
        float targetProgress = 0f;



        IEnumerator LoadAsyncSceneCoroutine()
        {
            /* 아이템 데이터 로드 */
            yield return ItemDBManager.Instance.LoadItemDB();
            targetProgress += 1f;

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

            

            SceneManager.LoadSceneAsync(sceneName);
        }
        #endregion
    }

}
