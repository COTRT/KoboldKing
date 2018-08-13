using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Quests
{
    public static class  ConfigDictionaryExtensions
    {
        public static void CheckConfigContains(this Dictionary<string, string> arguments, params string[] keys)
        {
            var missingKeys = keys.Where(k => !arguments.ContainsKey(k));
            if (missingKeys.Count() > 1)
                throw new ArgumentException(
                    $"Goal {(arguments["Description"] == null ? "" : ("(with description " + arguments["Description"] + ")"))} is missing these required infos:  {string.Join(",", missingKeys)}"
                );
        }
    }
}
