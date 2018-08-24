using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Quests.Rewards
{
    public class ExperienceReward : Reward
    {
        int Experience { get; set; }
        public ExperienceReward(Dictionary<string, string> arguments) : base(arguments)
        {
            arguments.CheckConfigContains("Experience");
            Experience = int.Parse(arguments["Experience"]);
        }

        public override void Apply()
        {
            Debug.Log($"OMG!! You just got {Experience} Experience!");
        }
    }
}
