using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonContoller : MonoBehaviour {

    public Transform canvas;


    public void MainMenu()
    {
        Application.LoadLevel(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Resume()
    {
        
            if (canvas.gameObject.activeInHierarchy == false)

            {
                canvas.gameObject.SetActive(true);
                Time.timeScale = 0;
            }

            else
            {
                canvas.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
        
    }
}
