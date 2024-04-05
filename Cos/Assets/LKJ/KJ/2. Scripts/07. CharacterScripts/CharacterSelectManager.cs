using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        /**/
        private string _class;

        void Awake()
        {
            //_gameData = NetData.Instance._gameData;
        }

        public GameObject characterInstance { get; private set; }


        public void CreateCharacter(string className)
        {
            Debug.Log("호출1");
            string shortUID = PlayerDBManager.Instance.CurrentShortUID;
            
            ClassType classType;
            if (Enum.TryParse(className, true, out classType))
            {
                Debug.Log("호출2");
                Class classData;
                /* Classes 에서 선택된 class 데이터 찾기 */
                GameData gameData = NetData.Instance.gameData;
                Debug.Log($"변환 성공: {classType}");
                classData = gameData.classes[classType];
                
                if (classData != null)
                {
                    Debug.Log("호출3");
                    Player playerData;
                    GameData _gameData = NetData.Instance._gameData;

                    Player p = new Player();
                    p.uid = FirebaseAuthManager.Instance._user.UserId;
                    PlayerDBManager.Instance.SaveOrUpdatePlayerData(p.uid, p);

                    if (_gameData.players.TryGetValue(p.shortUID, out playerData))
                    {
                        Debug.Log("호출4");
                        /* Player 객체에 Class 데이터 적용. */
                        playerData.classType = className;
                        playerData.inventory.items.Clear();
                        foreach (var item in classData.inventory.items)
                        {
                            Debug.Log($"아이템 id : {item.id}");
                            playerData.inventory.items.Add(new Item { id = item.id, quantity = item.quantity });                            
                        }
                        playerData.gold = classData.gold;
                        Debug.Log($"클래스 타입 : {playerData.classType}");

                        Debug.Log("캐릭터 프리팹" + className);

                        /* 프리팹 로드 및 인스턴스화. */

                        GameObject characterPrefab = Resources.Load<GameObject>($"Prefabs/{className}");
                        /* spawn 위치 Vector3 값 설정. */
                        Vector3 spawnPosition = new Vector3(-45, 3, 0);
                        if (characterPrefab != null)
                        {
                            Debug.Log("호출5");
                            characterInstance = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
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

                        Debug.Log($"PlayerShorUID: {playerData.shortUID}");

                        Debug.Log($"생성된 캐릭터 적용 여부: {classData.baseHp}");

                        // 인벤토리 아이템 적용 후
                        Debug.Log($"Applied inventory items to character. Item count: {playerData.inventory.items.Count}");

                        // 캐릭터의 초기 골드 설정 후
                        Debug.Log($"Character starting gold: {playerData.gold}");

                        // 프리팹 로드 및 인스턴스화 후 (예시 위치를 Vector3.zero로 가정)
                        Debug.Log($"Character spawned at position: {spawnPosition}");
                        /* 스킬 개수가 온전히 다 있는지 확인. */
                        Debug.Log($" 스킬 {classData.skills.Count}");
                    }
                }
            }
        }
        public void LoadToVillage()
        {
            SceneManager.LoadScene("VillageScene");
        }

        #region 팝업 UI
        /* 동적 할당 */
        public void OpenCheckPopupText(string className)
        {
            _class = className;

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
            _checkPopupUI.SetActive(true);
        }
        
        public void ConfigureYesButton()
        {
            Debug.Log("새로운 리스너 할당됨.");
            CreateCharacter(_class);
        }

        public void CloseCheckPopup()
        {
            _checkPopupUI.SetActive(false);
        }
        #endregion
    }
}

