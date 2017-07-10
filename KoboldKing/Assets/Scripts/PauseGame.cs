using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

    public Transform canvas;
    public bool present = false;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (present == false)
            {
                Instantiate(canvas);
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
