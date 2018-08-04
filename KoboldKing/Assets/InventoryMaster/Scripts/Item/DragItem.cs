using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//TODO:  Refactor for new Inventory Format.
public class DragItem : MonoBehaviour, IDragHandler, IPointerDownHandler, IEndDragHandler
{
    private Vector2 pointerOffset;
    private RectTransform rectTransform;
    private RectTransform draggedItemRectTransform;
    private CanvasGroup canvasGroup;
    private GameObject oldSlot;
    private Inventory inventory;
    private Transform draggedItemBox;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        draggedItemBox = GameObject.FindGameObjectWithTag("DraggingItem").transform;
        draggedItemRectTransform = draggedItemBox.GetComponent<RectTransform>();
        inventory = transform.parent.GetComponent<ItemSlot>().RootInventory;
    }


    public void OnDrag(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left && transform.parent.GetComponent<CraftResultSlot>() == null)
        {

            canvasGroup.blocksRaycasts = false;
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(draggedItemRectTransform, Input.mousePosition, data.pressEventCamera, out localPointerPosition))
            {
                Debug.Log(localPointerPosition);
                rectTransform.localPosition = localPointerPosition - pointerOffset;
                //ItemSlot duplication = oldSlot.GetComponent<ItemSlot>().Duplication;
                //if (duplication != null)
                //    Destroy(duplication);
            }
        }

        //inventory.OnUpdateItemList();
    }



    public void OnPointerDown(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out pointerOffset);
            oldSlot = transform.parent.gameObject;
            rectTransform.SetAsLastSibling();
            transform.SetParent(draggedItemBox);
        }
    }

    public void CreateDuplication(GameObject ItemToDup)
    {
        ItemSlot.CreateDuplication(ItemToDup.GetComponent<ItemSlot>());
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            canvasGroup.blocksRaycasts = true;
            Transform newSlot = null;
            if (data.pointerEnter != null)
                newSlot = data.pointerEnter.transform;

            if (newSlot != null)
            {
                //getting the items from the slots, GameObjects and RectTransform
                GameObject firstItemGameObject = this.gameObject;
                GameObject secondItemGameObject = newSlot.parent.gameObject;
                RectTransform firstItemRectTransform = this.gameObject.GetComponent<RectTransform>();
                RectTransform secondItemRectTransform = newSlot.parent.GetComponent<RectTransform>();
                Item firstItem = transform.GetComponent<ItemDisplay>().Item;
                Item secondItem = new Item();
                if (newSlot.parent.GetComponent<ItemOnObject>() != null)
                    secondItem = newSlot.parent.GetComponent<ItemOnObject>().Item;

                //get some informations about the two items
                bool sameItem = firstItem.Name == secondItem.Name;
                bool sameItemRerferenced = firstItem.Equals(secondItem);
                bool secondItemStack = false;
                bool firstItemStack = false;
                if (sameItem)
                {
                    firstItemStack = firstItem.Quantity < firstItem.MaxStack;
                    secondItemStack = secondItem.Quantity < secondItem.MaxStack;
                }

                GameObject Inventory = secondItemRectTransform.parent.gameObject;
                if (Inventory.tag == "Slot")
                    Inventory = secondItemRectTransform.parent.parent.parent.gameObject;

                if (Inventory.tag.Equals("Slot"))
                    Inventory = Inventory.transform.parent.parent.gameObject;

                //dragging in an Inventory      
                if (Inventory.GetComponent<Hotbar>() == null && Inventory.GetComponent<EquipmentSystem>() == null && Inventory.GetComponent<CraftSystem>() == null)
                {
                    //you cannot attach items to the resultslot of the craftsystem
                    if (newSlot.transform.parent.tag == "ResultSlot" || newSlot.transform.tag == "ResultSlot" || newSlot.transform.parent.parent.tag == "ResultSlot")
                    {
                        firstItemGameObject.transform.SetParent(oldSlot.transform);
                        firstItemRectTransform.localPosition = Vector3.zero;
                    }
                    else
                    {
                        int newSlotChildCount = newSlot.transform.parent.childCount;
                        bool isOnSlot = newSlot.transform.parent.GetChild(0).tag == "ItemIcon";
                        //dragging on a slot where allready is an item on
                        if (newSlotChildCount != 0 && isOnSlot)
                        {
                            //check if the items fits into the other item
                            bool fitsIntoStack = false;
                            if (sameItem)
                                fitsIntoStack = (firstItem.Quantity + secondItem.Quantity) <= firstItem.MaxStack;
                            //if the item is stackable checking if the firstitemstack and seconditemstack is not full and check if they are the same items

                            if (inventory.stackable && sameItem && firstItemStack && secondItemStack)
                            {
                                //if the item does not fit into the other item
                                if (fitsIntoStack && !sameItemRerferenced)
                                {
                                    secondItem.Quantity = firstItem.Quantity + secondItem.Quantity;
                                    secondItemGameObject.transform.SetParent(newSlot.parent.parent);
                                    Destroy(firstItemGameObject);
                                    secondItemRectTransform.localPosition = Vector3.zero;
                                }

                                else
                                {
                                    //creates the rest of the item
                                    int rest = (firstItem.Quantity + secondItem.Quantity) % firstItem.MaxStack;

                                    //fill up the other stack and adds the rest to the other stack 
                                    if (!fitsIntoStack && rest > 0)
                                    {
                                        firstItem.Quantity = firstItem.MaxStack;
                                        secondItem.Quantity = rest;

                                        firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                        secondItemGameObject.transform.SetParent(oldSlot.transform);

                                        firstItemRectTransform.localPosition = Vector3.zero;
                                        secondItemRectTransform.localPosition = Vector3.zero;
                                    }
                                }

                            }
                            //if does not fit
                            else
                            {
                                //creates the rest of the item
                                int rest = 0;
                                if (sameItem)
                                    rest = (firstItem.Quantity + secondItem.Quantity) % firstItem.MaxStack;

                                //fill up the other stack and adds the rest to the other stack 
                                if (!fitsIntoStack && rest > 0)
                                {
                                    secondItem.Quantity = firstItem.MaxStack;
                                    firstItem.Quantity = rest;

                                    firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                    secondItemGameObject.transform.SetParent(oldSlot.transform);

                                    firstItemRectTransform.localPosition = Vector3.zero;
                                    secondItemRectTransform.localPosition = Vector3.zero;
                                }
                                //if they are different items or the stack is full, they get swapped
                                else if (!fitsIntoStack && rest == 0)
                                {
                                    //if you are dragging an item from equipmentsystem to the inventory and try to swap it with the same itemtype
                                    if (oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() != null && firstItem.Type == secondItem.Type)
                                    {
                                        newSlot.transform.parent.parent.parent.parent.GetComponent<Inventory>().UnEquipItem(firstItem);
                                        oldSlot.transform.parent.parent.GetComponent<Inventory>().EquipItem(secondItem);

                                        firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                        secondItemGameObject.transform.SetParent(oldSlot.transform);
                                        secondItemRectTransform.localPosition = Vector3.zero;
                                        firstItemRectTransform.localPosition = Vector3.zero;

                                        if (secondItemGameObject.GetComponent<ItemSlot>().Duplication != null)
                                            Destroy(secondItemGameObject.GetComponent<ItemSlot>().Duplication);

                                    }
                                    //if you are dragging an item from the equipmentsystem to the inventory and they are not from the same itemtype they do not get swapped.                                    
                                    else if (oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() != null && firstItem.Type != secondItem.Type)
                                    {
                                        firstItemGameObject.transform.SetParent(oldSlot.transform);
                                        firstItemRectTransform.localPosition = Vector3.zero;
                                    }
                                    //swapping for the rest of the inventorys
                                    else if (oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() == null)
                                    {
                                        firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                        secondItemGameObject.transform.SetParent(oldSlot.transform);
                                        secondItemRectTransform.localPosition = Vector3.zero;
                                        firstItemRectTransform.localPosition = Vector3.zero;
                                    }
                                }

                            }

                        }

                        //empty slot
                        else
                        {
                            if (newSlot.tag != "Slot" && newSlot.tag != "ItemIcon")
                            {
                                firstItemGameObject.transform.SetParent(oldSlot.transform);
                                firstItemRectTransform.localPosition = Vector3.zero;
                            }
                            else
                            {
                                firstItemGameObject.transform.SetParent(newSlot.transform);
                                firstItemRectTransform.localPosition = Vector3.zero;

                                if (newSlot.transform.parent.parent.GetComponent<EquipmentSystem>() == null && oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() != null)
                                    oldSlot.transform.parent.parent.GetComponent<Inventory>().UnEquipItem(firstItem);
                            }
                        }
                    }
                }



                //dragging into a Hotbar            
                if (Inventory.GetComponent<Hotbar>() != null)
                {
                    int newSlotChildCount = newSlot.transform.parent.childCount;
                    bool isOnSlot = newSlot.transform.parent.GetChild(0).tag == "ItemIcon";
                    //dragging on a slot where allready is an item on
                    if (newSlotChildCount != 0 && isOnSlot)
                    {
                        //check if the items fits into the other item
                        bool fitsIntoStack = false;
                        if (sameItem)
                            fitsIntoStack = (firstItem.Quantity + secondItem.Quantity) <= firstItem.MaxStack;
                        //if the item is stackable checking if the firstitemstack and seconditemstack is not full and check if they are the same items

                        if (inventory.stackable && sameItem && firstItemStack && secondItemStack)
                        {
                            //if the item does not fit into the other item
                            if (fitsIntoStack && !sameItemRerferenced)
                            {
                                secondItem.Quantity = firstItem.Quantity + secondItem.Quantity;
                                secondItemGameObject.transform.SetParent(newSlot.parent.parent);
                                Destroy(firstItemGameObject);
                                secondItemRectTransform.localPosition = Vector3.zero;
                            }

                            else
                            {
                                //creates the rest of the item
                                int rest = (firstItem.Quantity + secondItem.Quantity) % firstItem.MaxStack;

                                //fill up the other stack and adds the rest to the other stack 
                                if (!fitsIntoStack && rest > 0)
                                {
                                    firstItem.Quantity = firstItem.MaxStack;
                                    secondItem.Quantity = rest;

                                    firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                    secondItemGameObject.transform.SetParent(oldSlot.transform);

                                    firstItemRectTransform.localPosition = Vector3.zero;
                                    secondItemRectTransform.localPosition = Vector3.zero;

                                    CreateDuplication(this.gameObject);
                                    secondItemGameObject.GetComponent<ItemSlot>().Item = secondItem;

                                }
                            }

                        }
                        //if does not fit
                        else
                        {
                            //creates the rest of the item
                            int rest = 0;
                            if (sameItem)
                                rest = (firstItem.Quantity + secondItem.Quantity) % firstItem.MaxStack;

                            bool fromEquip = oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() != null;

                            //fill up the other stack and adds the rest to the other stack 
                            if (!fitsIntoStack && rest > 0)
                            {
                                secondItem.Quantity = firstItem.MaxStack;
                                firstItem.Quantity = rest;

                                CreateDuplication(this.gameObject);

                                firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                secondItemGameObject.transform.SetParent(oldSlot.transform);

                                firstItemRectTransform.localPosition = Vector3.zero;
                                secondItemRectTransform.localPosition = Vector3.zero;

                            }
                            //if they are different items or the stack is full, they get swapped
                            else if (!fitsIntoStack && rest == 0)
                            {
                                if (!fromEquip)
                                {
                                    firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                    secondItemGameObject.transform.SetParent(oldSlot.transform);
                                    secondItemRectTransform.localPosition = Vector3.zero;
                                    firstItemRectTransform.localPosition = Vector3.zero;

                                    if (oldSlot.transform.parent.parent.gameObject.Equals(GameObject.FindGameObjectWithTag("MainInventory")))
                                    {
                                        Destroy(secondItemGameObject.GetComponent<ItemSlot>().Duplication);
                                        CreateDuplication(firstItemGameObject);
                                    }
                                    else
                                    {
                                        CreateDuplication(firstItemGameObject);
                                    }
                                }
                                else
                                {
                                    firstItemGameObject.transform.SetParent(oldSlot.transform);
                                    firstItemRectTransform.localPosition = Vector3.zero;
                                }
                            }

                        }
                    }
                    //empty slot
                    else
                    {
                        if (newSlot.tag != "Slot" && newSlot.tag != "ItemIcon")
                        {
                            firstItemGameObject.transform.SetParent(oldSlot.transform);
                            firstItemRectTransform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            firstItemGameObject.transform.SetParent(newSlot.transform);
                            firstItemRectTransform.localPosition = Vector3.zero;

                            if (newSlot.transform.parent.parent.GetComponent<EquipmentSystem>() == null && oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() != null)
                                oldSlot.transform.parent.parent.GetComponent<Inventory>().UnEquipItem(firstItem);
                            CreateDuplication(firstItemGameObject);
                        }
                    }

                }


                //dragging into a equipmentsystem/charactersystem
                if (Inventory.GetComponent<EquipmentSystem>() != null)
                {
                    ItemType[] itemTypeOfSlots = GameObject.FindGameObjectWithTag("EquipmentSystem").GetComponent<EquipmentSystem>().itemTypeOfSlots;
                    int newSlotChildCount = newSlot.transform.parent.childCount;
                    bool isOnSlot = newSlot.transform.parent.GetChild(0).tag == "ItemIcon";
                    bool sameItemType = firstItem.Type == secondItem.Type;
                    bool fromHot = oldSlot.transform.parent.parent.GetComponent<Hotbar>() != null;

                    //dragging on a slot where allready is an item on
                    if (newSlotChildCount != 0 && isOnSlot)
                    {
                        //items getting swapped if they are the same itemtype
                        if (sameItemType && !sameItemRerferenced) //
                        {
                            Transform temp1 = secondItemGameObject.transform.parent.parent.parent;
                            Transform temp2 = oldSlot.transform.parent.parent;

                            firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                            secondItemGameObject.transform.SetParent(oldSlot.transform);
                            secondItemRectTransform.localPosition = Vector3.zero;
                            firstItemRectTransform.localPosition = Vector3.zero;

                            if (!temp1.Equals(temp2))
                            {
                                if (firstItem.Type == ItemType.UFPS_Weapon)
                                {
                                    Inventory.GetComponent<Inventory>().UnEquipItem(secondItem);
                                    Inventory.GetComponent<Inventory>().EquipItem(firstItem);
                                }
                                else
                                {
                                    Inventory.GetComponent<Inventory>().EquipItem(firstItem);
                                    if (secondItem.Type != ItemType.Backpack)
                                        Inventory.GetComponent<Inventory>().UnEquipItem(secondItem);
                                }
                            }

                            if (fromHot)
                                CreateDuplication(secondItemGameObject);

                        }
                        //if they are not from the same Itemtype the dragged one getting placed back
                        else
                        {
                            firstItemGameObject.transform.SetParent(oldSlot.transform);
                            firstItemRectTransform.localPosition = Vector3.zero;

                            if (fromHot)
                                CreateDuplication(firstItemGameObject);
                        }

                    }
                    //if the slot is empty
                    else
                    {
                        for (int i = 0; i < newSlot.parent.childCount; i++)
                        {
                            if (newSlot.Equals(newSlot.parent.GetChild(i)))
                            {
                                //checking if it is the right slot for the item
                                if (itemTypeOfSlots[i] == transform.GetComponent<ItemOnObject>().Item.Type)
                                {
                                    transform.SetParent(newSlot);
                                    rectTransform.localPosition = Vector3.zero;

                                    if (!oldSlot.transform.parent.parent.Equals(newSlot.transform.parent.parent))
                                        Inventory.GetComponent<Inventory>().EquipItem(firstItem);

                                }
                                //else it get back to the old slot
                                else
                                {
                                    transform.SetParent(oldSlot.transform);
                                    rectTransform.localPosition = Vector3.zero;
                                    if (fromHot)
                                        CreateDuplication(firstItemGameObject);
                                }
                            }
                        }
                    }

                }

                if (Inventory.GetComponent<CraftSystem>() != null)
                {
                    CraftSystem cS = Inventory.GetComponent<CraftSystem>();
                    int newSlotChildCount = newSlot.transform.parent.childCount;


                    bool isOnSlot = newSlot.transform.parent.GetChild(0).tag == "ItemIcon";
                    //dragging on a slot where allready is an item on
                    if (newSlotChildCount != 0 && isOnSlot)
                    {
                        //check if the items fits into the other item
                        bool fitsIntoStack = false;
                        if (sameItem)
                            fitsIntoStack = (firstItem.Quantity + secondItem.Quantity) <= firstItem.MaxStack;
                        //if the item is stackable checking if the firstitemstack and seconditemstack is not full and check if they are the same items

                        if (inventory.stackable && sameItem && firstItemStack && secondItemStack)
                        {
                            //if the item does not fit into the other item
                            if (fitsIntoStack && !sameItemRerferenced)
                            {
                                secondItem.Quantity = firstItem.Quantity + secondItem.Quantity;
                                secondItemGameObject.transform.SetParent(newSlot.parent.parent);
                                Destroy(firstItemGameObject);
                                secondItemRectTransform.localPosition = Vector3.zero;
                                cS.ListWithItem();
                            }

                            else
                            {
                                //creates the rest of the item
                                int rest = (firstItem.Quantity + secondItem.Quantity) % firstItem.MaxStack;

                                //fill up the other stack and adds the rest to the other stack 
                                if (!fitsIntoStack && rest > 0)
                                {
                                    firstItem.Quantity = firstItem.MaxStack;
                                    secondItem.Quantity = rest;

                                    firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                    secondItemGameObject.transform.SetParent(oldSlot.transform);

                                    firstItemRectTransform.localPosition = Vector3.zero;
                                    secondItemRectTransform.localPosition = Vector3.zero;
                                    cS.ListWithItem();


                                }
                            }

                        }
                        //if does not fit
                        else
                        {
                            //creates the rest of the item
                            int rest = 0;
                            if (sameItem)
                                rest = (firstItem.Quantity + secondItem.Quantity) % firstItem.MaxStack;

                            //fill up the other stack and adds the rest to the other stack 
                            if (!fitsIntoStack && rest > 0)
                            {
                                secondItem.Quantity = firstItem.MaxStack;
                                firstItem.Quantity = rest;

                                firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                secondItemGameObject.transform.SetParent(oldSlot.transform);

                                firstItemRectTransform.localPosition = Vector3.zero;
                                secondItemRectTransform.localPosition = Vector3.zero;
                                cS.ListWithItem();

                            }
                            //if they are different items or the stack is full, they get swapped
                            else if (!fitsIntoStack && rest == 0)
                            {
                                //if you are dragging an item from equipmentsystem to the inventory and try to swap it with the same itemtype
                                if (oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() != null && firstItem.Type == secondItem.Type)
                                {

                                    firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                    secondItemGameObject.transform.SetParent(oldSlot.transform);
                                    secondItemRectTransform.localPosition = Vector3.zero;
                                    firstItemRectTransform.localPosition = Vector3.zero;

                                    oldSlot.transform.parent.parent.GetComponent<Inventory>().EquipItem(secondItem);
                                    newSlot.transform.parent.parent.parent.parent.GetComponent<Inventory>().UnEquipItem(firstItem);
                                }
                                //if you are dragging an item from the equipmentsystem to the inventory and they are not from the same itemtype they do not get swapped.                                    
                                else if (oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() != null && firstItem.Type != secondItem.Type)
                                {
                                    firstItemGameObject.transform.SetParent(oldSlot.transform);
                                    firstItemRectTransform.localPosition = Vector3.zero;
                                }
                                //swapping for the rest of the inventorys
                                else if (oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() == null)
                                {
                                    firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                    secondItemGameObject.transform.SetParent(oldSlot.transform);
                                    secondItemRectTransform.localPosition = Vector3.zero;
                                    firstItemRectTransform.localPosition = Vector3.zero;
                                }
                            }

                        }
                    }
                    else
                    {
                        if (newSlot.tag != "Slot" && newSlot.tag != "ItemIcon")
                        {
                            firstItemGameObject.transform.SetParent(oldSlot.transform);
                            firstItemRectTransform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            firstItemGameObject.transform.SetParent(newSlot.transform);
                            firstItemRectTransform.localPosition = Vector3.zero;

                            if (newSlot.transform.parent.parent.GetComponent<EquipmentSystem>() == null && oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() != null)
                                oldSlot.transform.parent.parent.GetComponent<Inventory>().UnEquipItem(firstItem);
                        }
                    }

                }


            }

            else
            {
                inventory.DropItem(GetComponent<ItemDisplay>().Item);
                Destroy(gameObject);
            }
        }
    }
}
