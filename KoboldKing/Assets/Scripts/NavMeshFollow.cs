using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class NavMeshFollow : MonoBehaviour {
    public Transform target;
    NavMeshAgent agent;
    private GameObject player;
	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void LateUpdate () {

        if (player != null)
        {
            //agent.GetComponent<NavMeshFollow>().destination = target.transform.position;
            agent.SetDestination(player.transform.position);
        }
    }
}
