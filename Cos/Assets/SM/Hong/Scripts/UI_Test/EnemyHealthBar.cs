using KJ;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Enemy가 데미지를 입었을때 해당 Enemy의 이름, 최대체력, 현재체력을 표시할 수 있게 함.
/// Enemy 스크립트에서 참조받아 사용.
/// </summary>
public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text enemyName;
    public TMP_Text curHealth;
    public TMP_Text mHealth;


    void Start()
    {
        healthSlider.gameObject.SetActive(false);
    }

    // 체력 바 UI 업데이트 메서드
    public void UpdateHealth(float currentHealth, float maxHealth, string name)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
            curHealth.text = Mathf.RoundToInt(currentHealth).ToString(); ;
            healthSlider.maxValue = maxHealth;
            mHealth.text = $"{maxHealth}";
            enemyName.text = name;

            healthSlider.gameObject.SetActive(currentHealth > 0);
        }
    }
}
