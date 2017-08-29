using UnityEditor;
using Assets.Scripts;
using Assets.Scripts.Models;
using Assets.Scripts.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var DefenderStats = other.GetComponent<Mob>();
            var Defender = DefenderStats.Name;
            var DefenseRating = DefenderStats.DefenseRating;
            var enemyStats = GetComponent<Mob>();
            var Attacker = enemyStats.Name;
            var AttackRating = enemyStats.AttackRating;
            var MinDamage = enemyStats.MinDamage;
            var MaxDamage = enemyStats.MaxDamage;
            var combatInput = new CombatInput();
            combatInput.Attacker = Attacker;
            combatInput.Defender = Defender;
            combatInput.MinDamage = MinDamage;
            combatInput.MaxDamage = MaxDamage;
            combatInput.AttackRating = AttackRating;
            combatInput.DefenseRating = DefenseRating;
            var combatManager =  GetComponent<CombatManager>();
            combatManager.CalcCombat(combatInput);
            var combatResult = GetComponent<CombatResult>();
            if (combatResult.Hit)
            {
                var damageable = other.GetComponent<Damageable>();
                damageable.DealDamage(DamageType.Default, combatResult.Damage + enemyStats.MeleeDamageBonus);
            }
            
            
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
//public string Attacker { get; set; }
//public string Defender { get; set; }
//public int MinDamage { get; set; }
//public int MaxDamage { get; set; }
//public int AttackRating { get; set; }
//public int DefenseRating { get; set; }
