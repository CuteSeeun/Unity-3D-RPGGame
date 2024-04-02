using KJ;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotPanel;
    public TMP_Text itemName;
    public TMP_Text itemDescription;
    public TMP_Text itemQuantity;

    private ItemData _itemData;
    private GameData _gameData;

    public void AddItem(Item item)
    {
        

        var slotInstance = Instantiate(slotPrefab, slotPanel);
    }
}
