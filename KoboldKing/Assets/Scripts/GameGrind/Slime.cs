using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine.AI;
using UnityEngine;

public class Slime : Interactable, IEnemy
{

    public LayerMask aggroLayerMask;
    public float currentHealth;
    public float maxHealth;


    private Player player;
    private const int EnemyDetectRadius = 10;
    private const int EnemyAttackDamage = 5;
    private const int EnemyAttackSpeed = 2;
    private NavMeshAgent navAgent;
    private CharacterStats characterStats;
    private Collider[] withinAggroColliders;



    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        characterStats = new CharacterStats(6, 10, 2);
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        withinAggroColliders = Physics.OverlapSphere(transform.position, EnemyDetectRadius, aggroLayerMask);
        if (withinAggroColliders.Length > 0)
        {
            ChasePlayer(withinAggroColliders[0].GetComponent<Player>());
        }
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }


    void ChasePlayer(Player player)
    {
        navAgent.SetDestination(player.transform.position);
        this.player = player;
        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            if (!IsInvoking("PerformAttack"))
            {
                InvokeRepeating("PerformAttack", .5f, EnemyAttackSpeed);
            }
        }
        else
        {
            CancelInvoke("PerformAttack");
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void PerformAttack()
    {
        Debug.Log("Enemy attacks for " + EnemyAttackDamage + " points of damage.");
        player.TakeDamage(EnemyAttackDamage);
    }
}
