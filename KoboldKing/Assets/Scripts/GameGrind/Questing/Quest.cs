﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GGQuest : MonoBehaviour
{
    public int Level { get; set; }

    public List<GGGoal> Goals { get; set; }
    public string QuestName { get; set; }
    public string Description { get; set; }
    public int ExperienceReward { get; set; }
    public Item ItemReward { get; set; }
    public bool Completed { get; set; }

    public void CheckGoals()
    {
        Completed = Goals.All(g => g.Completed);
    }

    public void GiveReward()
    {
        if (ItemReward != null)
            InventoryController.Instance.GiveItem(ItemReward);
    }
}
