using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : Goal
{
    public string ItemId { get; set; }


    public CollectionGoal(Quest quest, string itemId, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.Quest = quest;
        this.ItemId = itemId;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();
        UIEventHandler.OnItemAddedToInventory += ItemPickedUp;
    }

    void ItemPickedUp(Item item)
    {
        if (item.ObjectSlug == this.ItemId)
        {
            Debug.Log("Picked up item: " + ItemId);
            this.CurrentAmount++;
            Evaluate();
        }
    }
}
