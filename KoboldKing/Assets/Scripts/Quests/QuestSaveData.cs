using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Quests
{
    public class QuestSaveData
    {
        public string Name { get; set; }
        public List<Dictionary<string,string>> Goals { get; set; }
        public List<Dictionary<string,string>> Rewards { get; set; }
        public string Description { get; set; }
        public Quest ToQuest()
        {
            Quest quest = new Quest
            {
                QuestName = Name,
                Description = Description
            };
            foreach (var goalSettings in Goals)
            {
                quest.Goals.Add(CreateWithSettings<Goal>(goalSettings));
            }
            foreach(var rewardSettings in Rewards)
            {
                quest.Rewards.Add(CreateWithSettings<Reward>(rewardSettings));
            }
            return quest;
        }

        private T CreateWithSettings<T>(Dictionary<string,string> settings)
        {
            string typeName = typeof(T).Name;
            if (!settings.ContainsKey("Type"))
            {
                throw new ArgumentException($"The quest:  {Name} has a {typeName} missing a Type argument.  All {typeName}s must specify a Type.");
            }
            Type type = Type.GetType(settings["Type"]);
            if (type == null) throw new ArgumentException($"The quest:  {Name} defines a {typeName} with a Type argument that does could not be found");
            settings.Remove("Type");
            return (T)Activator.CreateInstance(type, settings);
        }
    }
}
