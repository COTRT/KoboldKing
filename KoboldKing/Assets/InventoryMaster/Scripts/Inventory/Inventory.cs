using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class Inventory : MonoBehaviour
{
    //Prefabs
    [SerializeField]
    private GameObject prefabCanvasWithPanel;
    [SerializeField]
    private GameObject prefabSlot;
    [SerializeField]
    private GameObject prefabSlotContainer;
    [SerializeField]
    private GameObject prefabItem;
    [SerializeField]
    private GameObject prefabDraggingItemContainer;
    [SerializeField]
    private GameObject prefabPanel;

    //Itemdatabase
    [SerializeField]
    private ItemDataBaseList itemDatabase;

    //GameObjects which are alive
    [SerializeField]
    private RectTransform PanelRectTransform;
    [SerializeField]
    private GameObject SlotContainer;
    [SerializeField]
    private RectTransform SlotContainerRectTransform;
    [SerializeField]
    private GridLayoutGroup SlotGridLayout;
    [SerializeField]
    private RectTransform SlotGridRectTransform;

    //Inventory Settings
    [SerializeField]
    public bool mainInventory;
    [SerializeField]
    public Item[] ItemsInInventory = new Item[0];
    [SerializeField]
    public int height;
    [SerializeField]
    public int width;
    public int Size { get { return width * height; } }
    [SerializeField]
    public bool stackable;
    [SerializeField]
    public static bool inventoryOpen;


    //GUI Settings
    [SerializeField]
    public int slotSize;
    [SerializeField]
    public int iconSize;
    [SerializeField]
    public int paddingBetweenX;
    [SerializeField]
    public int paddingBetweenY;
    [SerializeField]
    public int paddingLeft;
    [SerializeField]
    public int paddingRight;
    [SerializeField]
    public int paddingBottom;
    [SerializeField]
    public int paddingTop;
    [SerializeField]
    public int positionNumberX;
    [SerializeField]
    public int positionNumberY;

    InputManager inputManagerDatabase;
    private readonly Dictionary<int, Vector2> DimensionsDict = new Dictionary<int, Vector2>()
    {
        { 3, new Vector2(3,1) }, //For 3 slots, make the inventory 3 wide by 1 high
        {6, new Vector2(3,2) },
        {12, new Vector2(3,4) },
        {16, new Vector2(4,4) },
        {20,new Vector2(4,5) },
        {24,new Vector3(4,6) },
        {30, new Vector2(5,6) },
        {36, new Vector2(6,6) }
    };

    //event delegates for consuming, gearing
    public delegate void ItemDelegate(Item item);
    public static event ItemDelegate ItemConsume;
    public static event ItemDelegate ItemEquip;
    public static event ItemDelegate ItemUnequip;

    public event Action SizeChanged;

    void Start()
    {
        if (transform.GetComponent<Hotbar>() == null)
            gameObject.SetActive(false);

        inputManagerDatabase = (InputManager)Resources.Load("InputManager");
        prefabSlot = Resources.Load("Prefabs/Slot - Inventory") as GameObject;
        if (SlotContainer == null)
        {
            SlotContainer = Instantiate(prefabSlotContainer);
            SlotContainer.transform.SetParent(PanelRectTransform.transform);
        }
       
        //Load SlotContainer Stuff
        SlotContainerRectTransform = SlotContainer.GetComponent<RectTransform>();
        SlotGridRectTransform = SlotContainer.GetComponent<RectTransform>();
        SlotGridLayout = SlotContainer.GetComponent<GridLayoutGroup>();
        SlotContainerRectTransform = SlotContainer.GetComponent<RectTransform>();
        SlotContainerRectTransform.localPosition = Vector3.zero;
        PanelRectTransform = GetComponent<RectTransform>();
        SlotContainer = transform.GetChild(1).gameObject;
        SlotGridLayout = SlotContainer.GetComponent<GridLayoutGroup>();
        SlotGridRectTransform = SlotContainer.GetComponent<RectTransform>();
        
        //Initialize ItemsInInventory array from items in editor hierarchy
        ItemsInInventory = SlotContainer.transform.GetComponentsInChildren<ItemSlot>().Select(i => i.Item).ToArray();
        ChangeInventorySize(ItemsInInventory.Length);
        
        //Setup inventory sizing
        UpdateSlotSize(slotSize, iconSize);
        UpdatePadding(paddingBetweenX, paddingBetweenY);
        
        //Nab Prefabs
        prefabCanvasWithPanel = Resources.Load("Prefabs/Canvas - Inventory") as GameObject;
        prefabSlot = Resources.Load("Prefabs/Slot - Inventory") as GameObject;
        prefabSlotContainer = Resources.Load("Prefabs/Slots - Inventory") as GameObject;
        prefabItem = Resources.Load("Prefabs/Item") as GameObject;
        itemDatabase = (ItemDataBaseList)Resources.Load("ItemDatabase");
        prefabDraggingItemContainer = Resources.Load("Prefabs/DraggingItem") as GameObject;
        prefabPanel = Resources.Load("Prefabs/Panel - Inventory") as GameObject;

        //Default Settings
        if (GetComponent<EquipmentSystem>() == null && GetComponent<Hotbar>() == null && GetComponent<CraftSystem>() == null)
        {
            height = 5;
            width = 5;

            slotSize = 50;
            iconSize = 45;

            paddingBetweenX = 5;
            paddingBetweenY = 5;
            paddingTop = 35;
            paddingBottom = 10;
            paddingLeft = 10;
            paddingRight = 10;
        }
        else if (GetComponent<Hotbar>() != null)
        {
            height = 1;
            width = 9;

            slotSize = 50;
            iconSize = 45;

            paddingBetweenX = 5;
            paddingBetweenY = 5;
            paddingTop = 10;
            paddingBottom = 10;
            paddingLeft = 10;
            paddingRight = 10;
        }
        else if (GetComponent<CraftSystem>() != null)
        {
            height = 3;
            width = 3;
            slotSize = 55;
            iconSize = 45;

            paddingBetweenX = 5;
            paddingBetweenY = 5;
            paddingTop = 35;
            paddingBottom = 95;
            paddingLeft = 25;
            paddingRight = 25;
        }
        else
        {
            height = 4;
            width = 2;

            slotSize = 50;
            iconSize = 45;

            paddingBetweenX = 100;
            paddingBetweenY = 20;
            paddingTop = 35;
            paddingBottom = 10;
            paddingLeft = 10;
            paddingRight = 10;
        }
    }

    public void ChangeInventorySize(int size)
    {
        if (size < ItemsInInventory.Length)
        {
            foreach (var item in ItemsInInventory.Skip(size))
            {
                if (item == null) continue;
                GameObject dropItem = Instantiate(item.Model);
                dropItem.AddComponent<PickUpItem>().item = item;
                dropItem.transform.localPosition = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
            }
        }

        Vector2 dimensions;
        if (!DimensionsDict.TryGetValue(size, out dimensions)) //Check first to see if the given dimensions are in the dimensions dictionary
        {
            //Some very fancy math to calculate the most even grid possible for the given size.
            for (float h = Mathf.Ceil(Mathf.Sqrt(size)); h > 1; h--) //Count down from the Square Root (i.e., square inventory) of the total size (i.e. number of slots)
            {
                if ((size / h) % 1 == 0) //If the given number can divide the size evenly...
                {
                    dimensions = new Vector2(size / h, h); //Use that number as the height and the quotient as the width
                    break;
                }
            }
        }
        width = (int)dimensions.x;
        height = (int)dimensions.y;
        Array.Resize(ref ItemsInInventory, size);
        UpdateItemDisplay();
        AdjustInventorySize();
        SizeChanged?.Invoke();
    }

    public void UpdateItemDisplay()
    {

        ItemSlot[] startingSlots = SlotContainer.transform.GetComponentsInChildren<ItemSlot>();

        if (startingSlots.Length > ItemsInInventory.Length)
        {
            foreach (var slot in startingSlots.Skip(ItemsInInventory.Length).ToList())
            {
                Destroy(slot.gameObject);
            }
        }
        else if (startingSlots.Length < ItemsInInventory.Length)
        {
            for (int i = startingSlots.Length; i < ItemsInInventory.Length; i++)
            {
                ItemSlot Slot = Instantiate(prefabSlot).GetComponent<ItemSlot>();
                Slot.name = (i + 1).ToString();
                Slot.transform.SetParent(SlotContainer.transform);
            }
        } //Do nothing if the inventory array and hierarchy are aligned.

        //Update the Items in the slots
        for (int i = 0; i < ItemsInInventory.Length; i++)
        {
            var child = SlotContainer.transform.GetChild(i);
            if (ItemsInInventory[i] == null && child.childCount != 0)
            {
                Destroy(child.GetChild(0));
            }
            else if (ItemsInInventory[i] != null)
            {
                child.GetComponent<ItemSlot>().Item = ItemsInInventory[i];
            }
        }
        UpdateStackNumberSettings();
    }
    public void UpdateStackNumberSettings()  //TODO:  I'd imagine this is unnecessary if we update properly.
    {
        foreach (var itemSlot in GetComponentsInChildren<ItemSlot>()) itemSlot.UpdateDisplay(stackable, positionNumberX, positionNumberY);
    }

    public void UpdateSlotSize(int slotSize, int iconSize)
    {
        this.slotSize = slotSize;
        this.iconSize = iconSize;
        SlotGridLayout.cellSize = new Vector2(slotSize, slotSize);
        foreach (var slot in GetComponentsInChildren<ItemSlot>()) slot.UpdateSlotSize(slotSize, iconSize);
    }

    public void SortItems()
    {
        ItemsInInventory = ItemsInInventory.OrderBy(i => i == null).ToArray();
        UpdateItemDisplay();
        //A potential concern with this system is this will create and destroy moved items instead of, 
        //...you know, moving the items.  Perhaps a future improvement.
        //It is still a whole lot prettier and shorter than the old system.
    }

    public void SetMain(bool main)
    {
        if (main)
            this.gameObject.tag = "Untagged";
        else
            this.gameObject.tag = "MainInventory";
    }

    public void CloseInventory()
    {
        this.gameObject.SetActive(false);
    }

    public void OpenInventory()
    {
        this.gameObject.SetActive(true);
    }

    public void ConsumeItem(Item item)
    {
        ItemConsume?.Invoke(item);
    }

    public void EquipItem(Item item)
    {
        ItemEquip?.Invoke(item);
    }

    public void UnEquipItem(Item item)
    {
        ItemUnequip?.Invoke(item);
    }

    public bool CharacterSystem()
    {
        return GetComponent<EquipmentSystem>() != null;
    }

    public void AdjustInventorySize()
    {
        int x = (((width * slotSize) + ((width - 1) * paddingBetweenX)) + paddingLeft + paddingRight);
        int y = (((height * slotSize) + ((height - 1) * paddingBetweenY)) + paddingTop + paddingBottom);
        PanelRectTransform.sizeDelta = new Vector2(x, y);

        SlotGridRectTransform.sizeDelta = new Vector2(x, y);
    }

    public void UpdatePadding(int spacingBetweenX, int spacingBetweenY)
    {
        SlotGridLayout.spacing = new Vector2(paddingBetweenX, paddingBetweenY);
        SlotGridLayout.padding.bottom = paddingBottom;
        SlotGridLayout.padding.right = paddingRight;
        SlotGridLayout.padding.left = paddingLeft;
        SlotGridLayout.padding.top = paddingTop;
    }

    /// <summary>
    /// Add the item with provided ID (and quantity) to the inventory at the first available slots.  
    /// Returns the number of successfully added numbers 
    /// -(this will be less than the specified quantity if the inventory is too full)
    /// acceptPartialMove determines if the inventory should completely cancel the addition if there isn't enough space to complete it.
    /// </summary>
    public int AddItemToInventory(int id, int quantity,bool acceptPartialMove = true)
    {
        int totalCapacity = ItemsInInventory.Sum(i =>
        {
            if (i == null)
            {
                return i.MaxStack;
            }
            else if (i.ID == id)
            {
                return i.MaxStack - i.Quantity;
            }
            else
            {
                return 0;
            }
        });
        if (totalCapacity < quantity && !acceptPartialMove) return 0; //Add nothing if there isn't space for it and we have been instructed to kick out.
        int remainingQuantity = quantity;
        foreach(var item in Array.FindAll(ItemsInInventory,i => i.ID == id))
        {
            if (remainingQuantity <= 0) break;
            var availableCapacity = item.MaxStack - item.Quantity;
            if (availableCapacity>0) {
                var increase = Mathf.Min(availableCapacity, remainingQuantity); //Choose the smaller value
                remainingQuantity -= increase;
                item.Quantity += increase;
            }
        }
        while(remainingQuantity>0&&ItemsInInventory.Any(i => i == null))
        {
            var item = itemDatabase.getItemByID(id);
            var increase = Mathf.Min(remainingQuantity, item.MaxStack);
            remainingQuantity -= increase;
            item.Quantity += increase;
            ItemsInInventory[Array.FindIndex(ItemsInInventory, i => i == null)] = item;
        }
        
        UpdateItemDisplay();
        return quantity - remainingQuantity;
    }

    public void DeleteItem(Item item)
    {
        ItemsInInventory[Array.FindIndex(ItemsInInventory, i => i == item)] = null; //One unfortunate side effect of using an array; you get to use "Array" in front of everything.
        UpdateItemDisplay();
    }
}
