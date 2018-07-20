using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace Assets.Scripts.Quests
{
    public class Goal
    {
        public string Description { get; set; }
        public bool Completed { get; set; }
        public int CurrentAmount { get; set; }
        public int RequiredAmount { get; set; }

        public virtual void Init()
        {
            // default init stuff.
        }



        public void Evaluate()
        {
            if (CurrentAmount >= RequiredAmount)
            {
                GetCompletedStatus();
            }
        }

        private void GetCompletedStatus()
        {
            Completed = true;
        }
    }
}
