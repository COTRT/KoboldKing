using Assets.Scripts.Misc;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Quests
{
    public class QuestManager : JsonFileReader<Dictionary<string, QuestSaveData>>
    {
        public static QuestManager Instance { get; set; }
        List<Quest> ActiveQuests { get; set; } = new List<Quest>();
        List<Quest> CompletedQuests { get; set; } = new List<Quest>();
        public Quest Add(string questName) {
            if (!MasterJsonDict.ContainsKey(questName))
            {
                throw new ArgumentException("The Requested Quest Name ('"+questName+"') could not be found in the Quests JSON file.  Have you defined the quest yet?");
            }
            Quest quest = MasterJsonDict[questName].ToQuest();
            ActiveQuests.Add(quest);
            return quest;
        }
        private new void Awake()
        {
            base.Awake();
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
        public Quest[] Evaluate()
        {
            var newlyCompleteQuests = new List<Quest>();
            foreach(var quest in ActiveQuests)
            {
                if (quest.Evaluate())
                {
                    ActiveQuests.Remove(quest);
                    CompletedQuests.Add(quest);
                    newlyCompleteQuests.Add(quest);
                }
            }
            return newlyCompleteQuests.ToArray();
        }
    }
}
