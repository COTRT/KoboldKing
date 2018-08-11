using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Quests
{
    public class KillGoal : Goal
    {
        private string enemyId;
        public KillGoal(Dictionary<string, string> arguments) : base(arguments)
        {
            CheckConfigContains(arguments, "Enemy");
            enemyId = arguments["Enemy"];
            CombatEvents.OnEnemyDeath += OnEnemyDeath;
        }

        private void OnEnemyDeath(IEnemy enemy)
        {
            if (enemy.Id == enemyId) CurrentAmount++;
        }

        public override string Description { get; set; }
        public override int CurrentAmount { get; set; }
        public override int RequiredAmount { get; set; }
    }
}
