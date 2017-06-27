using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillPlayer : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "Player") {
            Destroy(other.gameObject);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
