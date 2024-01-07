using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arc.Inventory;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get { return instance; } }
    private static UIManager instance;

    public RectTransform inventoryContent;
    public BundledInventoryData bundledInventoryData;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        InventoryManager.Initiate();
    }

    private void Start()
    {
        RefreshInventory();
    }

    public void RefreshInventory()
    {
        InventoryManager.LoadInventory("Player Inventory");

        InventoryItemHolder[] inventoryItemHolders = inventoryContent.GetComponentsInChildren<InventoryItemHolder>();

        if(inventoryItemHolders != null)
        {
            foreach (var inventoryItemHolder in inventoryItemHolders)
            {
                Destroy(inventoryItemHolder.gameObject);
            }
        }

        bundledInventoryData = InventoryManager.GetBundledInventoryData();

        if(bundledInventoryData != null)
        {
            foreach (var bundledInventoryItem in bundledInventoryData.bundledInventoryItems)
            {
                if (bundledInventoryItem.bundledProperties.Count > 0
                    && bundledInventoryItem.bundledProperties.Find((x) => x.name == "Quantity") != null)
                {
                    if (bundledInventoryItem.bundledProperties.Find((x) => x.name == "Quantity").targetInt > 0)
                    {
                        if (bundledInventoryItem.bundledPersistentProperties.Count > 0
                                && bundledInventoryItem.bundledPersistentProperties.Find((x) => x.name == "ItemHolder") != null)
                        {
                            if (bundledInventoryItem.bundledPersistentProperties.Find((x) => x.name == "ItemHolder").targetGameObject != null)
                            {
                                GameObject temGameObject = Instantiate(bundledInventoryItem.bundledPersistentProperties.Find((x) => x.name == "ItemHolder").targetGameObject, inventoryContent);
                                InventoryItemHolder temInventoryItemHolder = temGameObject.GetComponent<InventoryItemHolder>();

                                temInventoryItemHolder.SetValues(bundledInventoryItem.inventoryItemType, bundledInventoryItem.inventoryItemName);

                                temInventoryItemHolder.titleText.text = bundledInventoryItem.inventoryItemType + "\n" + bundledInventoryItem.inventoryItemName;

                                temInventoryItemHolder.statsText.text = " Quantity:" + bundledInventoryItem.bundledProperties.Find((x) => x.name == "Quantity").targetInt.ToString();
                                bundledInventoryItem.bundledPersistentProperties.Find((x) => x.name == "ItemHolder").targetGameObject = temGameObject;

                                if (bundledInventoryItem.bundledProperties.Find((x) => x.name == "Value") != null)
                                {
                                    temInventoryItemHolder.statsText.text += "\n Value:" + bundledInventoryItem.bundledProperties.Find((x) => x.name == "Value").targetFloat;
                                }

                                if (bundledInventoryItem.bundledProperties.Find((x) => x.name == "IsBeingUsed") != null
                                    && bundledInventoryItem.bundledProperties.Find((x) => x.name == "IsBeingUsed").targetBool == true)
                                {
                                    temInventoryItemHolder.buttonImage.color = Color.red;
                                }
                            }
                        }
                    }
                }
            }
        }
        
    }


    public void Filter(TMP_InputField inputField)
    {

        string serach = inputField.text;
        foreach (var bundledInventoryItem in bundledInventoryData.bundledInventoryItems)
        {
            if(bundledInventoryItem.bundledPersistentProperties.Count > 0
                && bundledInventoryItem.bundledPersistentProperties.Find((x) => x.name == "ItemHolder") != null)
            {
                if (serach.Length < 4)
                {
                    bundledInventoryItem.bundledPersistentProperties.Find((x) => x.name == "ItemHolder").targetGameObject.SetActive(true);
                }
                else
                {
                    bundledInventoryItem.bundledPersistentProperties.Find((x) => x.name == "ItemHolder").targetGameObject.SetActive(false);
                }
            }
        }
        if (serach.Length < 4) return;
        List<BundledInventoryData.BundledInventoryItem> temBundledInventoryItems = bundledInventoryData.Filter(serach);


        foreach (var bundledInventoryItem in temBundledInventoryItems)
        {
            if (bundledInventoryItem.bundledPersistentProperties.Count > 0
                && bundledInventoryItem.bundledPersistentProperties.Find((x) => x.name == "ItemHolder") != null)
            {
                bundledInventoryItem.bundledPersistentProperties.Find((x) => x.name == "ItemHolder").targetGameObject.SetActive(true);
            }
        }
    }

    public void Sort()
    {
        InventoryItemHolder[] inventoryItemHolders = inventoryContent.GetComponentsInChildren<InventoryItemHolder>();
        List<InventoryItemHolder> sortedInventoryItemHolders = new List<InventoryItemHolder>();


        if (inventoryItemHolders != null)
        {
            int enumCounter = Enum.GetNames(typeof(InventoryItemType)).Length - 1;
            for (int i = 0; i < Enum.GetNames(typeof(InventoryItemType)).Length; i++)
            {
                for (int j = 0; j < inventoryItemHolders.Length; j++)
                {
                    if (inventoryItemHolders[j] != null && (int)inventoryItemHolders[j].inventoryItemType == enumCounter)
                    {
                        sortedInventoryItemHolders.Add(inventoryItemHolders[j]);
                        inventoryItemHolders[i] = null;
                    }
                }
                enumCounter--;
            }

            int index = 0;
            foreach (var sortedInventoryItemHolder in sortedInventoryItemHolders)
            {
                sortedInventoryItemHolder.transform.SetSiblingIndex(index);
                index++;
            }
        }
    }

}
