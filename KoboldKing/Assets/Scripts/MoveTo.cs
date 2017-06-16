using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {
    public Transform Goal;

    private NavMeshAgent navMeshAgent;
	// Use this for initialization
	void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        navMeshAgent.SetDestination(Goal.position);
	}
}
