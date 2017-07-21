using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Assets.Scripts;

public class EnemyKillPlayer : MonoBehaviour
{
    public Transform GameOverScreen;
    private GameObject myDamageable;

    

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var component = other.GetComponent<Damageable>();
            if(component!=null)
                component.DealDamage(DamageType.Default, 50);
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

