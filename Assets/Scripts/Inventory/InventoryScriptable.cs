using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace Arc.Inventory
{
    public class InventoryScriptable : ScriptableObject
    {
        public string inventroyName;

        [SerializeField]
        private InventoryData inventoryData;

        [System.Serializable]
        public class InventoryData
        {
            public List<InventoryItem> inventoryItems;

            [System.Serializable]
            public class InventoryItem
            {
                public string inventoryItemType;
                public string inventoryItemName;
                public List<PersistentProperty> persistentProperties;
                public List<Property> properties;


                [System.Serializable]
                public class PersistentProperty
                {
                    public string name;
                    public PersistentPropertyType persistentPropertyType;
                    [XmlIgnore]
                    public UnityEngine.Object target;
                }

                [System.Serializable]
                public class Property
                {
                    public string name;
                    public PropertyType propertyType;
                    public string defaultTarget;
                    public string target;
                }
            }
        }

        //---------------------------------Getters/Setters-----------------------------------
        public T GetPersistentProperty<T>(string inventoryItemType, string inventoryItemName, string persistentPropertyName)
        {
            InventoryData.InventoryItem temItem = inventoryData.inventoryItems.Find((x) => x.inventoryItemType == inventoryItemType && x.inventoryItemName == inventoryItemName);
            if (temItem != null && temItem.persistentProperties != null)
            {
                InventoryData.InventoryItem.PersistentProperty temPersistentProperty = temItem.persistentProperties.Find((x) => x.name == persistentPropertyName);
                if (temPersistentProperty != null)
                {
                    Type type = typeof(T);
                    if (type == typeof(GameObject) && temPersistentProperty.persistentPropertyType == PersistentPropertyType.GameObject)
                    {
                        return (T)(object)temPersistentProperty.target;
                    }
                    else if (type == typeof(Sprite) && temPersistentProperty.persistentPropertyType == PersistentPropertyType.Sprite)
                    {
                        return (T)(object)temPersistentProperty.target;
                    }
                }

            }

            return default(T);
        }


        public List<T> GetAllPersistentPropertiesOfItemType<T>(string inventoryItemType, string persistentPropertyName)
        {
            List<InventoryData.InventoryItem> temItems = inventoryData.inventoryItems.FindAll((x) => x.inventoryItemType == inventoryItemType);
            List<T> genricList = new List<T>();
            foreach (var item in temItems)
            {
                if (item.persistentProperties != null)
                {
                    InventoryData.InventoryItem.PersistentProperty temPersistentProperty = item.persistentProperties.Find((x) => x.name == persistentPropertyName);
                    if (temPersistentProperty != null && temPersistentProperty.target != null)
                    {
                        Type type = typeof(T);
                        if (type == typeof(GameObject) && temPersistentProperty.persistentPropertyType == PersistentPropertyType.GameObject)
                        {
                            genricList.Add((T)(object)temPersistentProperty.target);
                        }
                        else if (type == typeof(Sprite) && temPersistentProperty.persistentPropertyType == PersistentPropertyType.Sprite)
                        {
                            genricList.Add((T)(object)temPersistentProperty.target);
                        }
                    }

                }
            }

            if(genricList.Count > 0)
            {
                return genricList;
            }
            else
            {
                return default;
            }
        }



        public T GetProperty<T>(string inventoryItemType, string inventoryItemName, string propertyName)
        {
            InventoryData.InventoryItem temItem = inventoryData.inventoryItems.Find((x) => x.inventoryItemType == inventoryItemType && x.inventoryItemName == inventoryItemName);
            if (temItem != null && temItem.properties != null)
            {
                InventoryData.InventoryItem.Property temProperty = temItem.properties.Find((x) => x.name == propertyName);
                if (temProperty != null)
                {
                    Type type = typeof(T);
                    if (type == typeof(int) && temProperty.propertyType == PropertyType.Int)
                    {
                        object intProperty = int.Parse(temProperty.target);
                        return (T)intProperty;
                    }
                    else if (type == typeof(float) && temProperty.propertyType == PropertyType.Float)
                    {
                        object floatProperty = float.Parse(temProperty.target);
                        return (T)floatProperty;
                    }
                    else if (type == typeof(string) && temProperty.propertyType == PropertyType.String)
                    {
                        object stringProperty = temProperty.target;
                        return (T)stringProperty;
                    }
                    else if (type == typeof(bool) && temProperty.propertyType == PropertyType.Bool)
                    {
                        object boolProperty = bool.Parse(temProperty.target);
                        return (T)boolProperty;
                    }
                }

            }

            return default(T);
        }


        public List<T> GetPropertiesOfItemType<T>(string inventoryItemType, string propertyName)
        {
            List<InventoryData.InventoryItem> temItems = inventoryData.inventoryItems.FindAll((x) => x.inventoryItemType == inventoryItemType);
            List<T> genricList = new List<T>();
            foreach (var item in temItems)
            {
                if (item.properties != null)
                {
                    InventoryData.InventoryItem.Property temProperty = item.properties.Find((x) => x.name == propertyName);
                    if (temProperty != null)
                    {
                        Type type = typeof(T);
                        if (type == typeof(int) && temProperty.propertyType == PropertyType.Int)
                        {
                            genricList.Add((T)(object)temProperty.target);
                        }
                    }

                }
            }

            if (genricList.Count > 0)
            {
                return genricList;
            }
            else
            {
                return default;
            }
        }

        public bool SetProperty<T>(string inventoryItemType, string inventoryItemName, string propertyName, T value)
        {
            InventoryData.InventoryItem temItem = inventoryData.inventoryItems.Find((x) => x.inventoryItemType == inventoryItemType && x.inventoryItemName == inventoryItemName);
            if (temItem != null && temItem.properties != null)
            {
                InventoryData.InventoryItem.Property temProperty = temItem.properties.Find((x) => x.name == propertyName);
                if (temProperty != null)
                {
                    Type type = typeof(T);
                    if (type == typeof(int) && temProperty.propertyType == PropertyType.Int)
                    {
                        string property = (value).ToString();
                        temProperty.target = property;
                        SaveData();
                        return true;
                    }
                    else if (type == typeof(float) && temProperty.propertyType == PropertyType.Float)
                    {
                        string property = value.ToString();
                        temProperty.target = property;
                        SaveData();
                        return true;
                    }
                    else if (type == typeof(string) && temProperty.propertyType == PropertyType.String)
                    {
                        string property = value.ToString();
                        temProperty.target = property;
                        SaveData();
                        return true;
                    }
                    else if (type == typeof(bool) && temProperty.propertyType == PropertyType.Bool)
                    {
                        string property = value.ToString();
                        temProperty.target = property;
                        SaveData();
                        return true;
                    }
                }

            }

            return false;
        }
    
        public BundledInventoryData GetBundledInventoryData()
        {
            BundledInventoryData temBundledInventoryData = new BundledInventoryData();
            temBundledInventoryData.bundledInventoryItems = new List<BundledInventoryData.BundledInventoryItem>();
            foreach (var inventoryItem in inventoryData.inventoryItems)
            {
                BundledInventoryData.BundledInventoryItem temBundledInventoryItem = new BundledInventoryData.BundledInventoryItem();

                temBundledInventoryItem.inventoryItemName = inventoryItem.inventoryItemName;
                temBundledInventoryItem.inventoryItemType = inventoryItem.inventoryItemType;
                temBundledInventoryItem.bundledPersistentProperties = new List<BundledInventoryData.BundledInventoryItem.BundledPersistentProperty>();
                foreach (var persistentProperty in inventoryItem.persistentProperties)
                {
                    BundledInventoryData.BundledInventoryItem.BundledPersistentProperty temBundledPersistentProperty = new BundledInventoryData.BundledInventoryItem.BundledPersistentProperty();
                    temBundledPersistentProperty.name = persistentProperty.name;
                    if(persistentProperty.persistentPropertyType == PersistentPropertyType.GameObject)
                    {
                        temBundledPersistentProperty.targetGameObject = (GameObject)persistentProperty.target;
                    }
                    if (persistentProperty.persistentPropertyType == PersistentPropertyType.Sprite)
                    {
                        temBundledPersistentProperty.targetGameObject = (GameObject)persistentProperty.target;
                    }
                    temBundledInventoryItem.bundledPersistentProperties.Add(temBundledPersistentProperty);
                }

                temBundledInventoryItem.bundledProperties = new List<BundledInventoryData.BundledInventoryItem.BundledProperty>();
                foreach (var property in inventoryItem.properties)
                {
                    BundledInventoryData.BundledInventoryItem.BundledProperty temBundledProperty = new BundledInventoryData.BundledInventoryItem.BundledProperty();
                    temBundledProperty.name = property.name;
                    if (property.propertyType == PropertyType.Int)
                    {
                        temBundledProperty.targetInt = int.Parse(property.target);
                    }
                    else if (property.propertyType == PropertyType.Float)
                    {
                        temBundledProperty.targetFloat = float.Parse(property.target);
                    }
                    else if (property.propertyType == PropertyType.Float)
                    {
                        temBundledProperty.targetString = property.target;
                    }
                    else if (property.propertyType == PropertyType.Bool)
                    {
                        temBundledProperty.targetBool = bool.Parse(property.target);
                    }
                    temBundledInventoryItem.bundledProperties.Add(temBundledProperty);
                }

                temBundledInventoryData.bundledInventoryItems.Add(temBundledInventoryItem);
            }

            return temBundledInventoryData;
        }


        public void Setup()
        {
            if (File.Exists(Application.persistentDataPath + "/" + inventroyName + ".xml") == false)
            {
                SetDefaultTargets();
            }
            else
            {
                LoadData();
            }

            SaveData();
        }

        public void SaveData()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(InventoryData));
            try
            {
                Debug.Log("Path " + Application.persistentDataPath + "/" + inventroyName + ".xml");
                using (StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + "/" + inventroyName + ".xml"))
                {
                    serializer.Serialize(streamWriter, inventoryData);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error at saving data data: {e.Message}");
            }
        }

        private void SetDefaultTargets()
        {
            foreach (var inventoryItem in inventoryData.inventoryItems)
            {
                foreach (var property in inventoryItem.properties)
                {
                    property.target = property.defaultTarget;
                }
            }
        }

        private void LoadData()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(InventoryData));
            try
            {
                using (FileStream stream = new FileStream(Application.persistentDataPath + "/" + inventroyName + ".xml", FileMode.Open))
                {
                    InventoryData temInventoryData = (InventoryData)serializer.Deserialize(stream);

                    for (int i = 0; i < temInventoryData.inventoryItems.Count && i < inventoryData.inventoryItems.Count; i++)
                    {
                        inventoryData.inventoryItems[i].inventoryItemName = temInventoryData.inventoryItems[i].inventoryItemName;
                        inventoryData.inventoryItems[i].inventoryItemType = temInventoryData.inventoryItems[i].inventoryItemType;

                        if (temInventoryData.inventoryItems[i].persistentProperties != null
                            && inventoryData.inventoryItems[i].persistentProperties != null)
                        {
                            for (int j = 0; j < temInventoryData.inventoryItems[i].persistentProperties.Count
                                && j < inventoryData.inventoryItems[i].persistentProperties.Count; j++)
                            {
                                inventoryData.inventoryItems[i].persistentProperties[j].name = temInventoryData.inventoryItems[i].persistentProperties[j].name;
                                inventoryData.inventoryItems[i].persistentProperties[j].persistentPropertyType = temInventoryData.inventoryItems[i].persistentProperties[j].persistentPropertyType;
                            }
                        }

                        if (temInventoryData.inventoryItems[i].properties != null
                            && inventoryData.inventoryItems[i].properties != null)
                        {
                            for (int j = 0; j < temInventoryData.inventoryItems[i].properties.Count
                                && j < inventoryData.inventoryItems[i].properties.Count; j++)
                            {
                                inventoryData.inventoryItems[i].properties[j].propertyType = temInventoryData.inventoryItems[i].properties[j].propertyType;
                                inventoryData.inventoryItems[i].properties[j].name = temInventoryData.inventoryItems[i].properties[j].name;
                                inventoryData.inventoryItems[i].properties[j].target = temInventoryData.inventoryItems[i].properties[j].target;
                                inventoryData.inventoryItems[i].properties[j].defaultTarget = temInventoryData.inventoryItems[i].properties[j].defaultTarget;
                            }
                        }
                    }

                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error loading data: {e.Message}");
            }
        }
        //--------------------------------------------------------------------------------------------


    #if UNITY_EDITOR
        [CustomEditor(typeof(InventoryScriptable))]
        public class InventoryScriptableEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                var script = (InventoryScriptable)target;
                GUI.enabled = false;
                base.OnInspectorGUI();
            }
        }
    #endif

    #if UNITY_EDITOR
        public class InventoryWindow : EditorWindow
        {
            private static string inventroyName = "";
            private static string temInventoryName = "";
            private static InventoryScriptable temInventoryScriptable;
            private static GUIStyle blackStyle = null;
            private static InventoryData.InventoryItem removableInventoryItem = null;
            private static InventoryData.InventoryItem.PersistentProperty removablePersistentProperty = null;
            private static InventoryData.InventoryItem.Property removableProperty = null;
            private static Vector2 scrollPosition = Vector2.zero;


            [MenuItem("Inventory/InventoryWindow")]
            public static void ShowWindow()
            {
                InventoryWindow window = GetWindow<InventoryWindow>();
                window.titleContent = new GUIContent("Inventory");

                if (!AssetDatabase.IsValidFolder(InventoryConstants.InventoryPath))
                {
                    AssetDatabase.CreateFolder("Assets", "Inventory");
                }
            }

            void OnGUI()
            {
                if (blackStyle == null)
                {
                    Texture2D blackTexture = MakeTexture(2, 2, Color.black);

                    blackStyle = new GUIStyle(GUI.skin.box);
                    blackStyle.normal.background = blackTexture;
                }

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                // ----------------- Inventory filling GUI----------------------------

                EditorGUILayout.Space();
                temInventoryName = EditorGUILayout.TextField("Inventory Name", temInventoryName);

                if (temInventoryScriptable != null && inventroyName == temInventoryName)
                {
                    temInventoryScriptable.inventroyName = inventroyName;

                    EditorGUILayout.BeginVertical();
                    int itemCount = 0;

                    foreach (var inventoryItem in temInventoryScriptable.inventoryData.inventoryItems)
                    {
                        EditorGUILayout.BeginVertical(blackStyle);

                        EditorGUILayout.BeginHorizontal();
                        inventoryItem.inventoryItemType = EditorGUILayout.TextField("Item Type", inventoryItem.inventoryItemType);
                        inventoryItem.inventoryItemName = EditorGUILayout.TextField("Item Name", inventoryItem.inventoryItemName);
                        if (GUILayout.Button("-", GUILayout.MaxWidth(25))) // Remoe Inventory Item
                        {
                            removableInventoryItem = inventoryItem;
                        }
                        EditorGUILayout.EndHorizontal();




                        int persistentPropertyCounter = 0;
                        foreach (var persistentProperty in inventoryItem.persistentProperties)
                        {
                            EditorGUILayout.BeginVertical(blackStyle);
                            persistentProperty.name = EditorGUILayout.TextField("Name", persistentProperty.name);
                            if (persistentProperty.persistentPropertyType == PersistentPropertyType.GameObject)
                            {
                                persistentProperty.target = EditorGUILayout.ObjectField("Property", persistentProperty.target, typeof(GameObject), false);
                            }
                            else if (persistentProperty.persistentPropertyType == PersistentPropertyType.Sprite)
                            {
                                persistentProperty.target = EditorGUILayout.ObjectField("Property", persistentProperty.target, typeof(Sprite), false);
                            }
                            persistentProperty.persistentPropertyType = (PersistentPropertyType)EditorGUILayout.EnumPopup("Type", persistentProperty.persistentPropertyType);


                            if (GUILayout.Button("-", GUILayout.MaxWidth(25))) // Remove the Persistent Property
                            {
                                removablePersistentProperty = persistentProperty;
                            }


                            EditorGUILayout.EndVertical();
                            persistentPropertyCounter++;
                        }

                        if (removablePersistentProperty != null)
                        {
                            inventoryItem.persistentProperties.Remove(removablePersistentProperty);
                            removablePersistentProperty = null;
                        }

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Add Persistent Property", GUILayout.MinWidth(200)))
                        {
                            InventoryData.InventoryItem.PersistentProperty temPersistentProperty = new InventoryData.InventoryItem.PersistentProperty();
                            temPersistentProperty.name = "Persistent Property " + persistentPropertyCounter;
                            temPersistentProperty.persistentPropertyType = PersistentPropertyType.GameObject;
                            temPersistentProperty.target = null;

                            inventoryItem.persistentProperties.Add(temPersistentProperty);
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();



                        EditorGUILayout.Space();
                        int PropertyCounter = 0;
                        foreach (var property in inventoryItem.properties)
                        {
                            EditorGUILayout.BeginVertical(blackStyle);
                            property.name = EditorGUILayout.TextField("Name", property.name);
                            if (property.propertyType == PropertyType.String)
                            {
                                property.target = EditorGUILayout.TextField("Property", property.target);
                                property.defaultTarget = property.target;
                            }
                            else if (property.propertyType == PropertyType.Int)
                            {
                                property.target = EditorGUILayout.IntField("Property", int.Parse(property.target)).ToString();
                                property.defaultTarget = property.target;
                            }
                            else if (property.propertyType == PropertyType.Float)
                            {
                                property.target = EditorGUILayout.FloatField("Property", float.Parse(property.target)).ToString();
                                property.defaultTarget = property.target;
                            }
                            else if (property.propertyType == PropertyType.Bool)
                            {
                                property.target = EditorGUILayout.Toggle("Property", bool.Parse(property.target)).ToString();
                                property.defaultTarget = property.target;
                            }

                            PropertyType temPropertyType = (PropertyType)EditorGUILayout.EnumPopup("Type", property.propertyType);
                            if (temPropertyType != property.propertyType)
                            {

                                switch (temPropertyType)
                                {
                                    case PropertyType.Int:
                                        property.target = "0";
                                        break;
                                    case PropertyType.Float:
                                        property.target = "0";
                                        break;
                                    case PropertyType.String:
                                        property.target = "";
                                        break;
                                    case PropertyType.Bool:
                                        property.target = "False";
                                        break;
                                    default:
                                        break;
                                }

                                property.propertyType = temPropertyType;
                            }

                            if (GUILayout.Button("-", GUILayout.MaxWidth(25))) // Remove The Property
                            {
                                removableProperty = property;
                            }


                            EditorGUILayout.EndVertical();
                            PropertyCounter++;
                        }

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Add Property", GUILayout.MinWidth(200)))
                        {
                            InventoryData.InventoryItem.Property temProperty = new InventoryData.InventoryItem.Property();
                            temProperty.name = "Property " + PropertyCounter;
                            temProperty.propertyType = PropertyType.String;
                            temProperty.target = "";
                            inventoryItem.properties.Add(temProperty);
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();
                        if (removableProperty != null)
                        {
                            inventoryItem.properties.Remove(removableProperty);
                            removableProperty = null;
                        }


                        EditorGUILayout.EndVertical();
                        itemCount++;
                    }

                    EditorGUILayout.Space();

                    if (GUILayout.Button("Add Inventroy Item"))
                    {

                        InventoryData.InventoryItem temInventoryItem = new InventoryData.InventoryItem();
                        temInventoryItem.inventoryItemType = "Item Type" + itemCount;
                        temInventoryItem.inventoryItemName = "Item Name" + itemCount;
                        temInventoryItem.persistentProperties = new List<InventoryData.InventoryItem.PersistentProperty>();
                        temInventoryItem.properties = new List<InventoryData.InventoryItem.Property>();

                        temInventoryScriptable.inventoryData.inventoryItems.Add(temInventoryItem);

                        AssetDatabase.Refresh();
                    }
                    EditorGUILayout.EndVertical();

                    if (removableInventoryItem != null)
                    {
                        temInventoryScriptable.inventoryData.inventoryItems.Remove(removableInventoryItem);
                        removableInventoryItem = null;
                    }

                    Undo.RecordObject(temInventoryScriptable, "Inventory Scriptable");
                    EditorUtility.SetDirty(temInventoryScriptable);
                }



                //---------------------------------------------------------------------------------


                //-------------------------- Create or Load New Scriptble Object------------------------
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                if (inventroyName != temInventoryName)
                {
                    if (GUILayout.Button("Create Inventory"))
                    {
                        temInventoryScriptable = CreateInstance<InventoryScriptable>();
                        temInventoryScriptable.inventoryData = new InventoryData();
                        temInventoryScriptable.inventoryData.inventoryItems = new List<InventoryData.InventoryItem>();
                        string path = InventoryConstants.InventoryPath + temInventoryName + ".asset";
                        AssetDatabase.CreateAsset(temInventoryScriptable, path);
                        inventroyName = temInventoryName;
                        AssetDatabase.Refresh();
                    }

                    if (GUILayout.Button("Load Inventory"))
                    {
                        string path = InventoryConstants.InventoryPath + temInventoryName + ".asset";
                        temInventoryScriptable = AssetDatabase.LoadAssetAtPath<InventoryScriptable>(path);

                        if (temInventoryScriptable != null)
                        {
                            inventroyName = temInventoryName;
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
                //---------------------------------------------------------------------------------

                EditorGUILayout.EndScrollView();

            
            }

            private void OnDisable()
            {
                if(temInventoryScriptable != null)
                {
                    EditorUtility.SetDirty(temInventoryScriptable);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }

            private static Texture2D MakeTexture(int width, int height, Color color)
            {
                Color[] pixels = new Color[width * height];

                for (int i = 0; i < pixels.Length; i++)
                {
                    pixels[i] = color;
                }

                Texture2D backgroundTexture = new Texture2D(width, height);

                backgroundTexture.SetPixels(pixels);
                backgroundTexture.Apply();

                return backgroundTexture;
            }
        }


        [MenuItem("Data/Delete All Files")]
        private static void DeleteAllFilest()
        {
            string persistentDataPath = Application.persistentDataPath;

            if (Directory.Exists(persistentDataPath))
            {
                string[] files = Directory.GetFiles(persistentDataPath);

                foreach (string file in files)
                {
                    File.Delete(file);
                    Debug.Log($"File '{Path.GetFileName(file)}' deleted successfully from PersistentDataPath.");
                }

                Debug.Log("All files deleted from PersistentDataPath.");
            }
            else
            {
                Debug.LogWarning("PersistentDataPath does not exist.");
            }
        }
    #endif

    }

    public static class InventoryConstants
    {
        public static readonly string InventoryPath = "Assets/Resources/Inventory/";
    }


    public enum PersistentPropertyType
    {
        GameObject, Sprite
    }

    public enum PropertyType
    {
        Int, Float, String, Bool
    }

    /// <summary>
    /// This class purpose is to carry the data to outside class 
    /// </summary>
    /// 
    public class BundledInventoryData
    {
        public List<BundledInventoryItem> bundledInventoryItems;

        public class BundledInventoryItem
        {
            public string inventoryItemType;
            public string inventoryItemName;
            public List<BundledPersistentProperty> bundledPersistentProperties;
            public List<BundledProperty> bundledProperties;

            public class BundledPersistentProperty
            {
                public string name;
                public GameObject targetGameObject;
                public Sprite targetSprite;
            }

            public class BundledProperty
            {
                public string name;
                public int? targetInt = null;
                public float? targetFloat = null;
                public string targetString = null;
                public bool? targetBool = null;
            }
        }

        public List<BundledInventoryItem> Filter(string serach)
        {
            List<BundledInventoryItem> temBundledInventoryItems = new List<BundledInventoryItem>();

            foreach (var bundledInventoryItem in bundledInventoryItems)
            {
                if(CalculateSimilarity(bundledInventoryItem.inventoryItemName, serach) > 0.2f)
                {
                    temBundledInventoryItems.Add(bundledInventoryItem);
                }
            }

            return temBundledInventoryItems;
        }


        private float CalculateSimilarity(string string1, string string2)
        {
            int maxLength = Math.Max(string1.Length, string2.Length);

            if (maxLength == 0)
            {
                return 1.0f;
            }

            int distance = CalculateLevenshteinDistance(string1, string2);

            float similarity = 1.0f - (float)distance / maxLength;

            return similarity;
        }

        private int CalculateLevenshteinDistance(string a, string b)
        {
            int[,] distanceMatrix = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++)
            {
                for (int j = 0; j <= b.Length; j++)
                {
                    if (i == 0)
                        distanceMatrix[i, j] = j;
                    else if (j == 0)
                        distanceMatrix[i, j] = i;
                    else
                    {
                        int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;

                        distanceMatrix[i, j] = Math.Min(
                            Math.Min(distanceMatrix[i - 1, j] + 1, distanceMatrix[i, j - 1] + 1),
                            distanceMatrix[i - 1, j - 1] + cost
                        );
                    }
                }
            }

            return distanceMatrix[a.Length, b.Length];
        }
    }
}



