using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour {
    private GameObject Player;
    public Transform Canvas;
    public bool present = false;
    public Transform Canvas2;

    void OnTriggerEnter (Collider other) {

        Player = GameObject.FindGameObjectWithTag("Player");

        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(Canvas);
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (present == false)
                {
                    Instantiate(Canvas2);
                    present = true;
                    Time.timeScale = 0;
                }
                else
                {
                    DestroyObject(GameObject.FindGameObjectWithTag("PauseMenu"));
                    present = false;
                    Time.timeScale = 1;
                }
            }
        }

    }
}
