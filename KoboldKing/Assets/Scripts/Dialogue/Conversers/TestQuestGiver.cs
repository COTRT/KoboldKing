using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class TestQuestGiver : Converser, IQuestGiver
{
    private Quest quest;
    public void QuestAssigned(Quest quest)
    {
        this.quest = quest;
    }

    protected override Dialogue ChooseDialogue()
    {
        return ObjectDialogues[quest == null ? "Assign_Quest" : "Quest_Assigned"];
    }
}

