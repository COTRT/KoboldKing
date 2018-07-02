using System.Linq;
using UnityEngine;

/// <summary>
/// Chooses a random item from the available options,
/// Ignoring thier "situation keys", as I seem to have named them.
/// </summary>
public class RandomConverser : Converser
{
    protected override Dialogue ChooseDialogue()
    {
        return ObjectDialogues.Values.ElementAt(Random.Range(0, ObjectDialogues.Count));
    }
}
