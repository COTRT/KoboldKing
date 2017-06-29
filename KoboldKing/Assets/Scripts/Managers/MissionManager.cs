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
    public class MissionManager : MonoBehaviour, IGameManager
    {
        public ManagerStatus status { get; private set; }

        public int curLevel { get; private set; }
        public int maxLevel { get; private set; }

        public void Startup()
        {
            Debug.Log("Mission manager starting...");

            UpdateData(0, 3);

            // any long-running startup tasks go here, and set status to 'Initializing' until those tasks are complete
            status = ManagerStatus.STARTED;
        }

        public void UpdateData(int curLevel, int maxLevel)
        {
            this.curLevel = curLevel;
            this.maxLevel = maxLevel;
        }

        public void ReachObjective()
        {
            // could have logic to handle multiple objectives
            Messenger.Broadcast(GameEvent.LEVEL_COMPLETE);
        }

        public void GoToNext()
        {
            if (curLevel < maxLevel)
            {
                curLevel++;
                string name = "Level" + curLevel;
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
            string name = "Level" + curLevel;
            Debug.Log("Loading " + name);
            SceneManager.LoadScene(name);
        }

        public event UnhandledExceptionEventHandler OnException;
        public ManagerStatus Status { get; private set; }

        public void Startup(DataService dataService)
        {
            throw new NotImplementedException();
        }
    }
}
