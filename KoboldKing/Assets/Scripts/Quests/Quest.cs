using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

namespace Assets.Scripts.Quests
{
    public class Quest
    {
        [JsonProperty(PropertyName = "Name")]
        public string QuestName { get; set; }
        public List<Goal> Goals { get; set; }
        public List<Reward> Rewards { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public bool HasGivenReward { get; set; }

        public void Awake()
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
            foreach (var reward in Rewards) reward.Apply();
            HasGivenReward = true;
        }
    }
}