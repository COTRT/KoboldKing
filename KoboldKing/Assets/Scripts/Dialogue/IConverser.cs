
/// <summary>
/// This interface is a base from which any kind of dialogue grabber can be made.
/// Static?  Random?  Dependent on the environment?  It doesn't care.  Just get it it's dialogue, and it's happy.
/// </summary>
public interface IConverser
{
    Dialogue GetDialogue();
}