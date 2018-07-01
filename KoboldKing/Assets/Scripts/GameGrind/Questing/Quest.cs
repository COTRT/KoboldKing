using System.Collections;
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


    public void CheckGoals()
    {
        Goals = new List<Goal>();
        Completed = Goals.All(x => x.Completed);
    }

    public void GiveReward()
    {
        if (ItemReward != null)
        {
            InventoryController.Instance.GiveItem(ItemReward);
        }
    }



}
