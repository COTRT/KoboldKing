using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public  class DialogueManager : MonoBehaviour
{
    public string JSONFileName = "Dialogue.json";
    [HideInInspector]
    public static DialogueManager Instance;
    Dictionary<string, ObjectDialogues> _masterDialogueDict;


    public  Dictionary<string, ObjectDialogues> MasterDialogueDict
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
    private  Dictionary<string,ObjectDialogues> GetJSONDatabase()
    {
        var asset = Resources.Load<TextAsset>("JSON/" + JSONFileName);
        if (asset == null)
        {
            throw new System.IO.FileNotFoundException("The Target Dialogue File was not found");
        }
        return JsonConvert.DeserializeObject<Dictionary<string, ObjectDialogues>>(asset.text);

    }
    public static ObjectDialogues Get(string ObjectName)
    {
        return Instance.MasterDialogueDict[ObjectName];
    }
    private void Awake()
    {
        Instance = this; //We don't care what exists currently.  We can afford a reload.
    }
}
