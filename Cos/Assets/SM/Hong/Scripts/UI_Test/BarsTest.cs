using HJ;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CharacterController = HJ.CharacterController;

public class BarsTset: MonoBehaviour
{
    CharacterController playerHp;
    PlayerController playerSp;
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
        healthSlider.maxValue = maxHealth;
        menuHealthSlider.maxValue = maxHealth;
        UpdateHealthUI();
        /* 초기 스테미나 설정 */
        //playerSp.stamina = maxStamina;
        /* 슬라이더의 최대값을 최대 스테미너로 설정. */
        staminaSlider.maxValue = maxStamina;
        menuStaminaSlider.maxValue = maxStamina;
        /* 슬라이더의 값 초기화. */
        staminaSlider.value = playerSp.stamina;
        menuStaminaSlider.value = playerSp.stamina;
    }

    void Update()
    {
        UpdateHealthUI();
        UpdateStaminaUI();
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
}
