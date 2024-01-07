using Arc.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemHolder : MonoBehaviour
{
    public InventoryItemType inventoryItemType;
    public string inventoryItemName;
    public Image buttonImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI statsText;

    public void SetValues(string inventoryItemType, string inventoryItemName)
    {
        System.Enum.TryParse<InventoryItemType>(inventoryItemType, out this.inventoryItemType);
        this.inventoryItemName = inventoryItemName;
    }
    
    public void Used()
    {
        switch (inventoryItemType)
        {
            case InventoryItemType.Consumable:
                int quantity = InventoryManager.GetProperty<int>(inventoryItemType.ToString(), inventoryItemName, "Quantity");
                InventoryManager.SetProperty<int>(inventoryItemType.ToString(), inventoryItemName, "Quantity", quantity - 1);
                break;
            case InventoryItemType.Equipment:
                bool isUsed = InventoryManager.GetProperty<bool>(inventoryItemType.ToString(), inventoryItemName, "IsBeingUsed");
                InventoryManager.SetProperty<bool>(inventoryItemType.ToString(), inventoryItemName, "IsBeingUsed", !isUsed);
                break;
        }

        UIManager.Instance.RefreshInventory();
    }
}
