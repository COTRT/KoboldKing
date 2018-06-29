using System.Collections.Generic;
using UnityEngine;
//using Random = System.Random;

/// <summary>
/// This Converser implments <see cref="IConverser"/>'s <see cref="IConverser.GetDialogue"/> method by returning a random element of the dialog list
/// </summary>
public class Converser : MonoBehaviour, IConverser
{
    public Dictionary<string,Dialogue> Dialogues;
    public string JSONDatabaseEntryName;

    public Dialogue GetDialogue()
    {
        //return Dialogues[new Random().Next(Dialogues.Count)];
        //return new Dialogue()
        //{
        //    Statement = TempStatement,
        //    Responses = new Dictionary<string, Dialogue>()
        //    {
        //        {TempResponseOption1, new Dialogue(TempResponseStatement1) },
        //        {TempResponseOption2, new Dialogue(TempResponseStatement2) },
        //    }
        //};
        if (Dialogues == null)
        {
            if (string.IsNullOrEmpty(JSONDatabaseEntryName))
            {
                throw new KeyNotFoundException("No JSONDatabaseEntryName for GameObject of name " + name);
            }
            Dialogues = DialogueManager.MasterDialogueDict[JSONDatabaseEntryName];
        }
        return Dialogues;

    }
}
