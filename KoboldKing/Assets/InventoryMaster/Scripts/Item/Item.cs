using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

[System.Serializable]
public class Item : INotifyPropertyChanged
{
    [SerializeField]
    public List<ItemAttribute> itemAttributes = new List<ItemAttribute>();

    //Thank you, Mr. Sun of Miracle Salad, for that Sequencer of yours.
    [SerializeField]
    private string itemName;
    public string ItemName
    {
        get
        {
            return itemName;
        }

        set
        {
            itemName = value;
            OnPropertyChanged("ItemName");
        }
    }

    [SerializeField]
    private int itemID;
    public int ItemID
    {
        get
        {
            return itemID;
        }

        set
        {
            itemID = value;
            OnPropertyChanged("ItemID");
        }
    }

    [SerializeField]
    private string itemDesc;
    public string ItemDesc
    {
        get
        {
            return itemDesc;
        }

        set
        {
            itemDesc = value;
            OnPropertyChanged("ItemDesc");
        }
    }

    [SerializeField]
    private Sprite itemIcon;
    public Sprite ItemIcon
    {
        get
        {
            return itemIcon;
        }

        set
        {
            itemIcon = value;
            OnPropertyChanged("ItemIcon");
        }
    }

    [SerializeField]
    private GameObject itemModel;
    public GameObject ItemModel
    {
        get
        {
            return itemModel;
        }

        set
        {
            itemModel = value;
            OnPropertyChanged("ItemModel");
        }
    }

    [SerializeField]
    private int itemValue;
    public int ItemValue
    {
        get
        {
            return itemValue;
        }

        set
        {
            itemValue = value;
            OnPropertyChanged("ItemValue");
        }
    }

    [SerializeField]
    private ItemType itemType;
    public ItemType ItemType
    {
        get
        {
            return itemType;
        }

        set
        {
            itemType = value;
            OnPropertyChanged("ItemType");
        }
    }

    private float itemWeight;
    public float ItemWeight
    {
        get
        {
            return itemWeight;
        }

        set
        {
            itemWeight = value;
            OnPropertyChanged("ItemWeight");
        }
    }

    [SerializeField]
    private int maxStack;
    public int MaxStack
    {
        get
        {
            return maxStack;
        }

        set
        {
            maxStack = value;
            OnPropertyChanged("MaxStack");
        }
    }

    [SerializeField]
    private int indexItemInList;
    public int IndexItemInList
    {
        get
        {
            return indexItemInList;
        }

        set
        {
            indexItemInList = value;
            OnPropertyChanged("IndexItemInList");
        }
    }

    [SerializeField]
    private int rarity;
    public int Rarity
    {
        get
        {
            return rarity;
        }

        set
        {
            rarity = value;
            OnPropertyChanged("Rarity");
        }
    }

    private bool empty;
    public bool Empty
    {
        get
        {
            return empty;
        }

        set
        {
            empty = value;
            OnPropertyChanged("Empty");
        }
    }


    public Item() { }

    public Item(string name, int id, string desc, Sprite icon, GameObject model, int maxStack, ItemType type, string sendmessagetext, List<ItemAttribute> itemAttributes)                 //function to create a instance of the Item
    {
        ItemName = name;
        ItemID = id;
        ItemDesc = desc;
        ItemIcon = icon;
        ItemModel = model;
        ItemType = type;
        this.MaxStack = maxStack;
        this.itemAttributes = itemAttributes;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string name)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
    public Item getCopy()
    {
        return (Item)this.MemberwiseClone();
    }


}


