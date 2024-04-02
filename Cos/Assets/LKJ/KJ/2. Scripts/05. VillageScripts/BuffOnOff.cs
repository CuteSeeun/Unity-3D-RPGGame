using System.Linq;
using HJ;
using KJ;
using UnityEngine;
using UnityEngine.UI;

public class BuffOnOff : MonoBehaviour
{
    [Header("Buff Images")]
    public Image powerBuffImage;
    public Image healthBuffImage;
    public Image specialBuffImage;

    [Header("Bool Buffs")]
    [SerializeField] public bool _powerBuff = false;
    [SerializeField] public bool _healthBuff = false;
    [SerializeField] public bool _specialBuff = false;

    PlayerController playerController;

    void Start()
    {
        powerBuffImage.color = Color.gray;
        healthBuffImage.color = Color.gray;
        specialBuffImage.color = Color.gray;
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void PowerBuff()
    {

        if (_powerBuff == true)
        {
            powerBuffImage.color = new Color(214 / 255f, 150 / 255f, 150 / 255f, 1f);
            playerController.attackFood = 0.2f;
        }
        else
        {
            powerBuffImage.color = Color.gray;
            playerController.attackFood = 0;
        }
    }

    public void HealthBuff()
    {
        if (_healthBuff == true)
        {
            healthBuffImage.color = new Color(1f, 0f, 0f, 1f);
            playerController.armorFood = 0.2f;
        }
        else
        {
            healthBuffImage.color = Color.gray;
            playerController.armorFood = 0;
        }
    }

    public void SpecialBuff()
    {
        if (_specialBuff == true)
        {
            specialBuffImage.color = new Color(1f, 1f, 0f, 1f);
            playerController.spRecoveryFood = 0.4f;
        }
        else
        {
            specialBuffImage.color = Color.gray;
            playerController.spRecoveryFood = 0;
        }
    }

    #region 버프 테스트
    /* 버프 일정 시간 지나면 꺼지게 설정할 예정. */
    public void PowerOn()
    {
        _powerBuff = true;
        PowerBuff();
    }

    public void HealthOn()
    {
        _healthBuff = true;
        HealthBuff();
    }

    public void SpecialOn()
    {
        _specialBuff = true;
        SpecialBuff();
    }
    #endregion
}
