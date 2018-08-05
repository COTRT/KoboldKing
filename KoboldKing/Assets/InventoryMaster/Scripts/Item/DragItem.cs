using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//TODO:  Refactor for new Inventory Format.
public class DragItem : MonoBehaviour, IDragHandler, IPointerDownHandler, IEndDragHandler
{
    private Vector2 pointerOffset;
    private RectTransform sourceRectTransform;
    private RectTransform draggedItemRectTransform;
    private CanvasGroup sourceCanvasGroup;
    private ItemSlot sourceSlot;
    private Inventory sourceInventory;
    private Transform draggedItemBox;

    void Start()
    {
        draggedItemBox = GameObject.FindGameObjectWithTag("DraggingItem").transform;
        draggedItemRectTransform = draggedItemBox.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(sourceRectTransform, data.position, data.pressEventCamera, out pointerOffset);
            sourceSlot = transform.parent.GetComponent<ItemSlot>();
            sourceRectTransform = GetComponent<RectTransform>();
            sourceCanvasGroup = GetComponent<CanvasGroup>();
            sourceRectTransform.SetAsLastSibling();
            sourceInventory = transform.parent.GetComponent<ItemSlot>().RootInventory;
            transform.SetParent(draggedItemBox);
            //TODO:  remove dragged item from source inventory
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left && transform.parent.GetComponent<CraftResultSlot>() == null)
        {
            sourceCanvasGroup.blocksRaycasts = false;
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(draggedItemRectTransform, Input.mousePosition, data.pressEventCamera, out localPointerPosition))
            {
                Debug.Log(localPointerPosition);
                sourceRectTransform.localPosition = localPointerPosition - pointerOffset;
                //ItemSlot duplication = oldSlot.GetComponent<ItemSlot>().Duplication;
                //if (duplication != null)
                //    Destroy(duplication);
            }
        }
    }


    public void OnEndDrag(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            if(!MoveItem(data))
                Destroy(gameObject);
        }
    }

    /// <returns>Whether or not to preserve the dragging item</returns>
    public bool MoveItem(PointerEventData data)
    {

        sourceCanvasGroup.blocksRaycasts = true;
        ItemSlot destinationSlot = data.pointerEnter?.GetComponent<ItemSlot>(); //The OnDragEnd event will grab the actual slot from the hierarchy
        RectTransform destinationRectTransform = destinationSlot?.GetComponent<RectTransform>();

        if (destinationRectTransform != null)
        {
            //getting the items from the slots, GameObjects and RectTransform
            GameObject destinationGameObject = destinationRectTransform.gameObject;
            Item sourceItem = GetComponent<ItemDisplay>().Item;
            Item destinationItem = destinationSlot.Item;

            Inventory destinationInventory = destinationSlot.RootInventory;
            var equipmentSystem = destinationInventory.GetComponent<EquipmentSystem>();
            var craftSystem = destinationInventory.GetComponent<CraftSystem>();

            if (ReferenceEquals(sourceSlot, destinationSlot))
            {
                return false;  //Do nothing if the source and destination are the same item.
            }

            if (destinationItem == null)
            {
                if (equipmentSystem != null)
                {
                    //Make sure the item slots are of the same type before inserting
                    //kick out if not
                    if (sourceItem.Type != equipmentSystem.itemTypeOfSlots[destinationSlot.SlotNum])
                    {
                        sourceInventory[sourceSlot.SlotNum] = sourceItem;
                        return false;
                    }
                }
                //insert immediately
                destinationInventory[destinationSlot.SlotNum] = sourceItem;
                return false;
            }
            else
            {
                //dragging in a general Inventory (and Hotbar)
                if (destinationInventory.GetComponent<EquipmentSystem>() == null)
                {
                    if (sourceItem.ID == destinationItem.ID)
                    {
                        //Add or dump as much as possible of the item to the destination
                        int remainingItems = sourceItem.Quantity;
                        int increase = Mathf.Min(destinationItem.MaxStack - destinationItem.Quantity, remainingItems);
                        destinationItem.Quantity += increase;
                        remainingItems -= increase;
                        if (craftSystem != null) craftSystem.ListWithItem();
                        if (remainingItems > 0)
                        {
                            sourceItem.Quantity = remainingItems;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //Swap the items
                        Swap(destinationSlot, destinationInventory);
                    }
                }

                //dragging into a equipmentsystem/charactersystem
                if (equipmentSystem != null)
                {
                    if (sourceItem.Type == destinationItem.Type)
                    {
                        //swap
                        Swap(destinationSlot, destinationInventory);
                        if (!ReferenceEquals(sourceInventory, destinationInventory))
                        {
                            destinationInventory.UnEquipItem(destinationItem);
                            destinationInventory.EquipItem(sourceItem);
                        }
                    }
                    else
                    {
                        //kick out and send the item home
                        sourceInventory[sourceSlot.SlotNum] = sourceItem;
                    }
                }
            }
        }
        else
        {
            sourceInventory.DropItem(GetComponent<ItemDisplay>().Item);
        }
        return false;
    }

    private void Swap(ItemSlot destinationSlot, Inventory destinationInventory)
    {
        var source = sourceInventory[sourceSlot.SlotNum];
        sourceInventory[sourceSlot.SlotNum] = destinationInventory[destinationSlot.SlotNum];
        destinationInventory[sourceSlot.SlotNum] = source;
    }
}
