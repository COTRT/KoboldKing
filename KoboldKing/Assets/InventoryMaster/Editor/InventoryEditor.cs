using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor
{
    Inventory inv;

    SerializedProperty inventoryHeight;
    SerializedProperty inventoryWidth;
    SerializedProperty inventorySlotSize;
    SerializedProperty inventoryIconSize;
    SerializedProperty inventoryStackable;
    SerializedProperty mainInventory;

    SerializedProperty slotsPaddingBetweenX;
    SerializedProperty slotsPaddingBetweenY;
    SerializedProperty slotsPaddingLeft;
    SerializedProperty slotsPaddingRight;
    SerializedProperty slotsPaddingTop;
    SerializedProperty slotsPaddingBottom;
    SerializedProperty positionNumberX;
    SerializedProperty positionNumberY;


    private bool showStackableItems;


    private bool showInventorySettings = true;
    private bool showInventoryPadding = false;
    private bool showStackableItemsSettings = false;

    private int itemID;
    private int itemValue = 1;
    private int imageTypeIndex;



    void OnEnable()
    {
        inv = target as Inventory;
        inventoryHeight = serializedObject.FindProperty("height");
        inventoryWidth = serializedObject.FindProperty("width");
        inventorySlotSize = serializedObject.FindProperty("slotSize");
        inventoryIconSize = serializedObject.FindProperty("iconSize");
        slotsPaddingBetweenX = serializedObject.FindProperty("paddingBetweenX");
        slotsPaddingBetweenY = serializedObject.FindProperty("paddingBetweenY");
        slotsPaddingLeft = serializedObject.FindProperty("paddingLeft");
        slotsPaddingRight = serializedObject.FindProperty("paddingRight");
        slotsPaddingTop = serializedObject.FindProperty("paddingTop");
        slotsPaddingBottom = serializedObject.FindProperty("paddingBottom");
        inventoryStackable = serializedObject.FindProperty("stackable");
        positionNumberX = serializedObject.FindProperty("positionNumberX");
        positionNumberY = serializedObject.FindProperty("positionNumberY");
        mainInventory = serializedObject.FindProperty("mainInventory");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUILayout.BeginVertical("Box");
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(mainInventory, new GUIContent("Player Inventory"));
        if (EditorGUI.EndChangeCheck())
        {
            inv.SetMain(mainInventory.boolValue);
        }
        GUILayout.EndVertical();


        GUILayout.BeginVertical("Box");
        EditorGUI.indentLevel++;
        GUILayout.BeginVertical("Box");
        showInventorySettings = EditorGUILayout.Foldout(showInventorySettings, "Inventory Settings");
        if (showInventorySettings)
        {
            sizeOfInventoryGUI();
        }
        GUILayout.EndVertical();

        if (!inv.CharacterSystem())
        {
            GUILayout.BeginVertical("Box");
            showStackableItemsSettings = EditorGUILayout.Foldout(showStackableItemsSettings, "Stacking/Splitting");
            if (showStackableItemsSettings)
            {
                stackableItemsSettings();
                GUILayout.Space(20);
            }
            GUILayout.EndVertical();

        }
        EditorGUI.indentLevel--;
        GUILayout.EndVertical();


        serializedObject.ApplyModifiedProperties();
        SceneView.RepaintAll();
        GUILayout.BeginVertical("Box");
        addItemGUI();
        GUILayout.EndVertical();

    }


    void stackableItemsSettings()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(inventoryStackable, new GUIContent("Splitting/Stacking"));
        if (EditorGUI.EndChangeCheck()) inv.UpdateStackNumberSettings();
        if (inventoryStackable.boolValue)
        {
            EditorGUI.indentLevel++;
            showStackableItems = EditorGUILayout.Foldout(showStackableItems, "StackNumberPosition");
            if (showStackableItems)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.indentLevel++;
                positionNumberX.intValue = EditorGUILayout.IntSlider("Position X:", positionNumberX.intValue, -(inventorySlotSize.intValue / 2), inventorySlotSize.intValue / 2);
                positionNumberY.intValue = EditorGUILayout.IntSlider("Position Y:", positionNumberY.intValue, -(inventorySlotSize.intValue / 2), inventorySlotSize.intValue / 2);
                EditorGUI.indentLevel--;
                if (EditorGUI.EndChangeCheck())
                {
                    inv.UpdateStackNumberSettings();
                }
            }
            EditorGUI.indentLevel--;
        }
    }

    void sizeOfInventoryGUI()
    {

        EditorGUI.indentLevel++;
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.IntSlider(inventoryHeight, 1, 10, new GUIContent("Height"));
        EditorGUILayout.IntSlider(inventoryWidth, 1, 10, new GUIContent("Width"));
        if (EditorGUI.EndChangeCheck())
        {
            inv.UpdateItemDisplay();
            inv.AdjustInventorySize();
            inv.UpdatePadding(slotsPaddingBetweenX.intValue, slotsPaddingBetweenY.intValue);
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.IntSlider(inventorySlotSize, 20, 100, new GUIContent("Slot Size"));                                                                                        //intfield for the slotsize
        EditorGUILayout.IntSlider(inventoryIconSize, 20, 100, new GUIContent("Icon Size"));                                                                                        //intfield for the slotsize
        if (EditorGUI.EndChangeCheck())                                                                                                        //if intfield got changed
        {
            inv.UpdateSlotSize(inventorySlotSize.intValue, inventoryIconSize.intValue);
        }

        GUILayout.BeginVertical("Box");
        showInventoryPadding = EditorGUILayout.Foldout(showInventoryPadding, "Padding");
        if (showInventoryPadding)
        {

            EditorGUI.BeginChangeCheck();
            EditorGUI.indentLevel++;
            slotsPaddingLeft.intValue = EditorGUILayout.IntField("Left:", slotsPaddingLeft.intValue);
            slotsPaddingRight.intValue = EditorGUILayout.IntField("Right:", slotsPaddingRight.intValue);
            slotsPaddingBetweenX.intValue = EditorGUILayout.IntField("Spacing X:", slotsPaddingBetweenX.intValue);
            slotsPaddingBetweenY.intValue = EditorGUILayout.IntField("Spacing Y:", slotsPaddingBetweenY.intValue);
            slotsPaddingTop.intValue = EditorGUILayout.IntField("Top:", slotsPaddingTop.intValue);
            slotsPaddingBottom.intValue = EditorGUILayout.IntField("Bottom:", slotsPaddingBottom.intValue);
            EditorGUI.indentLevel--;
            if (EditorGUI.EndChangeCheck())
            {
                inv.AdjustInventorySize();
                inv.UpdatePadding(slotsPaddingBetweenX.intValue, slotsPaddingBetweenY.intValue);
            }

        }
        GUILayout.EndVertical();


        EditorGUI.indentLevel--;
    }

    void addItemGUI()                                                                                                       //add a item to the inventory through the inspector
    {
        if (!inv.CharacterSystem())
        {
            GUILayout.Label("Add an item:");
            EditorGUILayout.BeginHorizontal();                                                                                  //starting horizontal GUI elements
            ItemDataBaseList inventoryItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");                            //loading the itemdatabase
            string[] items = inventoryItemList.itemList.Select(i => i.Name).ToArray();
            itemID = EditorGUILayout.Popup("", itemID, items, EditorStyles.popup);                                              //create a popout with all itemnames in it and save the itemID of it
            itemValue = EditorGUILayout.IntField("", itemValue, GUILayout.Width(40));
            GUI.color = Color.green;                                                                                            //set the color of all following guielements to green
            if (GUILayout.Button("Add Item"))                                                                                   //creating button with name "AddItem"
            {                
                inv.AddItemToInventory(itemID, itemValue);                                                                      //and set the settings for possible stackedItems
            }

            EditorGUILayout.EndHorizontal();                                                                                    //end the horizontal gui layout
        }
    }

    [MenuItem("Master System/Create/Inventory and Storage")]        //creating the menu item
    public static void MenuItemCreateInventory()       //create the inventory at start
    {
        GameObject Canvas = null;
        if (GameObject.FindGameObjectWithTag("Canvas") == null)
        {
            GameObject inventory = new GameObject();
            inventory.name = "Inventories";
            Canvas = (GameObject)Instantiate(Resources.Load("Prefabs/Canvas - Inventory") as GameObject);
            Canvas.transform.SetParent(inventory.transform, true);
            GameObject panel = (GameObject)Instantiate(Resources.Load("Prefabs/Panel - Inventory") as GameObject);
            panel.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            panel.transform.SetParent(Canvas.transform, true);
            GameObject draggingItem = (GameObject)Instantiate(Resources.Load("Prefabs/DraggingItem") as GameObject);
            draggingItem.transform.SetParent(Canvas.transform, true);
            Inventory temp = panel.AddComponent<Inventory>();
            Instantiate(Resources.Load("Prefabs/EventSystem") as GameObject);
            panel.AddComponent<InventoryDesign>();
        }
        else
        {
            GameObject panel = (GameObject)Instantiate(Resources.Load("Prefabs/Panel - Inventory") as GameObject);
            panel.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, true);
            panel.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            Inventory temp = panel.AddComponent<Inventory>();
            panel.AddComponent<InventoryDesign>();
            DestroyImmediate(GameObject.FindGameObjectWithTag("DraggingItem"));
            GameObject draggingItem = (GameObject)Instantiate(Resources.Load("Prefabs/DraggingItem") as GameObject);
            draggingItem.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, true);
        }
    }
}
