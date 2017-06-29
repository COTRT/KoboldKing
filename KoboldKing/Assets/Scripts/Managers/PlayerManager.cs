using System;
using Assets.Scripts.Data;
using Assets.Scripts.Events;
using UnityEngine;

// Inherit a class and implement an interface
namespace Assets.Scripts.Managers
{
    public class PlayerManager : MonoBehaviour, IGameManager
    {
        public ManagerStatus status { get; private set; }
        public int health { get; private set; }
        public int maxHealth { get; private set; }

        //private NetworkService _network;

        public void Startup()
        {
            Debug.Log("Player manager starting...");

            //_network = service;

            // these values could be initialized with saved data
            UpdateData(50, 100);

            // any long-running startup tasks go here, and set status to 'Initializing' until those tasks are complete
            status = ManagerStatus.STARTED;
        }

        public void UpdateData(int health, int maxHealth)
        {
            this.health = health;
            this.maxHealth = maxHealth;
        }

        public void ChangeHealth(int value)
        {

            // other scripts can't set health directly but can call this method.
            health += value;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            else if (health < 0)
            {
                health = 0;
            }

            if (health == 0)
            {
                Messenger.Broadcast(GameEvent.LEVEL_FAILED);
               
            

            }

            Messenger.Broadcast(GameEvent.HEALTH_UPDATED);
        }

        public void Respawn()
        {
            UpdateData(50, 100);
        }

        public event UnhandledExceptionEventHandler OnException;
        public ManagerStatus Status { get; private set; }
        public void Startup(DataService dataService)
        {
            throw new NotImplementedException();
        }
    }
}
