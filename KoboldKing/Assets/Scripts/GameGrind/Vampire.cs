using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine.AI;
using UnityEngine;

public class Vampire : Interactable, IEnemy
{

    public LayerMask aggroLayerMask;
    public float currentHealth;
    public float maxHealth;
    public int Id { get; set; }
    public int Experience { get; set; }
    public DropTable DropTable { get; set; }
    public PickupItem pickupItem;
    public Spawner Spawner { get; set; }


    private Player player;
    private const int EnemyDetectRadius = 10;
    private const int EnemyAttackDamage = 5;
    private const int EnemyAttackSpeed = 2;
    private NavMeshAgent navAgent;
    private CharacterStats characterStats;
    private Collider[] withinAggroColliders;



    void Start()
    {
        DropTable = new DropTable();
        DropTable.loot = new List<LootDrop>()
        {
            new LootDrop("sword", 25),
            new LootDrop("staff", 25),
            new LootDrop("potion_log", 25)
        };

        Id = 1;
        Experience = 300;
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

    public void PerformAttack()
    {
        Debug.Log("Enemy attacks for " + EnemyAttackDamage + " points of damage.");
        player.TakeDamage(EnemyAttackDamage);
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

    public void Die()
    {
        DropLoot();
        CombatEvents.EnemyDied(this);
        this.Spawner.Respawn();
        Destroy(gameObject);
    }

    void DropLoot()
    {
        Item item = DropTable.GetDrop();
        if (item != null)
        {
            PickupItem instance = Instantiate(pickupItem, transform.position, Quaternion.identity);
            instance.ItemDrop = item;
        }
    }

}
