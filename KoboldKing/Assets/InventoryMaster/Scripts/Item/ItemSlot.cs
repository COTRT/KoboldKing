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
    //Refactor this


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
            duplication.RootInventory.updateItemList();
            duplication.RootInventory.stackableSettings();
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
            if (!mi.checkIfItemAllreadyExist(Item.ItemID, Item.ItemValue))
            {
                mi.addItemToInventory(Item.ItemID, Item.ItemValue);
                mi.stackableSettings();
            }
            CraftSystem cS = PlayerInventory.Instance.cS;
            cS.RemoveItemIngredients(Item); //TODO:  Change CraftSystem to store Blueprint in CraftResultSlot, and pass that in here, to make things significantly less stupid in that department
            CraftResultSlot result = cS.transform.GetChild(3).GetComponent<CraftResultSlot>();
            result.temp = 0;
            tooltip.deactivateTooltip();
            gearable = true;
            mi.updateItemList();
        }
        else
        {
            bool emptySlotFound = false;
            Transform firstSlotFound = null;
            if (eS != null)
            {
                for (int i = 0; i < eS.slotsInTotal; i++) //Keeping for loop for now... TODO replace with foreach when EquipmentSystem is refactored.
                {
                    if (eS.itemTypeOfSlots[i].Equals(Item.ItemType))
                    {
                        Transform slot = eS.transform.GetChild(1).GetChild(i);
                        if (slot.childCount == 0)
                        {
                            if (firstSlotFound == null) firstSlotFound = slot;
                            emptySlotFound = true;
                            RootInventory.EquipItem(Item);

                            transform.SetParent(eS.transform.GetChild(1).GetChild(i));
                            GetComponent<RectTransform>().localPosition = Vector3.zero;
                            eS.GetComponent<Inventory>().updateItemList();
                            RootInventory.updateItemList();
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
                    ItemSlot otherItemFromCharacterSystem = firstSlotFound.GetComponent<ItemSlot>();
                    Item otherSlotItem = otherItemFromCharacterSystem.Item;
                    if (Item.ItemType == ItemType.UFPS_Weapon)
                    {
                        RootInventory.UnEquipItem(otherSlotItem);
                        RootInventory.EquipItem(Item);
                    }
                    else
                    {
                        if (Item.ItemType != ItemType.Backpack)
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
                    eS.GetComponent<Inventory>().updateItemList();
                    RootInventory.OnUpdateItemList();
                }

            }

        }
        if (!gearable && Item.ItemType != ItemType.UFPS_Ammo && Item.ItemType != ItemType.UFPS_Grenade)
        {
            Item itemFromDup = null;
            if (duplication != null)
                itemFromDup = duplication.Item;

            RootInventory.ConsumeItem(Item);

            Item.ItemValue--;
            if (Item.ItemValue <= 0)
            {
                if (tooltip != null)
                    tooltip.deactivateTooltip();
                RootInventory.deleteItemFromInventory(Item);
                Destroy(duplication.gameObject);
            }
        }

    }

    public static void CreateDuplication(ItemSlot ItemToDup)
    {

        ItemSlot dup = mi.GetComponent<Inventory>().addItemToInventory(ItemToDup.Item.ItemID, ItemToDup.Item.ItemValue).GetComponent<ItemSlot>();
        dup.RootInventory.stackableSettings();
        ItemToDup.duplication = dup;
        dup.duplication = ItemToDup;
    }
}
