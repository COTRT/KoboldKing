using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Dialogue
{
    /// <summary>
    /// The Class template containing all information stored in the JSON Dialogue files
    /// </summary>
    /// <remarks>
    /// This guy maps strings (the names of different objects in the game, say the player or an NPC) to the corresponding object's Dialogues.
    /// An object's Dialogues is yet another dictionary, mapping different keys of different situations
    /// (Say, "quest_undelivered" for when an NPC needs to say stuff to deliver a quest)
    /// to that situation key's corresponding Dialogue tree.
    /// Each Dialogue tree can then have as long a conversation as desired.
    /// </remarks>
    class DialogueDB : Dictionary<string,ObjectDialogues>
    {
    }
}
