using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ItemOnObject : MonoBehaviour  //Saves the Item in the slot
{
    private Text itemCountText;  //text for the itemValue
    private Image itemImage;
    private DragItem dragItem;
    RectTransform itemTextRectTransform;

    private Item item;
    public GameObject itemObject;
    private bool draggable = true;
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
            if (item == null && value != null) value.PropertyChanged += Item_PropertyChanged;
            else if (item != null && value == null) item.PropertyChanged -= Item_PropertyChanged;
            item = value;
            UpdateDisplay();
        }
    }
    
    void Item_PropertyChanged(object sender, PropertyChangedEventArgs prop)
    {
        UpdateDisplay();
    }

    void Start()
    {
        if (Item.ID == 0) Item = null; //Correct for an odd bug where the Item tends to create itself, but empty.
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (Item == null)
        {
            if(itemObject != null)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(itemObject); //The editor version of this, which also happens to be a little dangerous to use.
                }
                else
                {
                    Destroy(itemObject); //The Runtime version of this.
                }
            }
            itemObject = null;
        }
        else
        {
            if (itemObject == null) //Item is not null, but we haven't made a display for it yet.
            {
                Debug.Log("Instantiating Item");
                itemObject = Instantiate(Resources.Load("Prefabs/Item")) as GameObject;
                var ioTrans = itemObject.transform;
                ioTrans.SetParent(transform);
                ioTrans.localPosition = Vector3.zero;
                itemImage = ioTrans.GetChild(0).GetComponent<Image>();
                itemCountText = ioTrans.GetChild(1).GetComponent<Text>();
                itemTextRectTransform = ioTrans.GetChild(1).GetComponent<RectTransform>();
                dragItem = ioTrans.GetComponent<DragItem>();
                itemObject.GetComponent<ItemDisplay>().Item = Item;
            }

            itemImage.sprite = Item.Icon;
            if(dragItem!=null)dragItem.enabled = draggable;
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

    public void UpdateDisplaySettings(bool? stackable = null, int? positionNumberX = null, int? positionNumberY = null,bool? draggable = null)
    {
        //If the caller does not specify a particular setting, default to what it is now.
        this.invStackable = stackable??this.invStackable;
        this.positionNumberX = positionNumberX ?? this.positionNumberX;
        this.positionNumberY = positionNumberY ?? this.positionNumberY;
        this.draggable = draggable ?? this.draggable;
        UpdateDisplay();
    }
}
