using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Assets.Scripts;

public class EnemyKillPlayer : MonoBehaviour {
    public Transform GameOverScreen;

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "Enemy") {

            if (other.tag == "Player")
            {
                Destroy(other.gameObject);
                GameOverScreen.gameObject.SetActive(true);
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
