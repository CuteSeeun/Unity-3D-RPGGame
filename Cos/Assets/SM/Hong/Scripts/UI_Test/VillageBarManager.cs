using System.Collections;
using System.Collections.Generic;
using HJ;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CharacterController = HJ.CharacterController;

public class VillageBarManager : MonoBehaviour
{
    CharacterController playerHp;
    PlayerController playerSp;

    [Header("Slider")]
    public Slider healthSlider;
    public Slider staminaSlider;

    public TMP_Text healthCur;
    public TMP_Text staminaCur;


    void Start()
    {
        playerHp = FindAnyObjectByType<CharacterController>();
        playerSp = FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        UpdateHealthUI();
        UpdateStaminaUI();
        }

    void UpdateHealthUI()
    {
        /* Slider, 텍스트 업데이트. */
        healthSlider.value = playerHp.hp;
        healthCur.text = $"{playerHp.hp}";
    }

    // 스테미나 업데이트
    void UpdateStaminaUI()
    {
        /* 슬라이더 업데이트 */
        staminaSlider.value = playerSp.stamina;
        /* 텍스트 업데이트 */
        /* 정수로 변환하여 소수점 안나오게 함. */
        staminaCur.text = Mathf.RoundToInt(playerSp.stamina).ToString();        
    }
}
