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
        craftSystem = transform.parent.GetComponent<CraftSystem>();

        GetComponent<RectTransform>().localPosition = Vector3.zero;
        Inventory mainInventory = PlayerInventory.Instance.mainInventory;
        UpdateDisplaySettings(true,mainInventory.positionNumberX, mainInventory.positionNumberY, false);
        gameObject.SetActive(false);
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
        if (mi.AddItemToInventory(Item.ID, Item.Quantity,false) != 0)
        {
            CraftSystem cS = PlayerInventory.Instance.craftSystem;
            cS.RemoveItemIngredients(Item); //TODO:  Change CraftSystem to store Blueprint in CraftResultSlot, and pass that in here, to make things significantly less stupid in that department4
            temp = 0;
            tooltip.deactivateTooltip();
        }
    }

}
