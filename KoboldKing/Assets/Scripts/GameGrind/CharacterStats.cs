using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public List<BaseStat> stats = new List<BaseStat>();

    void Start()
    {
        // Start is going to be used on a temp basis only.  In the future each object/player/npc will define and pass in its own values.
        stats.Add(new BaseStat(4, "Power", "Your power level."));
        stats.Add(new BaseStat(2, "Vitality", "Your vitality level."));

        //// For equipment - we would have a list of equipment as a list of stat bonus and then loop through lists.
        //// buffs...etc....
        //stats[0].AddStatBonus(new StatBonus(5));
        //stats[0].AddStatBonus(new StatBonus(-7));
        //stats[0].AddStatBonus(new StatBonus(21));
        //Debug.Log(stats[0].GetCalculatedStatValue());
    }

    public void AddStatBonus(List<BaseStat> statBonuses)
    {
        foreach (var statBonus in statBonuses)
        {
            stats.Find(x => x.StatName == statBonus.StatName).AddStatBonus(new StatBonus(statBonus.BaseValue));
        }
    }

    public void RemoveStatBonus(List<BaseStat> statBonuses)
    {
        foreach (var statBonus in statBonuses)
        {
            stats.Find(x => x.StatName == statBonus.StatName).RemoveStatBonus(new StatBonus(statBonus.BaseValue));
        }
    }

}
