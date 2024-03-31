using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace KJ
{
    public class CharacterSelectManager : MonoBehaviour
    {
        [Header("DynamicButton")]
        /* 팝업 창 버튼 (동적) */
        public Button yesButton;
        [Header("Popup")]
        /* 팝업 창 활성화 */
        [SerializeField]
        private GameObject _checkPopupUI;
        public TMP_Text selectText;
        [Header("Loading")]
        public GameObject loadingUI;


        public void CreateCharacter(string className)
        {
            string shortUID = PlayerDBManager.Instance.CurrentShortUID;
            GameData gameData = GameData.Instance;

            ClassType classType;
            if (Enum.TryParse(className, true, out classType))
            {
                Class classData;
                /* Classes 에서 선택된 class 데이터 찾기 */
                if (gameData.classes.TryGetValue(classType, out classData))
                {
                    Player playerData;
                    if (gameData.players.TryGetValue(shortUID, out playerData))
                    {
                        /* Player 객체에 Class 데이터 적용. */
                        playerData.classType = className;
                        playerData.inventory.items.Clear();
                        foreach (var item in classData.inventory.items)
                        {
                            playerData.inventory.items.Add(new Item { id = item.id, quantity = item.quantity });
                        }
                        playerData.gold = classData.gold;

                        /* 프리팹 로드 및 인스턴스화. */
                        GameObject characterPrefab = Resources.Load<GameObject>($"Prefabs/{className}");
                        /* spawn 위치 Vector3 값 설정. */
                        Vector3 spawnPosition = new Vector3(-400, -2, -5);
                        if (characterPrefab != null)
                        {
                            GameObject characterInstance = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
                            /* 씬 전환시 파괴 방지 */
                            DontDestroyOnLoad(characterInstance);

                        }
                        else
                        {
                            Debug.Log($"캐릭터 프리팹 로드 실패 {className}");
                        }

                        loadingUI.SetActive(true);
                        Invoke("LoadToVillage", 5);

                        // 클래스 데이터 로딩 성공 시
                        Debug.Log($"Creating character of class: {className}");

                        // 인벤토리 아이템 적용 후
                        Debug.Log($"Applied inventory items to character. Item count: {playerData.inventory.items.Count}");

                        // 캐릭터의 초기 골드 설정 후
                        Debug.Log($"Character starting gold: {playerData.gold}");

                        // 프리팹 로드 및 인스턴스화 후 (예시 위치를 Vector3.zero로 가정)
                        Debug.Log($"Character spawned at position: {spawnPosition}");
                    }
                }
            }
        }
        public void LoadiToVillage()
        {
            SceneManager.LoadScene("VillageScene");
        }

        #region 팝업 UI
        /* 동적 할당 */
        public void OpenCheckPopupText(string className)
        {
            switch (className)
            {
                case "Knight":
                    selectText.text = "기사로 캐릭터를 생성하시겠습니까?";
                    break;
                case "Barbarian":
                    selectText.text = "바바리안으로 캐릭터를 생성하시겠습니까?";
                    break;
                case "Rogue":
                    selectText.text = "로그로 캐릭터를 생성하시겠습니까?";
                    break;
                case "Mage":
                    selectText.text = "메이지로 캐릭터를 생성하시겠습니까?";
                    break;
            }
        }

        public void OpenCheckPopupButton(string className)
        {
            /* 팝업 텍스트 설정. */
            OpenCheckPopupText(className);

            /* 확인 버튼 기존 리스너를 모두 제거. */
            yesButton.onClick.RemoveAllListeners();

            /* 확인 버튼에 새로운 리스너 할당. */
            yesButton.onClick.AddListener(() => CreateCharacter(className));

            /* 확인창 UI 활성화 */
            _checkPopupUI.SetActive(true);
        }

        public void CloseCheckPopup()
        {
            _checkPopupUI.SetActive(false);
        }
        #endregion
    }
}

