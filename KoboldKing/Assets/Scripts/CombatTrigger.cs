using UnityEditor;
using Assets.Scripts;
using Assets.Scripts.Models;
using Assets.Scripts.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour {

    public float attackTimer;
    public float coolDown;
    public bool attackAgain = false;
    GameObject Player;	void Start () {
	// Use this for initialization
	void Start () {
        attackTimer = 0;
        coolDown = 2.0f;
	}
    private void OnTriggerEnter(Collider other)
    {
        CombatCalculator.Attack(gameObject, other.gameObject);
        attackTimer = coolDown;
        attackAgain = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player = other.gameObject;
            attackAgain = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        attackAgain = false;
    }

    // Update is called once per frame
    void Update () {
		if(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        if(attackTimer < 0)
        {
            attackTimer = 0;
        }
        if (attackTimer == 0 && attackAgain)
        {
            Attack();
        }
    }
}