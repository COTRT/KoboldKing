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
		//Now, this was a fun one.
		//Little LINQ, little Attributes, whole lotta Reflection...  Yeah.
		initializeSaver();
	}

	void OnGUI(){
		if(GUI.Button(new Rect(0,0,75,20),"Load All")){
			Stopwatch s = Stopwatch.StartNew();
			LoadAll();
			Debug.Log("All Loaded in "+s.ElapsedMilliseconds.ToString());
			
		}
		if(GUI.Button(new Rect(0,30,75,20),"Save All")){
			Stopwatch s = Stopwatch.StartNew();
			SaveAll();
			Debug.Log("All Saved in "+s.ElapsedMilliseconds.ToString());
		}
		if(GUI.Button(new Rect(0,60,75,20),"Scene 2")){
			SceneManager.LoadScene("Scene2");
		}
	}

	#region saving logic
	private void initializeSaver(){
		SaveFieldMappings = new Dictionary<string,string>();
		foreach(var Field in this.GetType().GetFields().Where(f => Attribute.IsDefined(f,typeof(SaveToAttribute)))){
			SaveFieldMappings.Add(Field.Name,Field.GetCustomAttributes(typeof(SaveToAttribute),false).Single().ToString());
		}
		ReversedSaveFieldMappings = SaveFieldMappings.ToDictionary(sfm => sfm.Value, sfm => sfm.Key);
	}
	public void SaveAll(){
		Debug.Log(SaveFieldMappings);
		foreach(var saveFieldMapping in SaveFieldMappings){
			SimpleSave(this.GetType().GetField(saveFieldMapping.Key),saveFieldMapping.Value);
		}
	}

	public void SimpleSave(string FieldName){
		if(SaveFieldMappings.ContainsKey(FieldName)){
			SimpleSave(this.GetType().GetField(FieldName),SaveFieldMappings[FieldName]);
		}else{
			throw new KeyNotFoundException("The Field "+FieldName+" Could not be saved because it has no relevant file mapping.  (In other words, this field needs to have a SaveAttribute on it)");
		}
	}

	protected void SimpleSave(FieldInfo fieldinfo, string filename){
		Debug.Log("Saving "+filename);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream saveFile = File.Open(Path.Combine(Application.persistentDataPath,filename),FileMode.OpenOrCreate);
		saveFile.SetLength(0);
		bf.Serialize(saveFile,fieldinfo.GetValue(this));
		saveFile.Close();
	}
	
	public void LoadAll(){
		foreach(var saveFile in Directory.GetFiles(Application.persistentDataPath)){
			string filename = Path.GetFileName(saveFile);
			if(ReversedSaveFieldMappings.ContainsKey(filename)){
				SimpleLoad(this.GetType().GetField(ReversedSaveFieldMappings[filename]),saveFile);
			}
		}
	}
	
	public void SimpleLoad(string filename){
		if(ReversedSaveFieldMappings.ContainsKey(filename)){
			SimpleLoad(this.GetType().GetField(ReversedSaveFieldMappings[filename]),filename);
		}else{
			throw new KeyNotFoundException("File:  "+filename+" cannot be loaded, because it has no relevant type mapping.  (In other words, no Field has a SaveAttribute that saves to this file)");
		}
	}
	protected void SimpleLoad(FieldInfo fieldinfo,string filename){
		string filePath = Path.Combine(Application.persistentDataPath,filename);
		Debug.Log("Loading "+filename);
		if(File.Exists(filePath)){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream loadFile = File.Open(filePath,FileMode.Open);
			fieldinfo.SetValue(this,bf.Deserialize(loadFile));
			loadFile.Close();
		}else{
			Debug.LogWarning("Field to Load:  "+fieldinfo.Name+" cannot be loaded because it's relevant load file has not been created yet.");
		}
	}
	#endregion
}