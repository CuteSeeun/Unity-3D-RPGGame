using KJ;
using Ricimi;
using Scene_Teleportation_Kit.Scripts.player;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    PlayerDBManager playerDBManager = PlayerDBManager.Instance;
    public static MenuUIManager Instance = null;

    /* 메뉴 UI */
    [Header("Menu UI")]
    public GameObject menuUI;
    /* 장비 팝업창 UI */
    [Header("Equip UI")]
    public GameObject weaponUI;
    public GameObject armorUI;
    public GameObject accUI;
    /* 인벤토리 UI */
    [Header("Inventory UI")]
    public GameObject inventoryUI;
    /* 확인창 UI */
    [Header("Check UI")]
    public GameObject checkLogout;
    public GameObject checkQuit;
    /* UI 골드 표시 */
    [Header("Gold")]
    public TMP_Text goldText;
    private int _currentGold;

    [Header("Class")]
    public TMP_Text classType;

    [Header("ClassImage")]
    public Image knghtImage;
    public Image babarianImage;
    public Image rogueImage;
    public Image mageImage;


    public Image _img_weapon;
    public Image _img_armor;
    public Image _img_accessory;


    public void Awake()
    {
        Instance = this;


    }

    //public GameB

    void Update()
    {
        /* esc 누르면 메뉴 호출 */
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC!!");
            menuUI.SetActive(true);
            classType.text = playerDBManager.LoadGameData(playerDBManager.CurrentShortUID).classType;

            switch(playerDBManager.LoadGameData(playerDBManager.CurrentShortUID).classType)
            {
                case "Knight":
                    knghtImage.gameObject.SetActive(true);
                    break;
                case "Babarian":
                    babarianImage.gameObject.SetActive(true);
                    break;
                case "Rogue":
                    rogueImage.gameObject.SetActive(true);
                    break;
                case "Mage":
                    mageImage.gameObject.SetActive(true);
                    break;
            }
            Cursor.visible = true;

            PauseGame();
        }
    }
    #region 메뉴 팝업
    /* UI창 닫기 */
    public void CloseMenuUI()
    {
        menuUI.SetActive(false);

        //Cursor.visible = false;

        ResumeGame();
    }
    #endregion
    #region 메뉴 팝업시 일시정지
    public void PauseGame()
    {
        Debug.Log("멈춤");
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Debug.Log("재생");
        Time.timeScale = 1;
    }
    #endregion
    #region 장비 팝업
    /* 팝업창 닫기 */
    // 무기 UI 창
    public void OpenWeaponUI()
    {
        weaponUI.SetActive(true);
    }

    public void CloseWeaponUI()
    {
        weaponUI.SetActive(false);
    }
    // 방어구 UI 창
    public void OpenArmorUI()
    {
        armorUI.SetActive(true);
    }

    public void CloseArmorUI()
    {
        armorUI.SetActive(false);
    }
    // 악세서리 UI 창
    public void OpenAccUI()
    {
        accUI.SetActive(true);
    }

    public void CloseAccUI()
    {
        accUI.SetActive(false);
    }
    #endregion
    #region 인벤토리 팝업
    public void OpenInventoryUI()
    {
        inventoryUI.SetActive(true);
    }

    public void CloseInventoryUI()
    {
        inventoryUI.SetActive(false);
    }
    #endregion
    #region 확인창 팝업

    /* 게임 종료 하는 확인창 */
    public void OpenCheckQuit()
    {
        checkQuit.SetActive(true);
    }

    public void CloseCheckQuit()
    {
        checkQuit.SetActive(false);
    }
    #endregion
    #region 골드 표시 (테스트)

    public void PlusGold(int amount)
    {
        _currentGold += amount;
        UpdateGoldText();
    }

    public void MinusGold(int amount)
    {
        _currentGold -= amount;
        UpdateGoldText();
    }

    public void UpdateGoldText()
    {
        goldText.text = $"{_currentGold.ToString("N0")}G";
    }

    /* 스테미나 테스트 */
    public void RandomPlusGold()
    {
        int random = UnityEngine.Random.Range(100, 999);
        PlusGold(random);
        Debug.Log($"현재 골드 : {_currentGold}, 얻은 골드 : {random}");
    }

    public void RandomMinusGold()
    {
        int random = UnityEngine.Random.Range(100, 999);
        MinusGold(random);
        Debug.Log($"현재 골드 : {_currentGold}, 잃은 골드 : {random}");
    }
    #endregion
    #region Scene 이동
    /* 게임 종료 */
    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    #endregion
}
