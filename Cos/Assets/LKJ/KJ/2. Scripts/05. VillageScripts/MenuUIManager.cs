using Ricimi;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
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
    public GameObject checkUI;
    public TMP_Text titleText;
    public TMP_Text warningText;
    /* 확인창 예 버튼 동적 할당 */
    [Header("Dynamic Button")]
    public Button yesButton;
    /* UI 골드 표시 */
    [Header("Gold")]
    public TMP_Text goldText;
    private int _currentGold;

    void Update()
    {
        /* esc 누르면 메뉴 호출 */
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC!!");
            menuUI.SetActive(true);

            Cursor.visible = true;

            PauseGame();
        }
    }
    #region 메뉴 팝업
    /* UI창 닫기 */
    public void CloseMenuUI()
    {
        menuUI.SetActive(false);

        Cursor.visible = false;

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
    /* 확인창 내용에 따라 다르게 팝업 */
    public void OpenCheck(string situation)
    {
        switch (situation)
        {
            case "Quit":
                titleText.text = "정말 종료하시겠습니까?";
                warningText.text = "저장 후 종료됩니다...";
                break;
            case "Login":
                titleText.text = "로그인 화면으로 돌아가시겠습니까?";
                warningText.text = "오늘 할 일 다하셨나요?";
                break;
        }
        checkUI.SetActive(true);
    }

    public void CloseCheck()
    {
        checkUI.SetActive(false);
    }


    #endregion
    #region 버튼 동적 할당

    /*  */
    public void CheckUIWithAction(string situation)
    {
        switch (situation)
        {
            case "Quit":
                ConfigureYesButton(Quit);
                break;
            case "Login":
                ConfigureYesButton(Login);
                break;
        }
        OpenCheck(situation);
    }

    /* 버튼 동적 할당 */
    public void ConfigureYesButton(Action yesAction)
    {
        // 기존 리스너들 제거.
        yesButton.onClick.RemoveAllListeners();
        // 새로운 액션 할당.
        yesButton.onClick.AddListener(() => yesAction());
        // 확인창 자동으로 닫는 리스너 추가
        yesButton.onClick.AddListener(CloseCheck);
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
        // 자동으로 PlayerDB 를 저장 하고 종료해야 함.
        // 저장할 때 로딩팝업창 5초 생성 후 종료.
        SceneManager.LoadScene("TestGameQuit");
    }

    /* 로그인 화면으로 이동 */
    public void Login()
    {
        // 자동으로 Player 를 저장하고 종료해야 함.
        // 저장할 때 로딩팝업창 5초 생성 후 종료.
        SceneManager.LoadScene("LoginScnen");
    }
    #endregion
}
