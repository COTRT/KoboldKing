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
    public string Name
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
    public int ID
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
    public string Description
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
    public Sprite Icon
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
    public GameObject Model
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
    private int itemQuantity;
    public int Quantity
    {
        get
        {
            return itemQuantity;
        }

        set
        {
            itemQuantity = value;
            OnPropertyChanged("ItemValue");
        }
    }

    [SerializeField]
    private ItemType itemType;
    public ItemType Type
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
    public float Weight
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

    public Item() { }

    public Item(string name, int id, string desc, Sprite icon, GameObject model, int maxStack, ItemType type, List<ItemAttribute> itemAttributes)                 //function to create a instance of the Item
    {
        Name = name;
        ID = id;
        Description = desc;
        Icon = icon;
        Model = model;
        Type = type;
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
    public Item GetCopy()
    {
        return (Item)this.MemberwiseClone();
    }


}


