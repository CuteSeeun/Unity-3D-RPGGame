using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBarsManager : MonoBehaviour
{
    /* 보스 체력 */
    [Header("Boss")]
    public Slider bossHpSlider;
    /* 적 체력 */
    [Header("Enemy")]
    public Slider enemyHpSlider;
    /* 적 및 보스 체력 표시 */
    [Header("Cur / Max")]
    /* 보스 */
    public TMP_Text bossHpCur;
    public TMP_Text bossHpMax;
    /* 적 */
    public TMP_Text enemyHpCur;
    public TMP_Text enemyHpMax;
    /* 최대 체력 */
    //[Header("MaxHp")]
    public float maxBossHp = 500f;
    public float maxEnemyHp = 80f;

    /* 현재 보스 체력 */
    private float _currentBossHp;
    /* 현재 적 체력 */
    private float _currentEnemyHp;

    void Start()
    {
        /* 초기 체력 설정. */
        _currentBossHp = maxBossHp;
        _currentEnemyHp = maxEnemyHp;
        /* 슬라이더 최대값 최대 체력으로 설정. */
        bossHpSlider.maxValue = maxBossHp;
        enemyHpSlider.maxValue = maxEnemyHp;
        UpdateTargetUI();
    }

    
    void Update()
    {
        UpdateTargetUI();
    }

    /* 체력바 업데이트 */
    void UpdateTargetUI()
    {
        /* Slider, 텍스트 업데이트 */
        bossHpSlider.value = _currentBossHp;
        bossHpCur.text = $"{_currentBossHp}";
        bossHpMax.text = $"{maxBossHp}";

        enemyHpSlider.value = _currentEnemyHp;
        enemyHpCur.text = $"{_currentEnemyHp}";
        enemyHpMax.text = $"{maxEnemyHp}";
    }
}
