using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class GGDialogue
{
    public string Conversation { get; set; }
    public GGDialogue(string _Conversation)
    {
        this.Conversation = _Conversation;
    }
    public enum Dialogues { Name, Quest }
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public Dialogues Talk { get; set; }

    public string Name;
    public string Introduction;
    public string Response;
    public string Conclusion;
    public string Quest { get; set; }

    [Newtonsoft.Json.JsonConstructor]
    public GGDialogue(string _Name, string _Introduction, string _Response, string _Conclusion, string _Quest, Dialogues _talk)
    {
        this.Name = _Name;
        this.Introduction = _Introduction;
        this.Response = _Response;
        this.Conclusion = _Conclusion;
        this.Quest = _Quest;
        this.Talk = _talk;
    }
}
//public enum 
//Types { Weapon, Consumable, Quest }


//public List<BaseStat> Stats { get; set; }
//public string ObjectSlug { get; set; } // What the heck -- named object slug? (will be renamed in the future)
//public string Description;

//[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
//public ItemTypes ItemTypeX { get; set; }


//public string ActionName;
//public string ItemName;
//public bool ItemModifier;

//public ItemX(List<BaseStat> _Stats, string _ObjectSlug)
//{
//    this.Stats = _Stats;
//    this.ObjectSlug = _ObjectSlug;
//}
//[Newtonsoft.Json.JsonConstructor]
//public ItemX(List<BaseStat> _Stats, string _ObjectSlug, string _Description, ItemTypes _itemType, string _ActionName, string _ItemName, bool _ItemModifier)
//{
//    this.Stats = _Stats;
//    this.ObjectSlug = _ObjectSlug;
//    this.Description = _Description;
//    this.ItemTypeX = _itemType;
//    this.ActionName = _ActionName;
//    this.ItemName = _ItemName;
//    this.ItemModifier = _ItemModifier;
//}
