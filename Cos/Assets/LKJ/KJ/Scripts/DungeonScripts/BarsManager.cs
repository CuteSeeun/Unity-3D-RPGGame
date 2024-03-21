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
    /* 최소/최대 체력 표시 */
    [Header("Cur / Max")]
    public TMP_Text healthCur;
    public TMP_Text healthMax;
    /* 최대 체력 */
    [Header("MaxHp")]
    public float maxHealth = 100f;
    /* 현재 체력 */
    private float _currentHealth;

    void Start()
    {
        // 초기 체력 설정.
        _currentHealth = maxHealth;
        // 슬라이더의 최대값을 최대 체력으로 설정.
        healthSlider.maxValue = maxHealth;
        UpdateHealthUI();
    }

    // 데미지 입었을 때.
    public void TakeDamage(float amount)
    {
        Debug.Log("데미지!");
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    public void Heal(float amount)
    {
        Debug.Log("힐!");
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    // 체력바 업데이트
    void UpdateHealthUI()
    {
        Debug.Log($"Current Health: {_currentHealth}, Max Health: {maxHealth}, Slider Value: {healthSlider.value}");
        // Slider 의 Value 값 % 로 설정.
        healthSlider.value = _currentHealth;
        healthCur.text = $"{_currentHealth}";
        healthMax.text = $"{maxHealth}";
    }
}
