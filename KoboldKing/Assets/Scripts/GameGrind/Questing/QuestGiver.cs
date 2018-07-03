using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{

    public bool AssignedQuest { get; set; }
    public bool Helped { get; set; }

#pragma warning disable CS0649
    [SerializeField] private GameObject quests;
    [SerializeField] private string questType;
#pragma warning restore CS0649

    private Quest Quest { get; set; }

    public override void Interact()
    {
        
        if (!AssignedQuest && !Helped)
        {
            base.Interact();
            AssignQuest();
        }
        else if (AssignedQuest && !Helped)
        {
            CheckQuest();
        }
        else
        {
            DialogueSystem.Instance.AddNewDialogue(
                new string[] { "Thanks for doing that thing you did for me that one time!" }, Name);
        }

    }

    void AssignQuest()
    {
        AssignedQuest = true;
        Quest = (Quest)quests.AddComponent(System.Type.GetType(questType));



    }

    void CheckQuest()
    {
        if (Quest.Completed)
        {
            Quest.GiveReward();
            Helped = true;
            AssignedQuest = false;
            DialogueSystem.Instance.AddNewDialogue(
                new string[] { "Thanks for tha! Here's your reward!", "More dialouge." }, Name);

        }
        else
        {
            DialogueSystem.Instance.AddNewDialogue(
                new string[] { "You're still in the middle of helping me.  Get back at it!" }, Name);
        }
    }
}
