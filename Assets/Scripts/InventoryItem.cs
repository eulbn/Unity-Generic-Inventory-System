using Arc.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public InventoryItemType inventoryItemType;
    public string inventoryItemName;
    public TextMeshProUGUI titleText;
    public Image itemImage;


    public void Collect()
    {
        int quantity = InventoryManager.GetProperty<int>(inventoryItemType.ToString(), inventoryItemName, "Quantity");
        Debug.Log(quantity);    
        
        InventoryManager.SetProperty<int>(inventoryItemType.ToString(), inventoryItemName, "Quantity", quantity + 1);

        UIManager.Instance.RefreshInventory();

        Destroy(gameObject);
    }
}


public enum InventoryItemType
{
    Consumable, Equipment, Quest
}