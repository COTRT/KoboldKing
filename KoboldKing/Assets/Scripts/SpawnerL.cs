using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerL : MonoBehaviour {


    public GameObject Enemy;
    public Transform transformLocation;
    public GameObject Spawns;



    //this is just for testing porposes will be deleted when we are ready to call this function elsewhere.
    void Start() {
        SpawnEnemy();


    }


    void SpawnEnemy()
    {
            Spawns = Instantiate(Enemy, transformLocation.position, Quaternion.Euler(0, 0, 0)) as GameObject;

    }
}

