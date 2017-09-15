using UnityEditor;
using Assets.Scripts;
using Assets.Scripts.Models;
using Assets.Scripts.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        CombatCalculator.Attack(gameObject, other.gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
//public string Attacker { get; set; }
//public string Defender { get; set; }
//public int MinDamage { get; set; }
//public int MaxDamage { get; set; }
//public int AttackRating { get; set; }
//public int DefenseRating { get; set; }
