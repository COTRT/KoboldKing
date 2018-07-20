using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// See <see cref="Converser"/> for a basic explanation of Conversers.
/// This Converser is simple and static.  It assumes the attached GameObject
/// can only ever have one dialogue tree, ever, no matter the situation (i.e., it's static)
/// As such, it returns the sole entry in the Dialogue dictionary, or nothing at all.
/// </summary>
public class StaticConverser : Converser
{
    // Since the Converser has already done the work to get this object's Dialogue options,
    //all this guy has to do is choose one of them.
    //for this particular class, that is done by the choosing the exactly on dialogue available to this class
    // and throwing an error otherwise.
    protected override Dialogue ChooseDialogue()
    {
        if (ObjectDialogues.Values.Count == 0 || ObjectDialogues.Values.Count > 1)
        {
            throw new InvalidOperationException("The StaticConverser on " + name + " (with JSONDatabaseEntryName '" + JSONDatabaseEntryName + "') does not have exactly one dialogue entry (it has " + ObjectDialogues.Count + ").  Returning null");
        }
        else
        {
            return ObjectDialogues.Values.Single();
        }
    }
}
