using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public List<BaseStat> stats = new List<BaseStat>();

    void Start()
    {
        stats.Add(new BaseStat(4, "Power", "Your power level."));

        // For equipment - we would have a list of equipment as a list of stat bonus and then loop through lists.
        // buffs...etc....
        stats[0].AddStatBonus(new StatBonus(5));
        stats[0].AddStatBonus(new StatBonus(-7));
        stats[0].AddStatBonus(new StatBonus(21));
        Debug.Log(stats[0].GetCalculatedStatValue());
    }

}
