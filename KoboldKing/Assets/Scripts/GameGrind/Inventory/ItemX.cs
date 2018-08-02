using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ItemX
{
    public enum ItemTypes { Weapon, Consumable, Quest }


    public List<BaseStat> Stats { get; set; }
    public string ObjectSlug { get; set; } // What the heck -- named object slug? (will be renamed in the future)
    public string Description;

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public ItemTypes ItemTypeX { get; set; }


    public string ActionName;
    public string ItemName;
    public bool ItemModifier;

    public ItemX(List<BaseStat> _Stats, string _ObjectSlug)
    {
        this.Stats = _Stats;
        this.ObjectSlug = _ObjectSlug;
    }
    [Newtonsoft.Json.JsonConstructor]
    public ItemX(List<BaseStat> _Stats, string _ObjectSlug, string _Description, ItemTypes _itemType, string _ActionName, string _ItemName, bool _ItemModifier)
    {
        this.Stats = _Stats;
        this.ObjectSlug = _ObjectSlug;
        this.Description = _Description;
        this.ItemTypeX = _itemType;
        this.ActionName = _ActionName;
        this.ItemName = _ItemName;
        this.ItemModifier = _ItemModifier;
    }

}
