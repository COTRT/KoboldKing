﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGKillGoal : GGGoal
{
    public string EnemyId { get; set; }


    public GGKillGoal(GGQuest quest, string enemyId, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.Quest = quest;
        this.EnemyId = enemyId;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();
        CombatEvents.OnEnemyDeath += EnemyDied;
    }

    void EnemyDied(IEnemy enemy)
    {
        if (enemy.Id == this.EnemyId)
        {
            this.CurrentAmount++;
            Evaluate();
        }
    }
}
