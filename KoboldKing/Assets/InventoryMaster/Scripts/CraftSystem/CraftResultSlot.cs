using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftResultSlot : ItemSlot
{
    CraftSystem craftSystem;
    public int temp = 0;
    //Inventory inventory;


    // Use this for initialization
    void Start()
    {
        //inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        craftSystem = transform.parent.GetComponent<CraftSystem>();

        GetComponent<RectTransform>().localPosition = Vector3.zero;
        GetComponent<DragItem>().enabled = false;
        gameObject.SetActive(false);
        transform.GetChild(1).GetComponent<Text>().enabled = true;
        transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector2(PlayerInventory.Instance.mainInventory.positionNumberX, PlayerInventory.Instance.mainInventory.positionNumberY);
    }
    protected override Inventory GetInventory()
    {
        return transform.parent.parent.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (craftSystem.possibleItems.Count != 0)
        {
            Item = craftSystem.possibleItems[temp];
            gameObject.SetActive(true);
        }
        else
            gameObject.SetActive(false);

    }
    public override void ConsumeIt()
    {
        if (mi.AddItemToInventory(Item.ID, Item.Quantity) != 0)
        {
            CraftSystem cS = PlayerInventory.Instance.craftSystem;
            cS.RemoveItemIngredients(Item); //TODO:  Change CraftSystem to store Blueprint in CraftResultSlot, and pass that in here, to make things significantly less stupid in that department
            CraftResultSlot result = cS.transform.GetChild(3).GetComponent<CraftResultSlot>();
            result.temp = 0;
            tooltip.deactivateTooltip();
        }
    }

}
