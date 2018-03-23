using System.Collections.Generic;

/// <summary>
/// This class is the basis of the Dialogue system.  It's sole purpose in life is to represent a conversation, in a tree like fashion.
/// It works pretty simply.  A given Dialogue class contains a statement, which is to be said to the player.  
/// The dictionary of responses contains the user's potential options for responses as keys
/// Once a user selects one of the options, the key they selected is then used to look up another Dialogue object inside that dictionary.
/// This dialogue object will then have it's own statement (to be made by the NPC), it's own set of responses, and a variety of potential conversation paths from there...
/// At the moment, there is no other Dialogue options to go in the Dialogue object (for example, a combat class)SSS
/// </summary>
public class Dialogue  {
    public Dialogue()
    {

    }
    public Dialogue(string Statement)
    {
        this.Statement = Statement;
    }
    public Dialogue(string Statement, Dictionary<string,Dialogue> Responses)
    {
        this.Statement = Statement;
        this.Responses = Responses;
    }
    public string Statement { get; set; }
    public Dictionary<string,Dialogue> Responses { get; set; }
}                                     
