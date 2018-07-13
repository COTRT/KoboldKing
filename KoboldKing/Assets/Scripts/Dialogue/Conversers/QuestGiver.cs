class QuestGiver : Converser, IQuestGiver
{

    private Quest quest;
    public void QuestAssigned(Quest quest)
    {
        this.quest = quest;
    }

    protected override Dialogue ChooseDialogue()
    {
        string questStatus;
        if (quest == null)
        {
            questStatus = "Quest_Assign"; //Have not yet assigned quest
        }
        else if (!quest.Completed)
        {
            questStatus = "Quest_Incomplete"; //Quest is still in progress
        }
        else if (!quest.HasGivenReward)
        {
            quest.GiveReward();
            questStatus = "Quest_Reward"; //Quest was just completed, now giving reward
        }
        else
        {
            questStatus = "Quest_Completed"; //Quest already completed, player apparently wants some small talk.
            //If you were so inclined, you could offer up another quest in the Quest_Completed slot and start the process over again.
        }
        return ObjectDialogues[questStatus];
    }
}

