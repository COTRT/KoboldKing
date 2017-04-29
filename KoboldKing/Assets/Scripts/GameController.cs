using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController controller;

	[SaveTo("inventory.dat")]
	public Inventory inventory;

	void Awake(){
        if (controller == null)
        {
            DontDestroyOnLoad(gameObject);
            controller = this;
        }
        else if (controller != this)
        {
            Destroy(gameObject);
        }
        //Give DataService the path it needs (since it isn't a monobehavoir, it doesn't have access to this stuff.)
        DataService.SaveDataPath = Application.persistentDataPath;
        //Sign up for log messages
        DataService.OnLogMessage += Debug.Log;
	}
	void Start(){
        DataService.Register(this, "GameController");
	}

	void OnGUI(){
		if(GUI.Button(new Rect(0,0,75,20),"Load All")){
			Stopwatch s = Stopwatch.StartNew();
			DataService.LoadAll();
			Debug.Log("All Loaded in "+s.ElapsedMilliseconds.ToString());
			
		}
		if(GUI.Button(new Rect(0,30,75,20),"Save All")){
			Stopwatch s = Stopwatch.StartNew();
			DataService.SaveAll();
			Debug.Log("All Saved in "+s.ElapsedMilliseconds.ToString());
		}
		if(GUI.Button(new Rect(0,60,75,20),"Scene 2")){
			SceneManager.LoadScene("Scene2");
		}
	}
}