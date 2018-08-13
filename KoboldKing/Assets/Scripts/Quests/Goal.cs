using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Newtonsoft.Json;

namespace Assets.Scripts.Quests
{
    public abstract class Goal
    {
        /// <summary>
        /// Make sure to override the base Goal constructor with your own necessary settings, but call the base constructor as well (see <see cref="KillGoal"/>)
        /// </summary>
        /// <param name="arguments"></param>
        public Goal(Dictionary<string, string> arguments)
        {
            RequiredAmount = arguments.ContainsKey("Amount") ? 0 : int.Parse(arguments["Amount"]);
            Description = arguments.ContainsKey("Description")?arguments["Description"]: "";
        }

        public abstract string Description { get; set; }
        public bool Completed { get; set; }
        public abstract int CurrentAmount { get; set; }
        public abstract int RequiredAmount { get; set; }

        public bool Evaluate()
        {
            if (CurrentAmount >= RequiredAmount)
            {
                Completed = true;
            }
            return Completed;
        }
    }
}