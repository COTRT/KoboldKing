
using Assets.Scripts.Events;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is a base from which any kind of dialogue grabber can be made.
/// Static?  Random?  Dependent on the environment?  It doesn't care.  Just get it it's dialogue, and it's happy.
/// </summary>
/// 
// Note the "abstract" modifier.
// Abstract classes can have thier own code in them to do stuff, but they can't be used directly.
// (that is, I couldn't go out and say "new Converser()")
// However, they can be used once extended in a class, like is done by <see cref="StaticConverser"/>
// "abstract" on a class merely makes it required to override it.
// "abstract" on a method means the overriding class MUST provide that method as well.
// This class is making use of all that to do some of the repetitive work in making a new Converser.
public abstract class Converser : Interactable
{
    public string JSONDatabaseEntryName;
    public ObjectDialogues ObjectDialogues;

    //This is the method people see and interact with.  
    //It, in turn calls ChooseDialogue once it's gotten the ObjectDialogue from the DialogueManager.
    public virtual Dialogue GetDialogue()
    {

        if (ObjectDialogues == null)
        {
            if (string.IsNullOrEmpty(JSONDatabaseEntryName))
            {
                throw new KeyNotFoundException("No JSONDatabaseEntryName for GameObject of name " + name);
            }
            ObjectDialogues = DialogueManager.Get(JSONDatabaseEntryName);
        }
        return ChooseDialogue();
    }

    public override void Interact()
    {
        Messenger<Dialogue,GameObject>.Broadcast(UIEvent.SHOW_DIALOGUE, GetDialogue(),gameObject);
    }

    //This is invisible to the outside world ("protected" means "available to overriding classes, but no more"), 
    //so that users can't call it mistakenly before the ObjectDialogues have been loaded.
    protected abstract Dialogue ChooseDialogue();
}