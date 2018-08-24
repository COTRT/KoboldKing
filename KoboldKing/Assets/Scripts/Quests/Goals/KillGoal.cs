using Assets.Scripts.Quests;
using System.Collections.Generic;

public class KillGoal : Goal
{
    private readonly string enemyId;
    public KillGoal(Dictionary<string, string> arguments) : base(arguments)
    {
        arguments.CheckConfigContains("Enemy");
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

