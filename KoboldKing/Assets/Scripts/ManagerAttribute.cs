using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
internal sealed class ManagerAttribute : System.Attribute
{
    /// <summary>The priority level of this manager at startup. Default 0, which is a fine priority for most managers.</summary>
    public int Priority = 0;
    /// <summary>
    /// Mark this as a Manager, to be started at game startup.  Lower priority managers start up after higher priority managers are COMPLETELY started.
    /// </summary>
    public ManagerAttribute()
    {

    }
}
