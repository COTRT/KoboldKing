using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// See <see cref="IConverser"/> for a basic explanation of Conversers.
/// This Converser is simple and static.  It assumes the attached GameObject
/// can only ever have one dialogue tree, ever, no matter the situation (i.e., it's static)
/// As such, it returns the sole entry in the Dialogue dictionary, or nothing at all.
/// </summary>
public class StaticConverser : MonoBehaviour, IConverser
{
    public ObjectDialogues Dialogues;
    public string JSONDatabaseEntryName;

    public Dialogue GetDialogue()
    {

        if (Dialogues == null)
        {
            if (string.IsNullOrEmpty(JSONDatabaseEntryName))
            {
                throw new KeyNotFoundException("No JSONDatabaseEntryName for GameObject of name " + name);
            }
            Dialogues = DialogueManager.Get(JSONDatabaseEntryName);
        }

        if (Dialogues.Values.Count == 0 || Dialogues.Values.Count > 1)
        {
            Debug.LogWarning("The StaticConverser on " + name + " (with JSONDatabaseEntryName '" + JSONDatabaseEntryName + "') does not have exactly one dialogue entry (it has " + Dialogues.Count + ").  Returning null");
            return null;
        }
        else
        {
            return Dialogues.Values.Single();
        }
    }
}
