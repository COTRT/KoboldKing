using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Aka item slot
/// </summary>
public class ItemOnObject : MonoBehaviour  //Saves the Item in the slot
{
    private Text itemCountText;  //text for the itemValue
    private Image itemImage;
    RectTransform itemTextRectTransform;

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
        UpdateDisplay();
        Item.PropertyChanged += (sender, prop) => UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (Item == null)
        {
#if UNITY_EDITOR
            DestroyImmediate(transform.GetChild(0)?.gameObject); //The editor version of this, which also happens to be a little dangerous to use.
#else
            Destroy(transform.GetChild(0)?.gameObject); //The Runtime version of this.
#endif

        }
        else
        {
            if (transform.childCount == 0) //Item is not null, but we haven't made a display for it yet.
            {
                Debug.Log("Instantiating Item");
                var itemObject = (Instantiate(Resources.Load("Prefabs/Item")) as GameObject).transform;
                itemObject.SetParent(transform);
                itemImage = itemObject.GetChild(0).GetComponent<Image>();
                itemCountText = itemObject.GetChild(1).GetComponent<Text>();
                itemTextRectTransform = itemObject.GetChild(1).GetComponent<RectTransform>();
            }

            itemImage.sprite = Item.Icon;
            if (Item.MaxStack > 1)
            {
                itemCountText.text = Item.Quantity.ToString();
                itemCountText.enabled = invStackable;
                itemTextRectTransform.localPosition = new Vector3(positionNumberX, positionNumberY, 0);
            }
            else
            {
                itemCountText.enabled = false;
            }
        }
    }

    public void UpdateDisplaySettings(bool stackable, int positionNumberX, int positionNumberY)
    {
        this.invStackable = stackable;
        this.positionNumberX = positionNumberX;
        this.positionNumberY = positionNumberY;
        UpdateDisplay();
    }
}
