using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TypeWriterEffect : MonoBehaviour {

	public float delay = 0.1f;
	private string fullText;
	private string currentText = "";
    public Text TextObject;
    public float WaitTime;
	// Use this for initialization
	void Start () {
        fullText = TextObject.text;
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText(){
        yield return new WaitForSeconds(WaitTime);
        for (int i = 0; i < fullText.Length; i++){
            currentText = fullText.Substring(0,i);
			this.GetComponent<Text>().text = currentText;
			yield return new WaitForSeconds(delay);
		}
	}
}
