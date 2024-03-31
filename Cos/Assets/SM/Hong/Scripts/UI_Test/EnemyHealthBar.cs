using KJ;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
