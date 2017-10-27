using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {
    public Transform GameOverScreen;
    private GameObject player;
    public bool done = false;
    GameObject EventManager;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");


        if (!done) {
            if (player == null)
            {

                Instantiate(GameOverScreen);
                done = true;

                EventManager = GameObject.FindGameObjectWithTag("Event Manager");
                AudioSource audioSource = EventManager.GetComponent<AudioSource>();
                audioSource.enabled = false;

                //if (GameOverScreen.gameObject.activeInHierarchy == false)
                //{
                //    GameOverScreen.gameObject.SetActive(true);

                //}
                //else
                //{
                //    GameOverScreen.gameObject.SetActive(false);
                //}
            }
        }
    }
}
