using System;
using Assets.Scripts.Data;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Events;

namespace Assets.Scripts.Managers
{
    [Manager(Priority = 0)]
    public class MagicManager : ManagerBase
    {
        public bool hothStarted = false;
        public override void Startup(DataService dataService)
        {
            dataService.Register(this, "Magic");
            hothStarted = true;
            Startup_Complete();
        }
        [On(StartupEvent.MANAGERS_STARTED)]
        public void OnAllStarted()
        {
            Debug.Log("MagicManager has recieved a StartupEvent.MANAGERS_STARTED event!");
        }
    }
}