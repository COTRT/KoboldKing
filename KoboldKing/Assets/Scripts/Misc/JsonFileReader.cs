using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Misc
{
    public class JsonFileReader<T>: MonoBehaviour
    {
        /// <summary>
        /// Please note:  No File Extensions.
        /// </summary>
        public string JSONFileName;
        [HideInInspector]
        public static JsonFileReader<T> Instance;
        T _masterJsonDict;
        bool _loaded = false;


        public T MasterJsonDict
        {
            get
            {
                if (!_loaded)
                {
                    _masterJsonDict = GetJSON();
                    _loaded = true;
                }
                return _masterJsonDict;
            }

            set
            {
                _masterJsonDict = value;
            }
        }

        /// <summary>
        /// This returns a dictionary mapping GameObject names to their corresonding Dialogue trees, which the object itself can then choose from 
        /// depending on the environment (say, follow the "Quest Completed" tree after a quest is completed)
        /// </summary>
        /// <returns></returns>
        private T GetJSON()
        {
            var asset = Resources.Load<TextAsset>("JSON/" + JSONFileName);
            if (asset == null)
            {
                throw new System.IO.FileNotFoundException("The Target JSON File was not found");
            }
            return JsonConvert.DeserializeObject<T>(asset.text);

        }
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
    }
}
