using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot_Inven : MonoBehaviour
{
    public string _itemidx = string.Empty;
    
    private void Start()
    {
        
        

    }
    public void OnButtonClick()
    {
        MenuUIManager mag  = MenuUIManager.Instance;
        InventoryUI invUI = mag.GetComponent<InventoryUI>();
        KJ.Item item = invUI._inventory.items.Find(item => item.id == _itemidx);

        Debug.Log("OnButtonClick: "+ item.imagePath);
        Debug.Log("OnButtonClick: " + item.type);

        Sprite s = ItemDBManager.Instance.LoadItemSprite(item.imagePath);
        switch (item.type)
        {
            case "weapon":
                mag._img_weapon.sprite = s;
                break;
            case "armor":
                mag._img_armor.sprite = s;
                break;
            case "accessory":
                mag._img_accessory.sprite = s;
                break;
        }
              
    }

}
