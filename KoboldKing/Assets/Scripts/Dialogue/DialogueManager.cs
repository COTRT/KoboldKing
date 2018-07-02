using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public  class DialogueManager : MonoBehaviour
{
    /// <summary>
    /// Please note:  No File Extensions.
    /// </summary>
    public string JSONFileName = "Dialogue";
    [HideInInspector]
    public static DialogueManager Instance;
    DialogueDB _masterDialogueDict;


    public DialogueDB MasterDialogueDict
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
    private DialogueDB GetJSONDatabase()
    {
        var asset = Resources.Load<TextAsset>("JSON/" + JSONFileName);
        if (asset == null)
        {
            throw new System.IO.FileNotFoundException("The Target Dialogue File was not found");
        }
        return JsonConvert.DeserializeObject<DialogueDB>(asset.text);

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
