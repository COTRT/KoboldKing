using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashFade : MonoBehaviour
{
    public Image[] splashImages;
    public string loadLevel;
    public int howLong;


    IEnumerator Start()
    {
        foreach(Image splashImage in splashImages)
        {
            splashImage.canvasRenderer.SetAlpha(0.0f);

        }

        foreach(Image splashImage in splashImages)
        {
            FadeIn(splashImage);
            yield return new WaitForSeconds(howLong);
            FadeOut(splashImage);
            yield return new WaitForSeconds(4f);
        }
        SceneManager.LoadScene(loadLevel);


    }

    void FadeIn(Image image)
    {
        image.CrossFadeAlpha(1.0f, 1.5f, false);

    }
    
    void FadeOut(Image image)
    {
        image.CrossFadeAlpha(0.0f, 2.5f, false);
       
    }
 
}
