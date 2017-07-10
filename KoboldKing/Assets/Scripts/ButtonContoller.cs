using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonContoller : MonoBehaviour {

    public GameObject Canvas;

   public void MainMenu()
    {
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
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        Canvas.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

}
