using Assets.Scripts.Data;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Managers
{
    public abstract class NamedPrefabManager<T> : ManagerBase where T : UnityEngine.Object
    {
        public Dictionary<string, T> Prefabs;

        public T Create(string prefabName)
        {
            return Instantiate(Get(prefabName));
        }
        public T Get(string prefabName)
        {
            return Prefabs[prefabName];
        }
        /// <summary>
        /// Warning:  This --> CREATES &lt-- (aka instantiates) the specified effect.  Use Get() to get the (unstantiated) prefab
        /// </summary>
        public T this[string prefabName]
        {
            get
            {
                return Create(prefabName);
            }
            set
            {
                Prefabs[prefabName] = value;
            }
        }
        public override void Startup(DataService dataService)
        {
            LoadPrefabs();
            Startup_Complete();
        }
        private void LoadPrefabs()
        {
            Prefabs = ResourceLoader.LoadNamedPrefabs<T>();

        }

        public bool Exists(string prefabName)
        {
            return Prefabs.ContainsKey(prefabName);
        }

    }

    public static class ResourceLoader
    {
        public static Dictionary<string, T> LoadNamedPrefabs<T>()
        {
            return Resources.LoadAll<GameObject>(typeof(T).Name + "s")
                            .ToDictionary(
                                s => s.name,
                                s => s.GetComponent<T>());
        }
    }
}