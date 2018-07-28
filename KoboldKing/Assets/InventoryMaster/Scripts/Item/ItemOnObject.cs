using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Aka item slot
/// </summary>
public class ItemOnObject : MonoBehaviour  //Saves the Item in the slot
{
    private Text itemCountText;  //text for the itemValue
    private Image image;

    private Item item;
    bool invStackable;
    int positionNumberX;
    int positionNumberY;

    public Item Item
    {
        get
        {
            return item;
        }

        set
        {
            item = value;
            UpdateDisplay();
        }
    }

    void Start()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        itemCountText = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        if (Item == null) Item = new Item() { Quantity = 0, ID = 0, Name = "Empty" };
        Item.PropertyChanged += (sender, prop) => UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        image.sprite = Item.Icon;
        if (Item.MaxStack > 1)
        {
            RectTransform textRectTransform = transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
            itemCountText.text = Item.Quantity.ToString(); ;
            itemCountText.enabled = invStackable;
            textRectTransform.localPosition = new Vector3(positionNumberX, positionNumberY, 0);
        }
        else
        {
            itemCountText.enabled = false;
        }
    }

    public void UpdateDisplay(bool stackable, int positionNumberX, int positionNumberY)
    {
        this.invStackable = stackable;
        this.positionNumberX = positionNumberX;
        this.positionNumberY = positionNumberY;
        UpdateDisplay();
    }
}
