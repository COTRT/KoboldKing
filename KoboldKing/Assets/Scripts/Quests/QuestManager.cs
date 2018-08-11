using Assets.Scripts.Misc;
using System.Collections.Generic;

namespace Assets.Scripts.Quests
{
    public class QuestManager: JsonFileReader<Dictionary<string,QuestSaveData>>
    {
        List<Quest> ActiveQuests { get; set; } = new List<Quest>();
        private static Quest Create(string questName) => Instance.MasterJsonDict[questName].ToQuest();
        public Quest Add(string questName) {
            Quest quest = Create(questName);
            ActiveQuests.Add(quest);
            return quest;
        }

    }
}
