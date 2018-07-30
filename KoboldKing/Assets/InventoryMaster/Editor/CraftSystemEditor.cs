using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[CustomEditor(typeof(CraftSystem))]
public class CraftSystemEditor : Editor
{


    SerializedProperty finalSlotPositionX;
    SerializedProperty finalSlotPositionY;
    SerializedProperty leftArrowPositionX;
    SerializedProperty leftArrowPositionY;
    SerializedProperty rightArrowPositionX;
    SerializedProperty rightArrowPositionY;
    SerializedProperty leftArrowRotation;
    SerializedProperty rightArrowRotation;
    CraftSystem cS;

    bool showFinalSlot;
    bool showArrow;
    bool showLeftArrow;
    bool showRightArrow;
    bool showDesign;
    bool showFinalSlotDesign;
    bool showArrowDesign;

    int imageTypeofFinalSlot;
    int imageTypeofArrows;
    void OnEnable()
    {
        cS = target as CraftSystem;
        finalSlotPositionX = serializedObject.FindProperty("finalSlotPositionX");
        finalSlotPositionY = serializedObject.FindProperty("finalSlotPositionY");
        leftArrowPositionX = serializedObject.FindProperty("leftArrowPositionX");
        leftArrowPositionY = serializedObject.FindProperty("leftArrowPositionY");
        rightArrowPositionX = serializedObject.FindProperty("rightArrowPositionX");
        rightArrowPositionY = serializedObject.FindProperty("rightArrowPositionY");
        leftArrowRotation = serializedObject.FindProperty("leftArrowRotation");
        rightArrowRotation = serializedObject.FindProperty("rightArrowRotation");
        cS.Start();
        cS.MirrorLeftArrowToRight();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUILayout.BeginVertical("Box");
        EditorGUI.indentLevel++;
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginVertical("Box");
        showFinalSlot = EditorGUILayout.Foldout(showFinalSlot, "FinalSlot");
        if (showFinalSlot)
        {
            EditorGUI.indentLevel++;
            finalSlotPositionX.intValue = EditorGUILayout.IntSlider("Position X", finalSlotPositionX.intValue, -cS.GetSizeX() / 2, cS.GetSizeX() / 2);
            finalSlotPositionY.intValue = EditorGUILayout.IntSlider("Position Y", finalSlotPositionY.intValue, -cS.GetSizeY() / 2, cS.GetSizeY() / 2);
            if (EditorGUI.EndChangeCheck())
            {
                cS.SetPositionFinalSlot();
            }
            EditorGUI.indentLevel--;
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical("Box");
        showArrow = EditorGUILayout.Foldout(showArrow, "Arrows");
        if (showArrow)
        {
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            showLeftArrow = EditorGUILayout.Foldout(showLeftArrow, "LeftArrow");
            if (showLeftArrow)
            {
                EditorGUI.indentLevel++;
                leftArrowPositionX.intValue = EditorGUILayout.IntSlider("Position X", leftArrowPositionX.intValue, -cS.GetSizeX() / 2, cS.GetSizeX() / 2);
                leftArrowPositionY.intValue = EditorGUILayout.IntSlider("Position Y", leftArrowPositionY.intValue, -cS.GetSizeY() / 2, cS.GetSizeY() / 2);
                leftArrowRotation.intValue = EditorGUILayout.IntSlider("Rotation", leftArrowRotation.intValue, 0, 360);
                if (EditorGUI.EndChangeCheck())
                {
                    cS.SetArrowSettings();
                }
                EditorGUI.indentLevel--;
            }

            showRightArrow = EditorGUILayout.Foldout(showRightArrow, "RightArrow");
            if (showRightArrow)
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                rightArrowPositionX.intValue = EditorGUILayout.IntSlider("Position X", rightArrowPositionX.intValue, -cS.GetSizeX() / 2, cS.GetSizeX() / 2);
                rightArrowPositionY.intValue = EditorGUILayout.IntSlider("Position Y", rightArrowPositionY.intValue, -cS.GetSizeY() / 2, cS.GetSizeY() / 2);
                rightArrowRotation.intValue = EditorGUILayout.IntSlider("Rotation", rightArrowRotation.intValue, 0, 360);
                if (EditorGUI.EndChangeCheck())
                {
                    cS.SetArrowSettings();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;

        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical("Box");
        showDesign = EditorGUILayout.Foldout(showDesign, "Design");
        if (showDesign)
        {
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            showFinalSlotDesign = EditorGUILayout.Foldout(showFinalSlotDesign, "Finalslot");
            if (showFinalSlotDesign)
            {
                EditorGUI.indentLevel++;
                cS.finalSlotImage.sprite = (Sprite)EditorGUILayout.ObjectField("Source Image", cS.finalSlotImage.sprite, typeof(Sprite), true);
                cS.finalSlotImage.color = EditorGUILayout.ColorField("Color", cS.finalSlotImage.color);
                cS.finalSlotImage.material = (Material)EditorGUILayout.ObjectField("Material", cS.finalSlotImage.material, typeof(Material), true);
                string[] imageTypes = new string[4]; imageTypes[0] = "Filled"; imageTypes[1] = "Simple"; imageTypes[2] = "Sliced"; imageTypes[3] = "Tiled";
                imageTypeofFinalSlot = EditorGUILayout.Popup("Image Type", imageTypeofFinalSlot, imageTypes, EditorStyles.popup);
                if (imageTypeofFinalSlot == 0) { cS.finalSlotImage.type = Image.Type.Filled; imageTypeofFinalSlot = 0; }
                else if (imageTypeofFinalSlot == 1) { cS.finalSlotImage.type = Image.Type.Simple; imageTypeofFinalSlot = 1; }
                else if (imageTypeofFinalSlot == 2) { cS.finalSlotImage.type = Image.Type.Sliced; imageTypeofFinalSlot = 2; }
                else if (imageTypeofFinalSlot == 3) { cS.finalSlotImage.type = Image.Type.Tiled; imageTypeofFinalSlot = 3; }
                cS.finalSlotImage.fillCenter = EditorGUILayout.Toggle("Fill Center", cS.finalSlotImage.fillCenter);
                EditorGUI.indentLevel--;
            }

            showArrowDesign = EditorGUILayout.Foldout(showArrowDesign, "Arrows");
            if (showArrowDesign)
            {
                EditorGUI.indentLevel++;
                cS.leftArrowImage.sprite = (Sprite)EditorGUILayout.ObjectField("Source Image", cS.leftArrowImage.sprite, typeof(Sprite), true);
                cS.leftArrowImage.color = EditorGUILayout.ColorField("Color", cS.leftArrowImage.color);
                cS.leftArrowImage.material = (Material)EditorGUILayout.ObjectField("Material", cS.leftArrowImage.material, typeof(Material), true);
                string[] imageTypes = new string[4]; imageTypes[0] = "Filled"; imageTypes[1] = "Simple"; imageTypes[2] = "Sliced"; imageTypes[3] = "Tiled";
                imageTypeofArrows = EditorGUILayout.Popup("Image Type", imageTypeofArrows, imageTypes, EditorStyles.popup);
                if (imageTypeofArrows == 0) { cS.leftArrowImage.type = Image.Type.Filled; imageTypeofArrows = 0; }
                else if (imageTypeofArrows == 1) { cS.leftArrowImage.type = Image.Type.Simple; imageTypeofArrows = 1; }
                else if (imageTypeofArrows == 2) { cS.leftArrowImage.type = Image.Type.Sliced; imageTypeofArrows = 2; }
                else if (imageTypeofArrows == 3) { cS.leftArrowImage.type = Image.Type.Tiled; imageTypeofArrows = 3; }
                cS.leftArrowImage.fillCenter = EditorGUILayout.Toggle("Fill Center", cS.leftArrowImage.fillCenter);
                EditorGUI.indentLevel--;
            }
            if (EditorGUI.EndChangeCheck())
            {
                cS.MirrorLeftArrowToRight();
            }
            EditorGUI.indentLevel--;
        }
        GUILayout.EndVertical();

        EditorGUI.indentLevel--;
        serializedObject.ApplyModifiedProperties();
        SceneView.RepaintAll();
        EditorGUI.indentLevel--;
        GUILayout.EndVertical();
    }

    [MenuItem("Master System/Create/Craft System")]
    public static void MenuItemCreateInventory()
    {
        GameObject Canvas = null;
        if (GameObject.FindGameObjectWithTag("Canvas") == null)
        {
            GameObject inventory = new GameObject();
            inventory.name = "Inventories";
            Canvas = (GameObject)Instantiate(Resources.Load("Prefabs/Canvas - Inventory") as GameObject);
            Canvas.transform.SetParent(inventory.transform, true);
            GameObject panel = (GameObject)Instantiate(Resources.Load("Prefabs/Panel - CraftSytem") as GameObject);
            panel.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            panel.transform.SetParent(Canvas.transform, true);
            GameObject draggingItem = (GameObject)Instantiate(Resources.Load("Prefabs/DraggingItem") as GameObject);
            Instantiate(Resources.Load("Prefabs/EventSystem") as GameObject);
            draggingItem.transform.SetParent(Canvas.transform, true);
            panel.AddComponent<CraftSystem>();
        }
        else
        {
            GameObject panel = (GameObject)Instantiate(Resources.Load("Prefabs/Panel - CraftSystem") as GameObject);
            panel.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, true);
            panel.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            panel.AddComponent<CraftSystem>();
            DestroyImmediate(GameObject.FindGameObjectWithTag("DraggingItem"));
            GameObject draggingItem = (GameObject)Instantiate(Resources.Load("Prefabs/DraggingItem") as GameObject);
            draggingItem.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, true);
        }
    }

}

