using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Assets.Scripts.Data;
using Assets.Scripts.Managers;

public class DataManager : ManagerBase
{

    private string _filename;

    public override void Startup(DataService dataService)
    {
        Debug.Log("Data manager starting...");

        // construct a full path to the game.dat file.
        _filename = Path.Combine(Application.persistentDataPath, "game.dat");

        // any long-running startup tasks go here, and set status to 'Initializing' until those tasks are complete
        Status = ManagerStatus.STARTED;
    }

    public void SaveGameState()
    {
        // dictionary that will be serialized.
        Dictionary<string, object> gamestate = new Dictionary<string, object>();
        gamestate.Add("inventory", Managers.Inventory.GetData());
        gamestate.Add("health", Managers.Player.Health);
        gamestate.Add("maxHealth", Managers.Player.MaxHealth);
        gamestate.Add("curLevel", Managers.Mission.CurLevel);
        gamestate.Add("maxLevel", Managers.Mission.MaxLevel);

        // create a file at the file path.
        FileStream stream = File.Create(_filename);
        BinaryFormatter formatter = new BinaryFormatter();

        // serialize the dictionary as contents of the created file.
        formatter.Serialize(stream, gamestate);
        stream.Close();
    }

    public void LoadGameState()
    {
        // only continue to load if the file exists.
        if (!File.Exists(_filename))
        {
            Debug.Log("No saved game");
            return;
        }

        // dictionary to put loaded data in.
        Dictionary<string, object> gamestate;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(_filename, FileMode.Open);
        gamestate = formatter.Deserialize(stream) as Dictionary<string, object>;
        stream.Close();

        // update managers with deserialized data.
        Managers.Inventory.UpdateData((Dictionary<string, int>)gamestate["inventory"]);
        Managers.Player.UpdateData((int)gamestate["health"], (int)gamestate["maxHealth"]);
        Managers.Mission.UpdateData((int)gamestate["curLevel"], (int)gamestate["maxLevel"]);
        Managers.Mission.RestartCurrent();
    }
}
