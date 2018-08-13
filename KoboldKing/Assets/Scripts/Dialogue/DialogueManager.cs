using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Assets.Scripts.Misc;
using Assets.Scripts.Data;

public  class DialogueManager : JsonFileReader<DialogueDB>
{
    public static DialogueManager Instance { get; set; }
    public static ObjectDialogues Get(string ObjectName)
    {
        return MasterJsonDict[ObjectName];
    }
    private new void StartManager(DataService dataService)
    {
        if (string.IsNullOrEmpty(JSONFileName)) JSONFileName = "Dialogue";
        base.StartManager(dataService);
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
