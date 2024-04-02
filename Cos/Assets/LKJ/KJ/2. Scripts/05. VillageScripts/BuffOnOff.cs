using KJ;
using UnityEngine;
using UnityEngine.UI;

public class BuffOnOff : MonoBehaviour
{
    //private GameData gameData;
    [Header("Buff Images")]
    public Image powerBuffImage;
    public Image healthBuffImage;
    public Image specialBuffImage;

    [Header("Bool Buffs")]
    [SerializeField] public bool _powerBuff = false;
    [SerializeField] public bool _healthBuff = false;
    [SerializeField] public bool _specialBuff = false;

    

    void Start()
    {
        //gameData = NetData.Instance._gameData;
        //Class playerData;
        powerBuffImage.color = Color.gray;
        healthBuffImage.color = Color.gray;
        specialBuffImage.color = Color.gray;
    }

    public void PowerBuff()
    {
        if (_powerBuff == true)
        {
            powerBuffImage.color = new Color(214 / 255f, 150 / 255f, 150 / 255f, 1f);
            
        }
        else
        {
            powerBuffImage.color = Color.gray;
        }
    }

    public void HealthBuff()
    {
        if (_healthBuff == true)
        {
            healthBuffImage.color = new Color(1f, 0f, 0f, 1f);
        }
        else
        {
            healthBuffImage.color = Color.gray;
        }
    }

    public void SpecialBuff()
    {
        if (_specialBuff == true)
        {
            specialBuffImage.color = new Color(1f, 1f, 0f, 1f);
        }
        else
        {
            specialBuffImage.color = Color.gray;
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
