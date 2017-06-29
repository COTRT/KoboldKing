using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class NavMeshFollow : MonoBehaviour {
    public Transform target;
    NavMeshAgent agent;
	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        //agent.GetComponent<NavMeshFollow>().destination = target.transform.position;
        agent.SetDestination(target.position);
    }
}
