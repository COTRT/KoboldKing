using Assets.Scripts.Quests;
using System.Collections.Generic;
using UnityEngine;

public class ItemReward : Reward
{
    public string ItemId { get; set; }
    public ItemReward(Dictionary<string, string> arguments) : base(arguments)
    {
        arguments.CheckConfigContains("Item");
        ItemId = arguments["Item"];
    }

    public override void Apply()
    {
        Debug.Log("Hey, you just added an item with Id:  " + ItemId + " to your inventory!  Now all that's left is to have an inventory...");
    }
}

