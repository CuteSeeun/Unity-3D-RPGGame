using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffOnOff : MonoBehaviour
{
    public Image powerBuffImage;
    public Image healthBuffImage;
    public Image specialBuffImage;

   
    public GameObject powerBuff;
    public GameObject healthBuff;
    public GameObject specialBuff;


    void Start()
    {
        powerBuffImage.color = Color.gray;
        healthBuffImage.color = Color.gray;
        specialBuffImage.color = Color.gray;
    }

    public void PowerBuff()
    {
        if (powerBuff != null)
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
        if (healthBuff != null)
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
        if (specialBuff != null)
        {
            specialBuffImage.color = new Color(1f, 1f, 0f, 1f);
        }
        else
        {
            specialBuffImage.color = Color.gray;
        }
    }
}
