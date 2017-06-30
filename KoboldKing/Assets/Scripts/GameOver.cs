using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {
    public Transform GameOverScreen;
    private GameObject player;
         
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");



        if (player == null)
        {
            if (GameOverScreen.gameObject.activeInHierarchy == false)
            {
                GameOverScreen.gameObject.SetActive(true);
                
            }
            else
            {
                GameOverScreen.gameObject.SetActive(false);
            }
        }
    }
}
