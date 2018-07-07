using Assets.Scripts.Data;
using Assets.Scripts.Events;
using Assets.Scripts.Managers;
using UnityEngine;

[Manager]
public class GiveQuest : ManagerBase
{
    [SerializeField]private Transform _quests;

    public Transform Quests
    {
        get
        {
            return _quests ?? (_quests = transform.Find("Quests"));
        }

        set
        {
            _quests = value;
        }
    }

    protected override void StartManager(DataService dataService)
    {
        Messenger<string[],GameObject>.AddListener(DialogueAction.GiveQuest, HandleGiveQuestAction);
    }
    public void HandleGiveQuestAction(string[] arguments,GameObject speaker)
    {
        foreach(var questname in arguments)
        {
            var quest = (Quest)Quests.gameObject.AddComponent(System.Type.GetType(questname));
            var questGiver = speaker.GetComponent<IQuestGiver>();
            if (questGiver != null)
            {
                questGiver.QuestAssigned(quest);
            }
        }
    }
}

