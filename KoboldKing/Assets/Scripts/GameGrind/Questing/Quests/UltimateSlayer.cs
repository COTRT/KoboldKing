using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSlayer : Quest
{


    void Start()
    {

        QuestName = "Ultimate Slayer";
        Description = "Kill a bunch of stuff";



        Goals.Add(new KillGoal(this, "Slime", "Kill 2 Slimes", false, 0, 2));
        Goals.Add(new KillGoal(this, "Vampire", "Kill 2 Vampires", false, 0, 2));
        Goals.Add(new CollectionGoal(this, "potion_log", "Find a Log Potion", false, 0, 1));

        Goals.ForEach(x => x.Init());


    }

}
