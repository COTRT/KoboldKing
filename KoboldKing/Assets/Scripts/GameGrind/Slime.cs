using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Interactable, IEnemy
{
    public float currentHealth, power, toughness;
    public float maxHealth;
    private CharacterStats characterStats;

    void Start()
    {
        characterStats = new CharacterStats(6, 10, 2);
        currentHealth = maxHealth;
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void PerformAttack()
    {
        throw new System.NotImplementedException();
    }
}
