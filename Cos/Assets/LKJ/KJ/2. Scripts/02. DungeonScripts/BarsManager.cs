using HJ;
using Ricimi;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarsManager : MonoBehaviour
{
    /* 슬라이더 */
    [Header("Slider")]
    public Slider healthSlider;
    public Slider staminaSlider;
    /* Min/Max 체력, 스테미나 표시 */
    [Header("Cur / Max")]
    public TMP_Text healthCur;
    public TMP_Text healthMax;
    public TMP_Text staminaCur;
    public TMP_Text staminaMax;
    /* 최대 체력 */
    [Header("MaxHp")]
    public float maxHealth = 100f;
    /* 최대 스테미나 */
    [Header("MaxSp")]
    public float maxStamina = 100f;
    [Header("StaminaRecovery")]
    /* 스테미나 회복 */
    public float staminaRecoveryRate = 5f;

    /* 현재 체력 */
    private float _currentHealth;
    /* 현재 스테미나 */
    private float _currentStamina;

    

    void Start()
    {
        /* 초기 체력 설정. */
        _currentHealth = maxHealth;
        /* 슬라이더의 최대값을 최대 체력으로 설정. */
        healthSlider.maxValue = maxHealth;
        UpdateHealthUI();
        /* 초기 스테미나 설정 */
        _currentStamina = maxStamina;
        /* 슬라이더의 최대값을 최대 스테미너로 설정. */
        staminaSlider.maxValue = maxStamina;
        /* 슬라이더의 값 초기화. */
        staminaSlider.value = _currentStamina;
    }

    void Update()
    {
        UpdateStaminaUI();
    }
    #region 바 테스트
    // 데미지 입었을 때.
    public void TakeDamage(float amount)
    {
        Debug.Log("데미지!");
        //_isDamaged = true;
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    public void Heal(float amount)
    {
        Debug.Log("힐!");
        //_isHealed = true;
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    public void Action(float amount)
    {
        Debug.Log("스테미나 사용!");
        _currentStamina -= amount;
        _currentStamina = Mathf.Clamp(_currentStamina, 0, maxStamina);
        UpdateStaminaUI();
    }
    /* 스테미나 테스트 */
    public void RandomAction()
    {
        float random = Random.Range(10f, 45f);
        Action(random);
    }
    #endregion

    /* 체력바 업데이트 */
    void UpdateHealthUI()
    {
        Debug.Log($"Current Health: {_currentHealth}, Max Health: {maxHealth}, Slider Value: {healthSlider.value}");
        /* Slider, 텍스트 업데이트. */
        healthSlider.value = _currentHealth;
        healthCur.text = $"{_currentHealth}";
        healthMax.text = $"{maxHealth}";
    }
    
    // 스테미나 업데이트
    void UpdateStaminaUI()
    {
        /* 스테미나 업데이트 */
        if (_currentStamina < maxStamina)
        {
            Debug.Log($"Current SP : {_currentStamina}, Max SP : {maxStamina}, Slider Value : {staminaSlider.value}");
            /* 스테미나가 시간에 따라 자동으로 채워짐. */
            _currentStamina += staminaRecoveryRate * Time.deltaTime;
            /* 최대 스테미너 넘지 않음. */
            _currentStamina = Mathf.Min(_currentStamina, maxStamina);
            /* 슬라이더 업데이트 */
            staminaSlider.value = _currentStamina;
            /* 텍스트 업데이트 */
            /* 정수로 변환하여 소수점 안나오게 함. */
            staminaCur.text = Mathf.RoundToInt(_currentStamina).ToString();
            staminaMax.text = $"{maxStamina}";
        }
    }
}
