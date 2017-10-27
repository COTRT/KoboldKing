namespace Assets.Scripts.Models
{
    public class CombatResult
    {
        public string Attacker { get; set; }
        public string Defender { get; set; }
        public bool Hit { get; set; }
        public int Damage { get; set; }
    }
}
