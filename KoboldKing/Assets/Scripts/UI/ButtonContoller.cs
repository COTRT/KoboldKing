using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonContoller : MonoBehaviour
{

    public GameObject Canvas;

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);

    }
    public void Quit()
    {
        Application.Quit();

    }
    public void GoToGame()
    {
        SceneManager.LoadScene(1);

    }
    public void Resume()
    {
        Object.Destroy(GameObject.FindGameObjectWithTag("PauseMenu"));
        Time.timeScale = 1;
    }

}
