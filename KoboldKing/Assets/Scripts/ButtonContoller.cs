using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonContoller : MonoBehaviour {

    public void MainMenu()
    {
        Application.LoadLevel(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
