using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {


    public GameObject Enemy;

    public int Count;
    public float Radius;


    //this is just for testing porposes will be deleted when we are ready to call this function elsewhere.
    void Start() {
        SpawnEnemy();


    }


    void SpawnEnemy()
    {
            //Spawns = Instantiate(Enemy, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;

    }
}

