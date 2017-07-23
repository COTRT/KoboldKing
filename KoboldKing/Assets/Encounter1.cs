using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter1 : MonoBehaviour {
    public GameObject Ecounter1Spawn;
	// Use this for initialization
	void Start () {
		
	}
    public void OnTriggerEnter(Collider other)
    {
        Instantiate(Ecounter1Spawn);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
