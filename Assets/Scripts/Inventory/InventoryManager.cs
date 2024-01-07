using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


namespace Arc.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        private static InventoryManager Instance;

        private static bool hasInitiated = false;
        [SerializeField]
        private List<InventoryScriptable> inventoryScriptables;
        [SerializeField]
        private InventoryScriptable loadedInventoryScriptable = null;
        public static void Initiate()
        {
            if (Instance == null)
            {
                GameObject temGameObject = new GameObject("InventoryManager");
                DontDestroyOnLoad(temGameObject);
                Instance = temGameObject.AddComponent<InventoryManager>();
                Instance.inventoryScriptables = new List<InventoryScriptable>();

                string[] guids = AssetDatabase.FindAssets("t:InventoryScriptable", new[] { InventoryConstants.InventoryPath });

                foreach (string guid in guids)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    InventoryScriptable inventoryScriptable = AssetDatabase.LoadAssetAtPath<InventoryScriptable>(assetPath);

                    if (inventoryScriptable != null)
                    {
                        inventoryScriptable.Setup();
                        Instance.inventoryScriptables.Add(inventoryScriptable);
                    }
                }
                hasInitiated = true;
            }
        }

        /// <summary>
        /// Load an inventory by name in the Inventory Manager
        /// </summary>
        /// <returns> returns if loaded </returns>
        public static bool LoadInventory(string name)
        {
            if (hasInitiated == false) { Initiate(); }

            InventoryScriptable temInventoryScriptable = Instance.inventoryScriptables.Find((x) => x.name == name);

            if (temInventoryScriptable != null)
            {
                Instance.loadedInventoryScriptable = temInventoryScriptable;
                return true;
            }

            return false;
        }


        /// <summary>
        /// Returns the Persistent Property of Inventory Item if the Property with conditions exist
        /// </summary>
        public static T GetPersistentProperty<T>(string inventoryItemType, string inventoryItemName, string persistentPropertyName)
        {
            if (hasInitiated == false) { Initiate(); }
        
            if(Instance.loadedInventoryScriptable != null)
            {
                return Instance.loadedInventoryScriptable.GetPersistentProperty<T>(inventoryItemType, inventoryItemName, persistentPropertyName);
            }
            else
            {
                Debug.LogWarning("Inventory has not been loaded");
            }
            return default;
        }

        public static List<T> GetAllPersistentPropertiesOfItemType<T>(string inventoryItemType, string persistentPropertyName)
        {
            if (hasInitiated == false) { Initiate(); }

            if (Instance.loadedInventoryScriptable != null)
            {
                return Instance.loadedInventoryScriptable.GetAllPersistentPropertiesOfItemType<T>(inventoryItemType, persistentPropertyName);
            }
            else
            {
                Debug.LogWarning("Inventory has not been loaded");
            }
            return default;
        }


        /// <summary>
        /// Returns the Property of Inventory Item if the Property with conditions exist
        /// </summary>
        public static T GetProperty<T>(string inventoryItemType, string inventoryItemName, string propertyName)
        {
            if (hasInitiated == false) { Initiate(); }

            if (Instance.loadedInventoryScriptable != null)
            {
                return Instance.loadedInventoryScriptable.GetProperty<T>(inventoryItemType, inventoryItemName, propertyName);
            }
            else
            {
                Debug.LogWarning("Inventory has not been loaded");
            }
            return default;
        }


        /// <summary>
        /// Sets the property value of Inventory Item and saves the data
        /// </summary>
        /// <returns> returns if it was able to set the property</returns>
        public static bool SetProperty<T>(string inventoryItemType, string inventoryItemName, string propertyName, T value)
        {
            if (hasInitiated == false) { Initiate(); }

            if(Instance.loadedInventoryScriptable != null)
            {
                return Instance.loadedInventoryScriptable.SetProperty(inventoryItemType, inventoryItemName, propertyName, value);
            }
            else
            {
                Debug.LogWarning("Inventory has not been loaded");
            }
            return false;
        }


        public static BundledInventoryData GetBundledInventoryData()
        {
            if (hasInitiated == false) { Initiate(); }

            if (Instance.loadedInventoryScriptable != null)
            {
                return Instance.loadedInventoryScriptable.GetBundledInventoryData();
            }
            else
            {
                Debug.LogWarning("Inventory has not been loaded");
                return null;
            }
        }
    }
}
