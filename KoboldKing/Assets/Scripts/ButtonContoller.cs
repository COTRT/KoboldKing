using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonContoller : MonoBehaviour {

    public GameObject Canvas;

   public void MainMenu()
    {
        Time.timeScale = 1;
        Application.LoadLevel(0);

    }
    public void Quit()
    {
        Application.Quit();

    }
    public void GoToGame()
    {
        Application.LoadLevel(1);

    }
    public void Resume()
    {
        DestroyObject(GameObject.FindGameObjectWithTag("PauseMenu"));
        Time.timeScale = 1;
    }

}
