using Assets.Scripts.Data;
using UnityEngine;
using Assets.Scripts.Events;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Assets.Scripts.Managers
{
    [Manager(Priority = 0)]
    public class MagicManager : ManagerBase
    {
        public Dictionary<string,Spell> Spells;

        public Spell Get(string spell)
        {
            return Spells[spell];
        }

        public Spell this[string spell]
        {
            get
            {
                return Get(spell);
            }
        }
        public override void Startup(DataService dataService)
        {
            dataService.Register(this, "Magic");
            LoadSpells();
            Startup_Complete();
        }
        private void LoadSpells()
        {
            Spells = Resources.LoadAll("Spells")
                .Select(s => (GameObject)s)
                .ToDictionary(
                    s => s.name,
                    s => s.GetComponent<Spell>());
            
        }
        [On(StartupEvent.MANAGERS_STARTED)]
        public void OnAllStarted()
        {
            Debug.Log("MagicManager has recieved a StartupEvent.MANAGERS_STARTED event!");
        }

        public bool Exists(string spell)
        {
            return Spells.ContainsKey(spell);
        }
    }
}