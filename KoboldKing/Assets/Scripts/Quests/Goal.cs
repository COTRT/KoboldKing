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
        public Goal(Dictionary<string, string> arguments)
        {
            RequiredAmount = arguments.ContainsKey("Amount") ? 0 : int.Parse(arguments["Amount"]);
            Description = arguments.ContainsKey("Decscription")?arguments["Description"]: "";
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
        protected static void CheckConfigContains(Dictionary<string, string> arguments, params string[] keys)
        {
            var missingKeys = keys.Where(k => !arguments.ContainsKey(k));
            if (missingKeys.Count() > 1)
                throw new ArgumentException(
                    $"Goal {(arguments["Description"] == null ? "" : ("(with description " + arguments["Description"] + ")"))} is missing these required infos:  {string.Join(",", missingKeys)}"
                );

        }
    }
}