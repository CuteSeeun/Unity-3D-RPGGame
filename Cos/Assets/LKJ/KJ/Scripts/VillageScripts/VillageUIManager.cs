using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageUIManager : MonoBehaviour
{
    /* ¸Þ´º UI */
    public GameObject menuUI;
    /* ÆË¾÷Ã¢ UI */
    public GameObject weaponUI;
    public GameObject armorUI;
    public GameObject accUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuUI.SetActive(true);

            Cursor.visible = true;
        }
    }
    /* UIÃ¢ ´Ý±â */
    public void CloseMenuUI()
    {
        menuUI.SetActive(false);

        Cursor.visible = false;
    }
    #region
    /* ÆË¾÷Ã¢ ´Ý±â */
    // ¹«±â UI Ã¢
    public void OpenWeaponUI()
    {
        weaponUI.SetActive(true);
    }

    public void CloseWeaponUI()
    {
        weaponUI.SetActive(false);
    }
    // ¹æ¾î±¸ UI Ã¢
    public void OpenArmorUI()
    {
        armorUI.SetActive(true);
    }

    public void CloseArmorUI()
    {
        armorUI.SetActive(false);
    }
    // ¾Ç¼¼¼­¸® UI Ã¢
    public void OpenAccUI()
    {
        accUI.SetActive(true);
    }

    public void CloseAccUI()
    {
        accUI.SetActive(false);
    }
    #endregion
}
