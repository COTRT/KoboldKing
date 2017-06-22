using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using System.Linq;

public class MagicAgent : MonoBehaviour
{
    public string spellName;
    public HashSet<string> AvailableSpells;

    private MagicManager magicManager;
    // Use this for initialization
    void Start()
    {
        AvailableSpells = new HashSet<string>();
        magicManager = this.GetManager<MagicManager>();
        foreach (var spell in magicManager.Spells.Keys)
        {
            AvailableSpells.Add(spell);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            TryCast(spellName);
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
            magicManager[spell].Cast(gameObject);
        }
        return canCast;
    }
}
