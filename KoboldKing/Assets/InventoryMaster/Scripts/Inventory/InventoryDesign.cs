using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Image))]
public class InventoryDesign : MonoBehaviour
{

    //UIDesign
    [SerializeField]
    public Image inventoryDesign;
    [SerializeField]
    public bool showInventoryCross;
    [SerializeField]
    public Image inventoryCrossImage;
    [SerializeField]
    public RectTransform inventoryCrossRectTransform;
    [SerializeField]
    public int inventoryCrossPosX;
    [SerializeField]
    public int inventoryCrossPosY;
    [SerializeField]
    public string inventoryTitle;
    [SerializeField]
    public Text inventoryTitleText;
    [SerializeField]
    public int inventoryTitlePosX;
    [SerializeField]
    public int inventoryTitlePosY;
    [SerializeField]
    public int panelSizeX;
    [SerializeField]
    public int panelSizeY;

    public Sprite slotSprite;
    public Color slotColor;
    public Material slotMaterial;
    public Image.Type slotImageType;
    public bool slotFillCenter;

    public void SetVariables()
    {
        inventoryTitlePosX = (int)transform.GetChild(0).GetComponent<RectTransform>().localPosition.x;
        inventoryTitlePosY = (int)transform.GetChild(0).GetComponent<RectTransform>().localPosition.y;
        panelSizeX = (int)GetComponent<RectTransform>().sizeDelta.x;
        panelSizeY = (int)GetComponent<RectTransform>().sizeDelta.y;
        inventoryTitle = transform.GetChild(0).GetComponent<Text>().text;
        inventoryTitleText = transform.GetChild(0).GetComponent<Text>();
        if (GetComponent<Hotbar>() == null)
        {
            inventoryCrossRectTransform = transform.GetChild(2).GetComponent<RectTransform>();
            inventoryCrossImage = transform.GetChild(2).GetComponent<Image>();
        }
        inventoryDesign = GetComponent<Image>();
        var slotContainer = transform.GetChild(1);
        if (slotContainer.childCount!=0)
        {
            var firstSlot = slotContainer.GetChild(0).GetComponent<Image>();
            slotSprite = firstSlot.sprite;
            slotColor = firstSlot.color;
            slotMaterial = firstSlot.material;
            slotImageType = firstSlot.type;
            slotFillCenter = firstSlot.fillCenter;
        }

    }

    public void UpdateTitle()
    {
        transform.GetChild(0).GetComponent<Text>().text = inventoryTitle;
        transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(inventoryTitlePosX, inventoryTitlePosY, 0); //Title pos
    }


    public void ChangeCrossSettings()
    {
        GameObject cross = transform.GetChild(2).gameObject;
        if (showInventoryCross)
        {
            cross.SetActive(showInventoryCross);
            inventoryCrossRectTransform.localPosition = new Vector3(inventoryCrossPosX, inventoryCrossPosY, 0);

        }
        else
        {
            cross.SetActive(showInventoryCross);
        }
    }

    public void UpdateAllSlots()
    {
        Image slot = null;
#if UNITY_EDITOR
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Prefabs/Slot - Inventory.prefab");

        AssetDatabase.StartAssetEditing();
        var pi = prefab.GetComponent<Image>();
        if(pi == null)
        {
            pi = prefab.AddComponent<Image>();
        }
        pi.sprite = slotSprite;
        pi.color = slotColor;
        pi.material = slotMaterial;
        pi.type = slotImageType;
        pi.fillCenter = slotFillCenter;
        AssetDatabase.StopAssetEditing();
#endif
        foreach (Transform child in transform.GetChild(1)) {
            slot = child.GetComponent<Image>();
            Debug.Log(slot.name);
            slot.sprite = slotSprite;
            slot.color = slotColor;
            slot.material = slotMaterial;
            slot.type = slotImageType;
            slot.fillCenter = slotFillCenter;
        }
    }
}
