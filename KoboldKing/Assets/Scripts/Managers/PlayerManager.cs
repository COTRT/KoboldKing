using System;
using Assets.Scripts.Data;
using Assets.Scripts.Events;
using UnityEngine;

// Inherit a class and implement an interface
namespace Assets.Scripts.Managers
{
    public class PlayerManager : ManagerBase
    {
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }

        //private NetworkService _network;

        protected override void StartManager(DataService dataService)
        {
            Debug.Log("Player manager starting...");

            //_network = service;

            // these values could be initialized with saved data
            UpdateData(50, 100);

            // any long-running startup tasks go here, and set status to 'Initializing' until those tasks are complete
            Status = ManagerStatus.STARTED;
        }

        public void UpdateData(int health, int maxHealth)
        {
            this.Health = health;
            this.MaxHealth = maxHealth;
        }

        public void ChangeHealth(int value)
        {

            // other scripts can't set health directly but can call this method.
            Health += value;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
            else if (Health < 0)
            {
                Health = 0;
            }

            if (Health == 0)
            {
                Messenger.Broadcast(GameEvent.LEVEL_FAILED);
               
            

            }

            Messenger.Broadcast(GameEvent.HEALTH_UPDATED);
        }

        public void Respawn()
        {
            UpdateData(50, 100);
        }

    }
}
