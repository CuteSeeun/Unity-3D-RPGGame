using KJ;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using Item = KJ.Item;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotPanel;
    public TMP_Text itemName;
    public TMP_Text itemDescription;
    public TMP_Text itemQuantity;

    public string uid = PlayerDBManager.Instance.CurrentShortUID;


    private GameData _gameData = NetData.Instance.gameData;
    private Class _class
    {
        get
        {
            return _gameData.classes[ClassType.knight];
        }
    }

    public Inventory _inventory
    {
        get
        {
            return _class.inventory;
        }
    }
    
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



    void Start()
    {
        AddItem("12");
        AddItem("24");
    }

    public void UpdateInventoryUI()
    {
        foreach (var item in _inventory.items)
        {
            AddItem(item.id);
        }
    }

    /* 인벤토리에 아이템 추가 */
    public void AddItem(string itemId)
    {
        Debug.Log($"{itemId} 추가");
        /*  근데 해당 아이템이 뭔지 알아야함 
            플레이어 인벤토리에 해당 아이템이 있는지 체크해야 함 
            이미 있는 아이템이면 수량만 +1, 없으면 플레이어 인벤토리에 아이템이 추가됨. 
            새로운 아이템(즉 없는 아이템을 얻을 경우 슬롯도 같이 생성) */
        
        Item itemToAdd = GetItItemById(itemId);
        
        if (itemToAdd != null )
        {
            Item item = _inventory.items.Find(item => item.id == itemId);

            if (item != null && item.id == itemId) 
            {
                Debug.Log($"제발 :{item.id}");
                Debug.Log($"제발2 :{item.imagePath}");

                item.quantity++;
                itemQuantity.text = item.quantity.ToString();
                CreateSlot(item);
            }
            else
            {
                
                _inventory.items.Add(itemToAdd);
                
                Debug.Log($"이미지 : {item.id}");
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
            Item item = _inventory.items.Find(item => item.id == itemId);

            if (item != null )
            {
                item.quantity--;
                itemQuantity.text = item.quantity.ToString();

                if (itemToRemove.quantity <= 0)
                {
                    _inventory.items.Remove(itemToRemove);
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
        Debug.Log("슬롯추가");
        slot.name = item.id;
        

        Image itemImageComponent = slot.GetComponentInChildren<Image>();

        Sprite itemImage = ItemDBManager.Instance.LoadItemSprite(item.imagePath);

        if( itemImage == null) Debug.Log("itemImage == null : "+ item.imagePath);
        if (itemImageComponent != null)
        {
            Debug.Log("이미지 추가");
            itemImageComponent.sprite = itemImage;
            itemImageComponent.enabled = true;
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
        
        /* 특정 타입(Weapon, Armor, Acc)을 클릭할 때 장비창에 해당 이미지 전달.*/
    }
}
