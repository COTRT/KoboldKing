using Assets.Scripts.Data;
using Assets.Scripts.Managers;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Misc
{
    public class JsonFileReader<T>: ManagerBase
    {
        /// <summary>
        /// Please note:  No File Extensions.
        /// </summary>
        public string JSONFileName;
        private static string _jsonFileName;
        static T _masterJsonDict;
        static bool _loaded = false;


        public static T MasterJsonDict
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
        private static T GetJSON()
        {
            var asset = Resources.Load<TextAsset>("JSON/" + _jsonFileName);
            if (asset == null)
            {
                throw new System.IO.FileNotFoundException("The Target JSON File was not found");
            }
            return JsonConvert.DeserializeObject<T>(asset.text);

        }

        protected override void StartManager(DataService dataService)
        {
            _jsonFileName = JSONFileName;
            //_loaded = false; //Reload JSON database
        }
    }
}
