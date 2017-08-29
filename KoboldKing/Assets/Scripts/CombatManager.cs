using System.Runtime.InteropServices.ComTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Models;

namespace Assets.Scripts
{
    //8. Assign Damage
    // messge to screen hit/miss damage?  what was the results of the round of battle??
    public class CombatManager
    {
        private static readonly Random Random = new Random();
        public CombatResult CalcCombat(CombatInput input)
        {
            var hit = calcHit(input);

            var damage = 0;
            if (hit)
            {
                damage = calcDamage(input);
            }

            var combatResult = new CombatResult()
            {
                Attacker = input.Attacker,
                Defender = input.Defender,
                Hit = hit,
                Damage = damage
            };


            return combatResult;
        }
        
        private int calcDamage(CombatInput input)
        {
            var rollDamage = Random.Next(input.MinDamage, input.MaxDamage);

            return rollDamage;
        }

        private bool calcHit(CombatInput input)
        {
            var baseHitChance = 50;

            var toHit = baseHitChance + input.AttackRating;
            var defense = input.DefenseRating;

            var hitChance = toHit - defense;

            var rollDice = Random.Next(1, 100);

            bool hit;
            if (rollDice <= hitChance)
            {
                return true;
            }
            else
            {
                return false;
            }
         }
    }
}


//private static System.Random s_Random = new System.Random();
//public Transform GameOverScreen;
//private GameObject myDamageable;
//private GameObject PlayerCombat;
//int randomPercent = s_Random.Next(0, 100);
//public int AttackerChance;
//int BaseHitChance = 50;
//int perCent;



//void OnTriggerEnter(Collider other)
//{
//    perCent = s_Random.Next(0, 100);
//    if (other.tag == "Player")
//    {
//        var DefenseChance = other.GetComponent<PlayerCombat>();
//        int AttackChance = BaseHitChance + AttackerChance - DefenseChance.DefenseChance;
//        if(perCent >= 95)
//        {
//            var component = other.GetComponent<Damageable>();
//            if (component != null)
//                component.DealDamage(DamageType.Default, 50);
//        }
//        else if (perCent <= AttackChance)
//        {
//            var component = other.GetComponent<Damageable>();
//            if (component != null)
//                component.DealDamage(DamageType.Default, 50);
//        }
//    }
//}

    //Start Combat Script.

    //1. attach to bg.
    //2. on triggr enter call combat manager.  Send to combat manager the following:
    //    A). Who am I.
    //    B). Who am I attacking.

