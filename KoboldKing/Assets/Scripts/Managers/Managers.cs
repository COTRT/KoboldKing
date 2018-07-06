using Assets.Scripts.Data;
using Assets.Scripts.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    [RequireComponent(typeof (DataManager))]
    [RequireComponent(typeof (InventoryManager))]
    [RequireComponent(typeof (MissionManager))]
    [RequireComponent(typeof (PlayerManager))]
    public class Managers : MonoBehaviour
    {

        // static properties that other code used to access managers.
        public static DataManager Data { get; private set; }
        public static InventoryManager Inventory { get; private set; }
        public static PlayerManager Player { get; private set; }
        public static MissionManager Mission { get; private set; }

        // the list of managers to loop through during startup sequence.
        private List<IGameManager> _startSequence;

        private void Awake()
        {
            // persist gameObjects when new scenes are loaded.  by default all objects are purged when a new scene loads.
            DontDestroyOnLoad(gameObject);

            Inventory = GetComponent<InventoryManager>();
            Mission = GetComponent<MissionManager>();
            Player = GetComponent<PlayerManager>();

            // DataManager uses other managers so it should be listed after those managers are created.
            Data = GetComponent<DataManager>();

            _startSequence = new List<IGameManager>();
            _startSequence.Add(Inventory);
            _startSequence.Add(Mission);
            _startSequence.Add(Player);
            _startSequence.Add(Data);

            // launch the startup sequence asynchronously
            StartCoroutine(StartupManagers());
        }

        private IEnumerator StartupManagers()
        {
            yield return null;

            int numModules = _startSequence.Count;
            int numReady = 0;

            // keep looping until all managers are started.
            while (numReady < numModules)
            {
                int lastReady = numReady;
                numReady = 0;

                foreach (IGameManager manager in _startSequence)
                {
                    if (manager.Status == ManagerStatus.STARTED)
                    {
                        numReady++;
                    }
                }

                if (numReady > lastReady)
                    Debug.Log("Progress: " + numReady + "/" + numModules);
                Messenger<int, int>.Broadcast(StartupEvent.MANAGERS_PROGRESS, numReady, numModules, MessengerMode.DONT_REQUIRE_LISTENER);

                // pause for one frame before checking again.
                yield return null;
            }

            Debug.Log("All managers started up");
            Messenger.Broadcast(StartupEvent.MANAGERS_STARTED, MessengerMode.DONT_REQUIRE_LISTENER);
        }
    }

}



    
