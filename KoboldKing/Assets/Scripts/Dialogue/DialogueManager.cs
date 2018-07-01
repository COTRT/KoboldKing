using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class DialogueManager
{
    static Dictionary<string, ObjectDialogues> _masterDialogueDict;


    public static Dictionary<string, ObjectDialogues> MasterDialogueDict
    {
        get
        {
            return _masterDialogueDict ?? (_masterDialogueDict = GetJSONDatabase());
        }

        set
        {
            _masterDialogueDict = value;
        }
    }
    
    /// <summary>
    /// This returns a dictionary mapping GameObject names to their corresonding Dialogue trees, which the object itself can then choose from 
    /// depending on the environment (say, follow the "Quest Completed" tree after a quest is completed)
    /// </summary>
    /// <returns></returns>
    private static Dictionary<string,ObjectDialogues> GetJSONDatabase()
    {
        return JsonConvert.DeserializeObject<Dictionary<string, ObjectDialogues>>(Resources.Load<TextAsset>("GameGrind/JSON/DialogueTest").ToString());

    }
}
