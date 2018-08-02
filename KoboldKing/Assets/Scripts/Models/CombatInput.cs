using Assets.Scripts.ItemX;

namespace Assets.Scripts.Models
{
    public class CombatInput
    {
        public CombatInput()
        {

        }
        public CombatInput(Mob Attacker,Mob Defender)
        {
            this.Attacker = Attacker.Name;
            AttackRating = Attacker.AttackRating;
            this.Defender = Defender.Name;
            DefenseRating = Defender.DefenseRating;
            MinDamage = Attacker.MinDamage;
            MaxDamage = Attacker.MaxDamage;
        }
        public string Attacker { get; set;}
        public string Defender { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int AttackRating { get; set; }
        public int DefenseRating { get; set; }

    }
}
