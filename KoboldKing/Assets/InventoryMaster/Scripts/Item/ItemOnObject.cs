using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Aka item slot
/// </summary>
public class ItemOnObject : MonoBehaviour  //Saves the Item in the slot
{
    private Text itemCountText;  //text for the itemValue
    private Image image;

    public Item Item { get; set; }

    void Start()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        itemCountText = transform.GetChild(1).GetComponent<Text>();  //get the text(itemValue GameObject) of the item
    }

    void Update()
    {
        itemCountText.text = Item.ItemValue.ToString();  //sets the itemValue         
        image.sprite = Item.ItemIcon;
    }


}
