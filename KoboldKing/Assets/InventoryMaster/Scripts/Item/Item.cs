using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

[System.Serializable]
public class Item : INotifyPropertyChanged
{
    [SerializeField]
    public List<ItemAttribute> itemAttributes = new List<ItemAttribute>();

    //Thank you, Mr. Sun of Miracle Salad, for that Sequencer of yours.
    private string _itemName;
    public string ItemName
    {
        get
        {
            return _itemName;
        }

        set
        {
            _itemName = value;
            OnPropertyChanged("ItemName");
        }
    }

    private int _itemID;
    public int ItemID
    {
        get
        {
            return _itemID;
        }

        set
        {
            _itemID = value;
            OnPropertyChanged("ItemID");
        }
    }

    private string _itemDesc;
    public string ItemDesc
    {
        get
        {
            return _itemDesc;
        }

        set
        {
            _itemDesc = value;
            OnPropertyChanged("ItemDesc");
        }
    }

    private Sprite _itemIcon;
    public Sprite ItemIcon
    {
        get
        {
            return _itemIcon;
        }

        set
        {
            _itemIcon = value;
            OnPropertyChanged("ItemIcon");
        }
    }

    private GameObject _itemModel;
    public GameObject ItemModel
    {
        get
        {
            return _itemModel;
        }

        set
        {
            _itemModel = value;
            OnPropertyChanged("ItemModel");
        }
    }

    private int _itemValue;
    public int ItemValue
    {
        get
        {
            return _itemValue;
        }

        set
        {
            _itemValue = value;
            OnPropertyChanged("ItemValue");
        }
    }

    private ItemType _itemType;
    public ItemType ItemType
    {
        get
        {
            return _itemType;
        }

        set
        {
            _itemType = value;
            OnPropertyChanged("ItemType");
        }
    }

    private float _itemWeight;
    public float ItemWeight
    {
        get
        {
            return _itemWeight;
        }

        set
        {
            _itemWeight = value;
            OnPropertyChanged("ItemWeight");
        }
    }

    private int _maxStack;
    public int MaxStack
    {
        get
        {
            return _maxStack;
        }

        set
        {
            _maxStack = value;
            OnPropertyChanged("MaxStack");
        }
    }

    private int _indexItemInList;
    public int IndexItemInList
    {
        get
        {
            return _indexItemInList;
        }

        set
        {
            _indexItemInList = value;
            OnPropertyChanged("IndexItemInList");
        }
    }

    private int _rarity;
    public int Rarity
    {
        get
        {
            return _rarity;
        }

        set
        {
            _rarity = value;
            OnPropertyChanged("Rarity");
        }
    }

    private bool _empty;
    public bool Empty
    {
        get
        {
            return _empty;
        }

        set
        {
            _empty = value;
            OnPropertyChanged("Empty");
        }
    }


    public Item(){}

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


