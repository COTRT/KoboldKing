using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour {
    public GameObject Player;
    public GameObject Enemy;
    public MusicTrigger MusicTrigger;

    // Use this for initialization
    void Start () {
        Instantiate(Player);
        Instantiate(Enemy);
        Instantiate(MusicTrigger);


    }

    // Update is called once per frame
    void Update () {
		
	}
}
