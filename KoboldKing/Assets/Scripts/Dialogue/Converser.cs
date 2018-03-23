using System.Collections.Generic;
using UnityEngine;
//using Random = System.Random;

/// <summary>
/// This Converser implments <see cref="IConverser"/>'s <see cref="IConverser.GetDialogue"/> method by returning a random element of the dialog list
/// </summary>
public class Converser : MonoBehaviour, IConverser
{
    public List<Dialogue> Dialogues;

    //Temporary fields for a simple, one-step, two-response-options conversation (for testing UI)
    public string TempStatement;
    public string TempResponseOption1;
    public string TempResponseStatement1;
    public string TempResponseOption2;
    public string TempResponseStatement2;
    public void Start()
    {

    }
    public Dialogue GetDialogue()
    {
        //return Dialogues[new Random().Next(Dialogues.Count)];
        return new Dialogue()
        {
            Statement = TempStatement,
            Responses = new Dictionary<string, Dialogue>()
            {
                {TempResponseOption1, new Dialogue(TempResponseStatement1) },
                {TempResponseOption2, new Dialogue(TempResponseStatement2) },
            }
        };
    }
}
