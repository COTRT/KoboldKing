using System.Collections;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class RandomHighlightedButtonColorGenerator : MonoBehaviour
{
    public Vector2 HueRange; //incorrect class usage on a seriously incorrect component
    public Vector2 SaturationRange;
    public Vector2 ValueRange;
    void Start()
    {
        float hue = Random.Range(HueRange.x, HueRange.y);
        float saturation = Random.Range(SaturationRange.x, SaturationRange.y);
        float value = Random.Range(ValueRange.x, ValueRange.y);
        Debug.LogFormat("H: {0}, S:  {1}, V:  {2}", hue, saturation, value);
        Button button = GetComponent<Button>();
        var colors = button.colors;
        colors.highlightedColor = Color.HSVToRGB(hue, saturation, value);
        button.colors = colors;
     }
}
