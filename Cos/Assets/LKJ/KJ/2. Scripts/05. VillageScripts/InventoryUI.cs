using KJ;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Item = KJ.Item;

public class InventoryUI : MonoBehaviour
{
    [Header("Slot")]
    public GameObject slotPrefab;
    public Transform slotPanel;
    [Header("Item Infomation")]
    public TMP_Text itemName;
    public TMP_Text itemDescription;
    public TMP_Text itemQuantity;

    public string uid = PlayerDBManager.Instance.CurrentShortUID;

    PlayerDBManager playerDBManager = PlayerDBManager.Instance;



    private GameData _gameData = NetData.Instance.gameData;

    private Class _class
    {
        get
        {
            Debug.Log("type: " + playerDBManager.LoadGameData(playerDBManager.CurrentShortUID).classType);

            switch (playerDBManager.LoadGameData(playerDBManager.CurrentShortUID).classType)
            {
                case "Knight":
                    Debug.Log("기사");
                    return _gameData.classes[ClassType.knight];

                case "Barbarian":
                    Debug.Log("바바리안");
                    return _gameData.classes[ClassType.barbarian];

                case "Rogue":
                    Debug.Log("로그");
                    return _gameData.classes[ClassType.rogue];

                case "Mage":
                    Debug.Log("메이지");
                    return _gameData.classes[ClassType.mage];

            }
            Debug.Log("안불러와짐");
            return null;
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
        _inventory.items.Clear();

        foreach (var c in _gameData.classes.Values)
        {
            Debug.Log("start " + c.classType.ToString());
        }
        if (_class.classType == ClassType.knight)
        {
            Debug.Log("기사2");
            AddItem("12");
            AddItem("24");
        }
        else if (_class.classType == ClassType.barbarian)
        {
            Debug.Log("바바리안2");
            AddItem("15");
            AddItem("24");
        }
        else if (_class.classType == ClassType.rogue)
        {
            Debug.Log("로그2");
            AddItem("18");
            AddItem("24");
        }
        else
        {
            Debug.Log("메이지2");
            AddItem("21");
            AddItem("24");
        }
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

        if (itemToAdd != null)
        {
            Item item = null;
            foreach (var i in _inventory.items)
            {
                if(i.id == itemId) 
                {
                    item = i;
                    break;
                }
            }
            //Item item = _inventory.items.Find(item => item.id == itemId);

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

                Debug.Log($"이미지 : {itemToAdd.id}");
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

            if (item != null)
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
        ItemSlot_Inven sSlot = slot.GetComponent<ItemSlot_Inven>();
        sSlot._itemidx = item.id;


        Image[] itemImageComponent = slot.GetComponentsInChildren<Image>();

        Sprite itemImage = ItemDBManager.Instance.LoadItemSprite(item.imagePath);

        if (itemImage == null) Debug.Log("itemImage == null : " + item.imagePath);
        if (itemImageComponent != null)
        {
            Debug.Log("이미지 추가");
            itemImageComponent[1].sprite = itemImage;
            itemImageComponent[1].enabled = true;
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
        return _itemData.items.Find(item => item.id == id);
    }

    /* 슬롯 클릭 */
    public void ClickDescription(Item item)
    {
        itemName.text = item.id;
        itemDescription.text = item.description;
        //bool _type = item.type

        switch (item.type)
        {
            case "weapon":
                
                break;
            case "armor": break;
            case "accessory": break;

        }
        /* 특정 타입(Weapon, Armor, Acc)을 클릭할 때 장비창에 해당 이미지 전달.*/
    }

    public void ClickSlot()
    {
        //ClickDescription();
    }
}
