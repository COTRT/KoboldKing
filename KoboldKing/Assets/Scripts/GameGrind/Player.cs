using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public CharacterStats characterStats;
    public int currentHealth;
    public int maxHealth;
    public PlayerLevel PlayerLevel { get; set; }

    void Start()
    {
        PlayerLevel = GetComponent<PlayerLevel>();
        this.currentHealth = this.maxHealth;
        characterStats = new CharacterStats(10, 10, 10);
        // GameGrind removed this becuase he could not find the null reference...
        // ...which we avoided because of the null checks in UIEventHandler.cs
        //UIEventHandler.HealthChanged(this.currentHealth, this.maxHealth);

    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Player takes " + amount + "points of damage.");
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }

        UIEventHandler.HealthChanged(this.currentHealth, this.maxHealth);


    }

    private void Die()
    {
        Debug.Log("Player dead.  Reset health.");
        this.currentHealth = this.maxHealth;
        UIEventHandler.HealthChanged(this.currentHealth, this.maxHealth);
    }
}
