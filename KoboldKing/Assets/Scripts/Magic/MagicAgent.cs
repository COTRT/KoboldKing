using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using System.Linq;

public class MagicAgent : MonoBehaviour
{
    public string spellName;
    public HashSet<string> AvailableSpells;

    private SpellManager magicManager;
    // Use this for initialization
    void Start()
    {
        AvailableSpells = new HashSet<string>();
        magicManager = this.GetManager<SpellManager>();
        //No AddRange() :-(
        foreach (var spell in magicManager.Prefabs.Keys)
        {
            AvailableSpells.Add(spell);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (TryCast(spellName)){
                Debug.Log("Casted spell:  " + spellName);
            } else {
                Debug.Log("Could not cast Spell:  " + spellName);
            }
        }
    }

    /// <summary>
    /// Not type casting - spell casting
    /// </summary>
    bool TryCast(string spell)
    {
        bool canCast = AvailableSpells.Contains(spell) && magicManager.Exists(spell);
        if (canCast)
        {
            magicManager.Create(spell).Cast(gameObject);
        }
        return canCast;
    }
}
