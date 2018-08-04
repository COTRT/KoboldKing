using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : ItemOnObject, IPointerDownHandler
{
    //For all ya'lls wondering, calling a something "protected" makes it invsible to all but this class, and any classes overriding this one (e.g., CraftResultSlot)
    protected static Tooltip tooltip;
    public static EquipmentSystem eS;
    private ItemSlot duplication;

    /// <summary>
    /// Main Inventory (not root)
    /// </summary>
    protected static Inventory mi;
    public Inventory RootInventory;

    public ItemSlot Duplication
    {
        get
        {
            return duplication;
        }

        set
        {
            duplication = value;
        }
    }
    private ItemOnObject _itemOnObject;
    //public Item Item
    //{
    //    get
    //    {
    //        return transform.childCount > 0 ? (_itemOnObject  ?? (_itemOnObject= transform.GetChild(0).GetComponent<ItemOnObject>())).Item : null;
    //    }
    //    set
    //    {
    //        if (value == null&& transform.childCount > 0)
    //        {
    //            Destroy(transform.GetChild(0).gameObject);
    //        }
    //        else if(value!=null&&transform.childCount==0)
    //        {

    //        }
    //    }
    //}

    void Start()
    {
        tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
        eS = GameObject.FindGameObjectWithTag("EquipmentSystem")?.GetComponent<EquipmentSystem>();
        mi = PlayerInventory.Instance.mainInventory;
        RootInventory = GetInventory();
    }
    protected virtual Inventory GetInventory() //This function exists for the sake of being overideable.
    {
        return transform.parent.parent.GetComponent<Inventory>();
    }
    void OnEnable()
    {
        if (Item != null) Item.PropertyChanged += Item_PropertyChanged;
    }
    void OnDisable()
    {
        if (Item != null) Item.PropertyChanged -= Item_PropertyChanged;
    }

    private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (Duplication != null)
        {
            //TODO remove Duplication functionality
            //duplication.RootInventory.UpdateItemList();
            //duplication.RootInventory.UpdateStackNumbers();
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (RootInventory.GetComponent<EquipmentSystem>() == null && data.button == PointerEventData.InputButton.Right)
        {
            ConsumeIt();
        }
    }

    public virtual void ConsumeIt()
    {
        bool availableToBeConsumed = true;

        bool emptySlotFound = false;
        Transform firstSlotFound = null;
        if (eS != null)
        {
            for (int i = 0; i < eS.slotsInTotal; i++) //Keeping for loop for now... TODO replace with foreach when EquipmentSystem is refactored.
            {
                if (eS.itemTypeOfSlots[i].Equals(Item.Type))
                {
                    Transform slot = eS.transform.GetChild(1).GetChild(i);
                    if (slot.childCount == 0)
                    {
                        if (firstSlotFound == null) firstSlotFound = slot;
                        emptySlotFound = true;
                        RootInventory.EquipItem(Item);

                        transform.SetParent(eS.transform.GetChild(1).GetChild(i));
                        GetComponent<RectTransform>().localPosition = Vector3.zero;
                        availableToBeConsumed = false;
                        if (Duplication != null)
                            Destroy(Duplication.gameObject);
                        break;
                    }
                }
            }
            availableToBeConsumed = firstSlotFound == null;

            if (!emptySlotFound && firstSlotFound != null)
            {
                //TODO replace moving logic with array movement (instead of hierarchy)
                ItemSlot otherItemFromCharacterSystem = firstSlotFound.GetComponent<ItemSlot>();
                Item otherSlotItem = otherItemFromCharacterSystem.Item;
                if (Item.Type == ItemType.UFPS_Weapon)
                {
                    RootInventory.UnEquipItem(otherSlotItem);
                    RootInventory.EquipItem(Item);
                }
                else
                {
                    if (Item.Type != ItemType.Backpack)
                        RootInventory.UnEquipItem(otherItemFromCharacterSystem.GetComponent<ItemOnObject>().Item);
                    RootInventory.EquipItem(Item);
                }

                otherItemFromCharacterSystem.transform.SetParent(transform.parent);
                otherItemFromCharacterSystem.GetComponent<RectTransform>().localPosition = Vector3.zero;
                if (RootInventory.GetComponent<Hotbar>() != null)
                    CreateDuplication(otherItemFromCharacterSystem);

                transform.SetParent(firstSlotFound);
                GetComponent<RectTransform>().localPosition = Vector3.zero;


                availableToBeConsumed = false;
                if (Duplication != null)
                    Destroy(Duplication.gameObject);
                RootInventory.UpdateItemDisplay();
            }

        }


        if (!availableToBeConsumed && Item.Type != ItemType.UFPS_Ammo && Item.Type != ItemType.UFPS_Grenade)
        {
            Item itemFromDup = null;
            if (Duplication != null)
                itemFromDup = Duplication.Item;

            RootInventory.ConsumeItem(Item);

            Item.Quantity--;
            if (Item.Quantity <= 0)
            {
                if (tooltip != null)
                    tooltip.deactivateTooltip();
                RootInventory.DeleteItem(Item);
                Destroy(Duplication.gameObject);
            }
        }

    }

    public void UpdateSlotSize(int slotSize, int iconSize)
    {
        if (transform.childCount > 0)
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
            Transform child = transform.GetChild(0);
            child.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
            child.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(iconSize, iconSize);
        }
    }

    public static void CreateDuplication(ItemSlot ItemToDup)
    {
        //TODO remove this functionality in favor of independent Hotbar and Inventory systems (i.e., no duplication)
        //It also happens that this functionality is not possible with the new array-based inventory system, since in-inventory GameObject references are not attainable.
        //ItemSlot dup = mi.GetComponent<Inventory>().AddItemToInventory(ItemToDup.Item.ID, ItemToDup.Item.Quantity).GetComponent<ItemSlot>();
        //ItemToDup.Duplication = dup;
        //dup.Duplication = ItemToDup;
    }
}
