using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController controller;
    ///<summary>
	/// Field Name v. destination path
	///</summary>
	private static Dictionary<string,string> SaveFieldMappings;
	///<summary>
	///	destination path v. Field Name
	///</summary>
	private static Dictionary<string,string> ReversedSaveFieldMappings;

	[SaveTo("inventory.dat")]
	public Inventory inventory;

	void Awake(){
		if(controller == null){
			DontDestroyOnLoad(gameObject);
			controller = this;
		}
		else if(controller != this){
			Destroy(gameObject);
		}
	}
	void Start(){
	    
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