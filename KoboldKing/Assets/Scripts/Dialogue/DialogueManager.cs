using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Assets.Scripts.Misc;

public  class DialogueManager : JsonFileReader<DialogueDB>
{
    public static DialogueManager Instance { get; set; }
    public static ObjectDialogues Get(string ObjectName)
    {
        return MasterJsonDict[ObjectName];
    }
    private new void Awake()
    {
        if (string.IsNullOrEmpty(JSONFileName)) JSONFileName = "Dialogue";
        base.Awake();
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
