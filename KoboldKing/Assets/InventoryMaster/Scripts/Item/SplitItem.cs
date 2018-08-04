using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//TODO:  Refactor to use ItemSlot.RootInventory
public class SplitItem : MonoBehaviour, IPointerDownHandler
{     //splitting an Item

    private bool pressingButtonToSplit;             //bool for pressing a item to split it
    public Inventory inv;                          //inventory script  
    static InputManager inputManagerDatabase;

    void Update()
    {
        if (Input.GetKeyDown(inputManagerDatabase.SplitItem))                     //if we press right controll the ....
            pressingButtonToSplit = true;                               //getting changed to true 
        if (Input.GetKeyUp(inputManagerDatabase.SplitItem))
            pressingButtonToSplit = false;                              //or false

    }

    void Start()
    {
        inputManagerDatabase = (InputManager)Resources.Load("InputManager");
    }

    //TODO refactor with array inventory system
    public void OnPointerDown(PointerEventData data)                    //splitting the item now
    {
        inv = transform.parent.parent.parent.GetComponent<Inventory>();
        if (transform.parent.parent.parent.GetComponent<Hotbar>() == null && data.button == PointerEventData.InputButton.Left && pressingButtonToSplit && inv.stackable && (inv.ItemsInInventory.Length < (inv.height * inv.width))) //if you press leftclick and and keycode
        {
            ItemOnObject itemOnObject = GetComponent<ItemOnObject>();                                                   //we take the ItemOnObject script of the item in the slot

            if (itemOnObject.Item.Quantity > 1)                                                                         //we split the item only when we have more than 1 in the stack
            {
                int splitPart = itemOnObject.Item.Quantity;                                                           //we take the value and store it in there
                itemOnObject.Item.Quantity = (int)itemOnObject.Item.Quantity / 2;                                     //calculate the new value for the splitted item
                splitPart = splitPart - itemOnObject.Item.Quantity;                                                   //take the different

                inv.AddItemToInventory(itemOnObject.Item.ID, splitPart);                                            //and add a new item to the inventory
            }
        }
    }
}
