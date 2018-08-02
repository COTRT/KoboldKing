using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine.SceneManagement;
using Assets.Scripts.Data;

public class GameController : MonoBehaviour {

	public static GameController controller;
    public DataService dataService;

	[SaveTo("inventory.dat")]
	public InventoryX inventory;

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
        dataService = new DataService(Application.persistentDataPath);
        //Sign up for log messages
        dataService.OnLogMessage += Debug.Log;
	}
	void Start(){
        dataService.Register(this, "GameController");
	}

	void OnGUI(){
		if(GUI.Button(new Rect(0,0,75,20),"Load All")){
			Stopwatch s = Stopwatch.StartNew();
			dataService.LoadAll();
			Debug.Log("All Loaded in "+s.ElapsedMilliseconds.ToString());
			
		}
		if(GUI.Button(new Rect(0,30,75,20),"Save All")){
			Stopwatch s = Stopwatch.StartNew();
			dataService.SaveAll();
			Debug.Log("All Saved in "+s.ElapsedMilliseconds.ToString());
		}
		if(GUI.Button(new Rect(0,60,75,20),"Scene 2")){
			SceneManager.LoadScene("Scene2");
		}
	}
}