using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;


public static class DataService
{
    public static Dictionary<string, ObjectSaveInfo> RegisteredObjects = new Dictionary<string, ObjectSaveInfo>();
    public static Dictionary<Type, TypeSaveInfo> TypeSaveInfos = new Dictionary<Type, TypeSaveInfo>();
    public static Dictionary<string, PathSaveInfo> PathSaveInfos = new Dictionary<string, PathSaveInfo>();
    private static List<string> claimedFolderNames = new List<string>();

    private static string _applicationPersistentDataPath;
    public static string ApplicationPersistentDataPath
    {
        private get
        {
            if(_applicationPersistentDataPath == null)
            {
                throw new NullReferenceException("ApplicationPesistentDataPath is null.  It must be set to the Application Persistent Data Path before Saving operations can occur");
            }
            return _applicationPersistentDataPath;
        }
        set
        {
            _applicationPersistentDataPath = value;
        }
    }

    public static List<string> ClaimedFolderNames
    {
        get
        {
            return claimedFolderNames;
        }

        private set
        {
            claimedFolderNames = value;
        }
    }

    public static void Register(object ObjectToRegister,string FolderName)
    {
        if (ClaimedFolderNames.Contains(FolderName))
        {
            throw new ArgumentException("Folder with name: " + FolderName + " already exists.  Please select a different folder name");
        }
        Type oType = ObjectToRegister.GetType();
        //We don't load all Type Infos off the bat; instead, we load the infos as soon an object of a certain type is registered, and then we add it to a dictionary so the TypeSaveInfo object only has to be created once.
        if (!TypeSaveInfos.ContainsKey(oType))
        {
            TypeSaveInfos.Add(oType, new TypeSaveInfo(oType));
        }
        ObjectSaveInfo osi = new ObjectSaveInfo(ObjectToRegister, TypeSaveInfos[oType], FolderName);
        RegisteredObjects.Add(FolderName, osi);
        ClaimedFolderNames.Add(FolderName);
        FindPaths(osi);
    }

    private static void FindPaths(ObjectSaveInfo osi)
    {
        string basePath = Path.Combine(ApplicationPersistentDataPath, osi.FolderName);
        foreach(var FieldPathGroup in osi.TypeSaveInfo.SavedFields)
        {
            PathSaveInfos.Add(FieldPathGroup.Value, new PathSaveInfo(osi, FieldPathGroup.Key));8i
        }
    }

    public static void SaveAll()
    {
        foreach (var RegisteredObject in RegisteredObjects)
        {
            Save(RegisteredObject.Value);
        }
    }

    public static void Save(string FolderName)
    {
        if (!RegisteredObjects.ContainsKey(FolderName))
        {
            throw new InvalidOperationException("Specified FolderName of "+FolderName+" is not Registered in DataService.  Please register all objects before saving them");
        }
        else
        {
            Save(RegisteredObjects[FolderName]);
        }
    }

    public static void Save(ObjectSaveInfo RegisteredObject)
    {
        foreach(string FieldName in RegisteredObject.TypeSaveInfo.SavedFields.Keys)
        {
            Save(RegisteredObject, FieldName);
        }
    }

    public static void Save(string FolderName, string FieldName)
    {
        if (!RegisteredObjects.ContainsKey(FolderName))
        {
            throw new InvalidOperationException("Specified FolderName of " + FolderName +" is not Registered in DataService.  Please register all FolderNames before saving them");
        }
        else
        {
            Save(RegisteredObjects[FolderName],FieldName);
        }
    }

    public static void Save(ObjectSaveInfo RegisteredObject,string FieldName)
    {
        if (RegisteredObject.TypeSaveInfo.SavedFields.ContainsKey(FieldName))
        {
            //Ugh.  C# 4.
            string filepath = new[] { ApplicationPersistentDataPath, RegisteredObject.FolderName, RegisteredObject.TypeSaveInfo.SavedFields[FieldName] }.Aggregate(Path.Combine);
            
            Save(RegisteredObject,RegisteredObject.Object.GetType().GetField(FieldName), filepath);
        }
        else
        {
            throw new KeyNotFoundException("The Field " + FieldName + " Could not be saved because it has no relevant file mapping.  (In other words, this field either doesn't exist in this Object/Type, or doesn't have a SaveAttribute)");
        }
    }

    private static void Save(ObjectSaveInfo RegisteredObject, FieldInfo fieldinfo, string filepath)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.Open(filepath, FileMode.OpenOrCreate);
        saveFile.SetLength(0);
        bf.Serialize(saveFile, fieldinfo.GetValue(RegisteredObject.Object));
        saveFile.Close();
    }

    public static void LoadAll()
    {
        foreach (var saveFile in Directory.GetFiles(ApplicationPersistentDataPath))
        {
            string filename = Path.GetFileName(saveFile);
            if (ReversedSaveFieldMappings.ContainsKey(filename))
            {
                SimpleLoad(this.GetType().GetField(ReversedSaveFieldMappings[filename]), saveFile);
            }
        }
    }

    public static void SimpleLoad(string filename)
    {
        if (ReversedSaveFieldMappings.ContainsKey(filename))
        {
            SimpleLoad(this.GetType().GetField(ReversedSaveFieldMappings[filename]), filename);
        }
        else
        {
            throw new KeyNotFoundException("File:  " + filename + " cannot be loaded, because it has no relevant type mapping.  (In other words, no Field has a SaveAttribute that saves to this file)");
        }
    }
    protected static void SimpleLoad(FieldInfo fieldinfo, string filename)
    {
        string filePath = Path.Combine(Application.persistentDataPath, filename);
        Debug.Log("Loading " + filename);
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream loadFile = File.Open(filePath, FileMode.Open);
            fieldinfo.SetValue(this, bf.Deserialize(loadFile));
            loadFile.Close();
        }
        else
        {
            Debug.LogWarning("Field to Load:  " + fieldinfo.Name + " cannot be loaded because it's relevant load file has not been created yet.");
        }
    }
}
