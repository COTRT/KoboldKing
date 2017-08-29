using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Models
{
    public class CombatResult : MonoBehaviour
    {
        public string Attacker { get; set; }
        public string Defender { get; set; }
        public bool Hit { get; set; }
        public int Damage { get; set; }
    }
}
