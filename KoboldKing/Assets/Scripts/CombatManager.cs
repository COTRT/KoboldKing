using Assets.Scripts.Models;
using UnityEngine;
using Assets.Scripts.Item;
using Random = System.Random;

namespace Assets.Scripts
{
    //8. Assign Damage
    // message to screen hit/miss damage?  what was the results of the round of battle??
    public static class CombatCalculator
    {
        private static readonly Random Rand = new Random();
        public static CombatResult CalcCombat(CombatInput input)
        {
            var hit = CalcHit(input);
            
            

            var damage = 0;
            if (hit)
            {
                damage = CalcDamage(input);
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
        
        private static int CalcDamage(CombatInput input)
        {
            var rollDamage = Rand.Next(input.MinDamage, input.MaxDamage);

            return rollDamage;
        }

        private static bool CalcHit(CombatInput input)
        {
            var baseHitChance = 50;

            var toHit = baseHitChance + input.AttackRating;
            var defense = input.DefenseRating;

            var hitChance = toHit - defense;

            var rollDice = Rand.Next(1, 100);

            return rollDice <= hitChance;
         }


        public static CombatAttackResult Attack(GameObject Attacker, GameObject Defender)
        {
            if (Defender.name != Attacker.name)
            {
                var AttackerStats = Attacker.GetComponent<Mob>();
                if(AttackerStats == null)
                {
                    return new CombatAttackResult(CombatAttackError.AttackerNotMob);
                }

                var OtherStats = Defender.GetComponent<Mob>();
                if (OtherStats == null)
                {
                    return new CombatAttackResult(CombatAttackError.DefenderNotMob);
                }

                var combatInput = new CombatInput(AttackerStats, OtherStats);
                var combatResult = CalcCombat(combatInput);
                if (combatResult.Hit)
                {
                    var damageable = Defender.GetComponent<Damageable>();
                    if(damageable == null)
                    {
                        return new CombatAttackResult(CombatAttackError.DefenderNotDamageable);
                    }
                    damageable.DealDamage(DamageType.Default, combatResult.Damage + OtherStats.MeleeDamageBonus);
                }
                return new CombatAttackResult(CombatAttackError.None, combatResult);
            }
            else
            {
                return new CombatAttackResult(CombatAttackError.DefenderIsAttacker);
            }
        }
    }
    public class CombatAttackResult : CombatResult
    {
        public CombatAttackResult()
        {

        }
        public CombatAttackResult(CombatAttackError error, CombatResult combatResult = null)
        {
            this.CombatAttackError = error;
            if(combatResult != null)
            {
                this.Defender = combatResult.Defender;
                this.Attacker = combatResult.Attacker;
                this.Damage = combatResult.Damage;
                this.Hit = combatResult.Hit;
            }
        }
        public CombatAttackError CombatAttackError { get; set; }
    }
    public enum CombatAttackError
    {
        AttackerNotMob,
        DefenderNotMob,
        DefenderNotDamageable,
        DefenderIsAttacker,
        None
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

