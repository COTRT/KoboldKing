class QuestGiver : Converser, IQuestGiver
{

    private Quest quest;
    public void QuestAssigned(Quest quest)
    {
        this.quest = quest;
    }

    protected override Dialogue ChooseDialogue()
    {
        string situationKey;
        if (quest == null)
        {
            situationKey = "Quest_Assign"; //Have not yet assigned quest
        }else if (!quest.Completed)
        {
            situationKey = "Quest_Incomplete"; //Quest is still in progress
        }
        else if(!quest.HasGivenReward)
        {
            quest.GiveReward();
            situationKey = "Quest_Completing"; //Quest was just completed, now giving reward
        }
        else
        {
            situationKey = "Quest_Completed"; //Quest already completed, player apparently wants some small talk.
            //If you were so inclined, you could offer up another quest in the Quest_Completed slot and start the process over again.
        }
        return ObjectDialogues[situationKey];
    }
}

