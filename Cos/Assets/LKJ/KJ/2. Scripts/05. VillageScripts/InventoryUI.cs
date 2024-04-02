using KJ;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotPanel;
    public TMP_Text itemName;
    public TMP_Text itemDescription;
    public TMP_Text itemQuantity;

    private GameData _gameData;
    private ItemData _itemData 
    {
        get
        {
            return ItemDBManager.Instance._itemData;
        }
    }  

    public Item GetItem(string id)
    {
        return ItemDBManager.Instance.GetItem(id);
    }


    Player playerData { get 
        {
            string uid = PlayerDBManager.Instance.CurrentShortUID;
            return NetData.Instance.gameData.players[uid];
             
        }}
    
    /* 인벤토리에 아이템 추가 */
    public void AddItem(string itemId)
    {
        /*  근데 해당 아이템이 뭔지 알아야함 
            플레이어 인벤토리에 해당 아이템이 있는지 체크해야 함 
            이미 있는 아이템이면 수량만 +1, 없으면 플레이어 인벤토리에 아이템이 추가됨. 
            새로운 아이템(즉 없는 아이템을 얻을 경우 슬롯도 같이 생성) */
        
        Item itemToAdd = GetItItemById(itemId);
        
        if (itemToAdd != null )
        {
            Item item  = playerData.inventory.items.Find(item => item.id == itemId);
            
            if( item != null) 
            {
                item.quantity++;
                itemQuantity.text = item.quantity.ToString();
            }
            else
            {
                playerData.inventory.items.Add(itemToAdd);
                CreateSlot(itemToAdd);
            }
            
        }        
    }

    /* 인벤토리에 아이템 제거 */
    public void RemoveItem(string itemId)
    {
        Item itemToRemove = GetItItemById(itemId);

        if (itemToRemove != null)
        {
            Item item = playerData.inventory.items.Find(item => item.id == itemId);

            if (item != null )
            {
                item.quantity--;
                itemQuantity.text = item.quantity.ToString();

                if (itemToRemove.quantity <= 0)
                {
                    playerData.inventory.items.Remove(itemToRemove);
                    RemoveSlot(itemToRemove);
                }
            }
            else
            {
                Debug.Log("제거할 아이템 찾을 수 없음." + itemId);
            }
        }
    }

    /* 슬롯 생성 */
    private void CreateSlot(Item item)
    {
        GameObject slot = Instantiate(slotPrefab, slotPanel);
        slot.name = item.id;

        Image itemImageComponent = slot.GetComponentInChildren<Image>();

        Sprite itemImage = ItemDBManager.Instance.LoadItemSprite(item.imagePath);

        if (itemImageComponent != null)
        {
            itemImageComponent.sprite = itemImage;
        }
    }

    /* 슬롯 삭제 */
    private void RemoveSlot(Item item)
    {
        /* 제거할 아이템 id 찾기 
           아이템이 없는거 확인하면 해당 슬롯도 같이 삭제*/
        foreach (Transform slotTransform in slotPanel)
        {
            if (slotTransform.name == item.id)
            {
                Destroy(slotTransform.gameObject);
                break;
            }
        }
    }

    /* 아이템DB 에서 주어진 ID 를 가진 아이템 찾음. */
    public Item GetItItemById(string id)
    {
        Debug.Log($"ID = {id}");
        return _itemData.items.Find(item =>  item.id == id);
    }

    /* 슬롯 클릭 */
    public void ClickSlot(Item item)
    {
        itemName.text = item.id;
        itemDescription.text = item.description;      
    }
}
