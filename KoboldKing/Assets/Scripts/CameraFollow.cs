using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;

	// Use this for initialization
	void Start () {
        if (player != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
	}
	
	// Update is called once per frame after update
	void LateUpdate () {
        if (player != null)
        {
            transform.position = player.transform.position;
        }
	}
}
