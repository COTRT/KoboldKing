using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashFade : MonoBehaviour
{
    public Image[] splashImage;
    public string loadLevel;
    public int howLong;


    IEnumerator Start()
    {

        splashImage[0].canvasRenderer.SetAlpha(0.0f);
        splashImage[1].canvasRenderer.SetAlpha(0.0f);
        splashImage[2].canvasRenderer.SetAlpha(0.0f);


        for (int i = 0; i<= splashImage.Length; i++)
        {
            FadeIn(i);
            yield return new WaitForSeconds(howLong);
            FadeOut(i);
            yield return new WaitForSeconds(4f);
        }
        SceneManager.LoadScene(loadLevel);
        splashImage[2].canvasRenderer.SetAlpha(0.0f);


    }

    void FadeIn(int I)
    {
        splashImage[I].CrossFadeAlpha(1.0f, 1.5f, false);

    }

    void FadeOut(int i)
    {
        splashImage[i].CrossFadeAlpha(0.0f, 2.5f, false);
       
    }
 
}
