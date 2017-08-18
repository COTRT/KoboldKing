using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour {
    public GameObject[] Encounters;
    public GameObject Player;
    public GameObject Enemy;

    // Use this for initialization
    void Start () {
        Instantiate(Player);
        Instantiate(Enemy);
        InstantiateAllEncounters();



    }
    void InstantiateAllEncounters()
    {
        for(int i = 0; i < Encounters.Length; i++)
        {
            Instantiate(Encounters[i]);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
