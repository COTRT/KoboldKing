using Assets.Scripts.Data;
using Assets.Scripts.Events;
using Assets.Scripts.Managers;
using Assets.Scripts.Quests;
using UnityEngine;

[Manager]
public class GiveQuest : ManagerBase
{
    protected override void StartManager(DataService dataService)
    {
        Messenger<string[],GameObject>.AddListener(DialogueAction.GiveQuest, HandleGiveQuestAction);
    }
    public void HandleGiveQuestAction(string[] arguments,GameObject speaker)
    {
        foreach(var questname in arguments)
        {
            var quest = QuestManager.Instance.Add(questname);
            var questGiver = speaker.GetComponent<IQuestGiver>();
            if (questGiver != null)
            {
                questGiver.QuestAssigned(quest);
            }
        }
    }
}

