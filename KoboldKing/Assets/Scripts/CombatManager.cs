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

            return false;
        }
    }
}
