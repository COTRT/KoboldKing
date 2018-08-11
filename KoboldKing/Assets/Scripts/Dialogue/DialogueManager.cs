using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Assets.Scripts.Misc;

public  class DialogueManager : JsonFileReader<DialogueDB>
{
    public static ObjectDialogues Get(string ObjectName)
    {
        return Instance.MasterJsonDict[ObjectName];
    }
    private void Awake()
    {
        if (string.IsNullOrEmpty(JSONFileName)) JSONFileName = "Dialogue";
    }
}
