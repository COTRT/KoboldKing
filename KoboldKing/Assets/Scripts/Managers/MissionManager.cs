using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Data;
using Assets.Scripts.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class MissionManager : ManagerBase
    {

        public int CurLevel { get; private set; }
        public int MaxLevel { get; private set; }

        public override void Startup(DataService dataService)
        {
            Debug.Log("Mission manager starting...");

            UpdateData(0, 3);

            // any long-running startup tasks go here, and set status to 'Initializing' until those tasks are complete
            Status = ManagerStatus.STARTED;
        }

        public void UpdateData(int curLevel, int maxLevel)
        {
            this.CurLevel = curLevel;
            this.MaxLevel = maxLevel;
        }

        public void ReachObjective()
        {
            // could have logic to handle multiple objectives
            Messenger.Broadcast(GameEvent.LEVEL_COMPLETE);
        }

        public void GoToNext()
        {
            if (CurLevel < MaxLevel)
            {
                CurLevel++;
                string name = "Level" + CurLevel;
                Debug.Log("Loading " + name);
                SceneManager.LoadScene(name);
            }
            else
            {
                Debug.Log("Last level");
                Messenger.Broadcast(GameEvent.GAME_COMPLETE);
            }
        }

        public void RestartCurrent()
        {
            string name = "Level" + CurLevel;
            Debug.Log("Loading " + name);
            SceneManager.LoadScene(name);
        }

    }
}
