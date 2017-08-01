using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Assets.Scripts;
using Assets.Scripts.Misc;

public class EnemyKillPlayer : MonoBehaviour
{
    private static System.Random s_Random = new System.Random();
    public Transform GameOverScreen;
    private GameObject myDamageable;
    private GameObject PlayerCombat;
    int randomPercent = s_Random.Next(0, 100);
    public int AttackerChance;
    int BaseHitChance = 50;
    int perCent;



    void OnTriggerEnter(Collider other)
    {
        perCent = s_Random.Next(0, 100);
        if (other.tag == "Player")
        {
            var DefenseChance = other.GetComponent<PlayerCombat>();
            int AttackChance = BaseHitChance + AttackerChance - DefenseChance.DefenseChance;
            if(perCent >= 95)
            {
                var component = other.GetComponent<Damageable>();
                if (component != null)
                    component.DealDamage(DamageType.Default, 50);
            }
            else if (perCent <= AttackChance)
            {
                var component = other.GetComponent<Damageable>();
                if (component != null)
                    component.DealDamage(DamageType.Default, 50);
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

