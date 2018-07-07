﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Quest : MonoBehaviour
{

    public List<Goal> Goals { get; set; }
    public string QuestName { get; set; }
    public string Description { get; set; }
    public int ExperienceReward { get; set; }
    public Item ItemReward { get; set; }
    public bool Completed { get; set; }
    public bool HasGivenReward { get; set; }

    public void Start()
    {
        Goals = new List<Goal>();
        HasGivenReward = false;
    }
    public void CheckGoals()
    {
        
        Completed = Goals.All(x => x.Completed);
    }

    public void GiveReward()
    {
        if (ItemReward != null)
        {
            InventoryController.Instance.GiveItem(ItemReward);
        }
        HasGivenReward = true;
    }



}
