using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Models
{
    public class CombatInput
    {
        public string Attacker { get; set;}
        public string Defender { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int AttackRating { get; set; }
        public int DefenseRating { get; set; }
    }
}
