using HJ;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CharacterController = HJ.CharacterController;

public class BarsTset: MonoBehaviour
{
    CharacterController playerHp;
    PlayerController playerSp;

    /* 캐릭터 사망 UI*/
    [Header("Player Death UI")]
    public GameObject playerDeathUI;
    /* 슬라이더 */
    [Header("Slider")]
    public Slider healthSlider;
    public Slider staminaSlider;
    [Header("MenuSlider")]
    public Slider menuHealthSlider;
    public Slider menuStaminaSlider;
    /* Min/Max 체력, 스테미나 표시 */
    [Header("Cur / Max")]
    public TMP_Text healthCur;
    public TMP_Text healthMax;
    public TMP_Text staminaCur;
    public TMP_Text staminaMax;
    public TMP_Text menuHealthCur;
    public TMP_Text menuStaminaCur;
    public TMP_Text potionNum;
    /* 최대 체력 */
    [Header("MaxHp")]
    public float maxHealth = 100f;
    /* 최대 스테미나 */
    [Header("MaxSp")]
    public float maxStamina = 100f;
    //[Header("StaminaRecovery")]
    /* 스테미나 회복 */
    //public float staminaRecoveryRate = 5f;




    void Start()
    {
        playerHp = FindAnyObjectByType<CharacterController>();
        playerSp = FindAnyObjectByType<PlayerController>();
        /* 초기 체력 설정. */
        //playerHp.hp = maxHealth;
        /* 슬라이더의 최대값을 최대 체력으로 설정. */
        healthSlider.maxValue = playerHp.hpMax;
        menuHealthSlider.maxValue = playerHp.hpMax;
        UpdateHealthUI();
        /* 초기 스테미나 설정 */
        //playerSp.stamina = maxStamina;
        /* 슬라이더의 최대값을 최대 스테미너로 설정. */
        staminaSlider.maxValue = playerSp.spMax;
        menuStaminaSlider.maxValue = playerSp.spMax;
        /* 슬라이더의 값 초기화. */
        staminaSlider.value = playerSp.stamina;
        menuStaminaSlider.value = playerSp.stamina;
        /* 플레이어 사망 시 노출되는 UI 숨김*/
        playerDeathUI.SetActive(false);
    }

    /// <summary>
    /// 실시간으로 체력바와 스테미나바를 업데이트하고 플레이어의 체력이 0이 되면 3초 후 Gameover UI 호출
    /// </summary>
    void Update()       
    {
        UpdateHealthUI();
        UpdateStaminaUI();
        if(playerHp.hp <= 0)
        {
            Invoke("Gameover", 3);
        }
    }
    
    /* 체력바 업데이트 */
    void UpdateHealthUI()
    {
        //Debug.Log($"Current Health: {playerHp.hp}, Max Health: {playerHp.hpMax}, Slider Value: {healthSlider.value}");
        /* Slider, 텍스트 업데이트. */
        healthSlider.value = playerHp.hp;
        menuHealthSlider.value = playerHp.hp;
        healthCur.text = $"{playerHp.hp}";
        menuHealthCur.text = $"{playerHp.hp}";
        healthMax.text = $"{playerHp.hpMax}";
        potionNum.text = $"{playerSp.potionNumber} /";
    }

    // 스테미나 업데이트
    void UpdateStaminaUI()
    {
        /* 스테미나 업데이트 */
        
        Debug.Log($"Current SP : {playerSp.stamina}, Max SP : {maxStamina}, Slider Value : {staminaSlider.value}");
        /* 스테미나가 시간에 따라 자동으로 채워짐. */
        //playerSp.stamina += staminaRecoveryRate * Time.deltaTime;
        /* 최대 스테미너 넘지 않음. */
        playerSp.stamina = Mathf.Min(playerSp.stamina, maxStamina);
        /* 슬라이더 업데이트 */
        staminaSlider.value = playerSp.stamina;
        menuStaminaSlider.value = playerSp.stamina;
        /* 텍스트 업데이트 */
        /* 정수로 변환하여 소수점 안나오게 함. */
        staminaCur.text = Mathf.RoundToInt(playerSp.stamina).ToString();
        menuStaminaCur.text = Mathf.RoundToInt(playerSp.stamina).ToString();
        staminaMax.text = $"{maxStamina}";      
    }

    //플레이어 사망 시
    void Gameover()
    {
        playerDeathUI.SetActive(true);
        Time.timeScale = 0;
    }

    //마을로 이동
    public void LoadVillage()
    {
        // 데이터베이스에 현재 씬 데이터 저장 
        // 마을 씬으로 이동
    }
}
