using UnityEditor;
using Assets.Scripts;
using Assets.Scripts.Models;
using Assets.Scripts.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour {

    public float attackTimer;
    public float coolDown;
    public bool attackAgain = false;
    GameObject Player;

	// Use this for initialization
	void Start () {
        attackTimer = 0;
        coolDown = 2.0f;
	}

    void Attack()
    {
        var DefenderStats = Player.GetComponent<Mob>();
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
        var combatResult = new CombatResult();
        var combatManager = new CombatManager();
        combatResult = combatManager.CalcCombat(combatInput);
        if (combatResult.Hit)
        {
            var damageable = Player.GetComponent<Damageable>();
            damageable.DealDamage(DamageType.Default, combatResult.Damage + enemyStats.MeleeDamageBonus);
        }
        attackTimer = coolDown;
        attackAgain = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player = other.gameObject;
            attackAgain = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        attackAgain = false;
    }

    // Update is called once per frame
    void Update () {
		if(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        if(attackTimer < 0)
        {
            attackTimer = 0;
        }
        if (attackTimer == 0 && attackAgain)
        {
            Attack();
        }
    }
}
//public string Attacker { get; set; }
//public string Defender { get; set; }
//public int MinDamage { get; set; }
//public int MaxDamage { get; set; }
//public int AttackRating { get; set; }
//public int DefenseRating { get; set; }
