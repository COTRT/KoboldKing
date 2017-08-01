using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts.Item;

namespace Assets.Scripts.Models.Enemies
{
    public class CreatureCatalog
    {
       //public int 
       //public int
       //public int
       //public int
       //public int
       //public int
       //public int
       //public int
       //public int
    }


    // TODO: WHAT EXP Value / level means?
    public class GoblinRangerMinion
    {
        public string MobName = "Goblin Ranger";
        public int CurrentHealth = 5;
        public int MaxHealth = 5;
        public int Level = 1;
        public int AttackRating = 1;
        public int DefenseRating = 0;
        public int LuckRating = 0;
        public int MeleeDamageBonus = -1;
        public int RangedDamageBonus = 1;
        public int MagicDamageBonus = 0;
        public int ExpValue = 50;
        public string MobTag = "Enemy";
        public string Prefab = "Goblin_Ranger_R1";
        public string Icon = "None";
        public List<int> Buffs = new List<int>();
        public MobTypes MobType = MobTypes.Goblin;
        public MobRanks MobRank = MobRanks.Minion;
    }

    public class GoblinRangerCommon
    {
        public string MobName = "Goblin Ranger";
        public int CurrentHealth = 8;
        public int MaxHealth = 8;
        public int Level = 1;
        public int AttackRating = 1;
        public int DefenseRating = 0;
        public int LuckRating = 0;
        public int MeleeDamageBonus = 1;
        public int RangedDamageBonus = 1;
        public int MagicDamageBonus = 0;
        public int ExpValue = 90;
        public string MobTag = "Enemy";
        public string Prefab = "Goblin_Ranger_R1";
        public string Icon = "None";
        public List<int> Buffs = new List<int>();
        public MobTypes MobType = MobTypes.Goblin;
        public MobRanks MobRank = MobRanks.Common;

    }


    public class Kobold
    {

    }


}
