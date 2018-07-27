using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
        }

        //if (gameObject.tag == "Enemy")
        //{

        //    if (other.tag == "Player")
        //    {
        //        Destroy(other.gameObject);
        //    }
        //}
    }
}
