using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Aka item slot
/// </summary>
public class ItemOnObject : MonoBehaviour  //Saves the Item in the slot
{
    private Text itemCountText;  //text for the itemValue
    private Image image;

    public Item Item;

    void Start()
    {
        if (Item == null) Item = new Item() { ItemValue=0, ItemID=0,ItemName="Empty"};
        image = transform.GetChild(0).GetComponent<Image>();
        itemCountText = transform.GetChild(1).GetComponent<Text>();  //get the text(itemValue GameObject) of the item
    }

    void Update()
    {
        itemCountText.text = Item.ItemValue.ToString();  //sets the itemValue         
        image.sprite = Item.ItemIcon;
    }


}
