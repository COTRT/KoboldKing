using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

   public bool quit = false;
    public bool load = false;
    void OnMouseEnter()
    {
        GetComponent<Renderer>().materials[0].color = Color.red;

    }
    void OnMouseExit()
    {
        GetComponent<Renderer>().materials[0].color = Color.white;
    }
    void OnMouseUp()
    {
        if (quit == true)
        {
            Application.Quit(); //If you click on quit aplication quits.
        }
        else if (load == true)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(1); //If you click on other button it loads game!
        }
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) //If you press (escape) game force closes!
        {
            Application.Quit();
        }
    }
}