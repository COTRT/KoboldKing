using System.Collections.Generic;

public abstract class Reward
{
    public Reward(Dictionary<string,string> arguments)
    {
        string desc;
        arguments.TryGetValue("Description", out desc);
        Description = desc;
    }
    public string Description { get; set; }
    public abstract void Apply();

}