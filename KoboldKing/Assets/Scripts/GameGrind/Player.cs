using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public CharacterStats characterStats;
    public float currentHealth;
    public float maxHealth;

    void Start()
    {
        this.currentHealth = this.maxHealth;
        characterStats = new CharacterStats(10, 10, 10);
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Player takes " + amount + "points of damage.");
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        Debug.Log("Player dead.  Reset health.");
        this.currentHealth = this.maxHealth;
    }
}
