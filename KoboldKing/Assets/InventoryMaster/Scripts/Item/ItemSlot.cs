using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : ItemOnObject, IPointerDownHandler
{
    private static Tooltip tooltip;
    public static EquipmentSystem eS;
    public ItemSlot duplication;

    private static Inventory mi;
    public Inventory RootInventory;


    void Start()
    {
        tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
        var eSGameObject = GameObject.FindGameObjectWithTag("EquipmentSystem");
        eS = eSGameObject == null ? null : eSGameObject.GetComponent<EquipmentSystem>(); //I'm so missing C# 7's eSGameObject?.GetComponent<>() right now
        mi = PlayerInventory.Instance.mainInventory;
        RootInventory = transform.parent.parent.parent.GetComponent<Inventory>();
    }
    void OnEnable()
    {
        Item.PropertyChanged += Item_PropertyChanged;
    }
    void OnDisable()
    {
        Item.PropertyChanged -= Item_PropertyChanged;
    }

    private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (duplication != null)
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

    public void ConsumeIt()
    {
        bool gearable = false;

        //item from craft system to inventory
        if (transform.parent.GetComponent<CraftResultSlot>() != null)
        {
            if (mi.AddItemToInventory(Item.ID, Item.Quantity)!=0)
            {
                CraftSystem cS = PlayerInventory.Instance.craftSystem;
                cS.RemoveItemIngredients(Item); //TODO:  Change CraftSystem to store Blueprint in CraftResultSlot, and pass that in here, to make things significantly less stupid in that department
                CraftResultSlot result = cS.transform.GetChild(3).GetComponent<CraftResultSlot>();
                result.temp = 0;
                tooltip.deactivateTooltip();
                gearable = true;
            }
        }
        else
        {
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
                            gearable = true;
                            if (duplication != null)
                                Destroy(duplication.gameObject);
                            break;
                        }
                    }
                }
                gearable = firstSlotFound != null;

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


                    gearable = true;
                    if (duplication != null)
                        Destroy(duplication.gameObject);
                    RootInventory.UpdateItemDisplay();
                }

            }

        }
        if (!gearable && Item.Type != ItemType.UFPS_Ammo && Item.Type != ItemType.UFPS_Grenade)
        {
            Item itemFromDup = null;
            if (duplication != null)
                itemFromDup = duplication.Item;

            RootInventory.ConsumeItem(Item);

            Item.Quantity--;
            if (Item.Quantity <= 0)
            {
                if (tooltip != null)
                    tooltip.deactivateTooltip();
                RootInventory.DeleteItem(Item);
                Destroy(duplication.gameObject);
            }
        }

    }

    public void UpdateSlotSize(int slotSize, int iconSize)
    {
        if (transform.childCount > 0)
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
            transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
            transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(iconSize, iconSize);
        }
    }

    public static void CreateDuplication(ItemSlot ItemToDup)
    {
        //TODO remove this functionality in favor of independent Hotbar and Inventory systems (i.e., no duplication)
        //It also happens that this functionality is not possible with the new array-based inventory system, since in-inventory GameObject references are not attainable.
        ItemSlot dup = mi.GetComponent<Inventory>().AddItemToInventory(ItemToDup.Item.ID, ItemToDup.Item.Quantity).GetComponent<ItemSlot>();
        ItemToDup.duplication = dup;
        dup.duplication = ItemToDup;
    }
}
