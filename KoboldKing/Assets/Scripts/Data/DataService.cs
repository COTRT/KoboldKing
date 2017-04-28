using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class DataService
{
    private static Dictionary<string, ObjectSaveInfo> RegisteredObjects = new Dictionary<string, ObjectSaveInfo>();
    private static Dictionary<Type, TypeSaveInfo> TypeSaveInfos = new Dictionary<Type, TypeSaveInfo>();
    private static Dictionary<string, PathSaveInfo> PathSaveInfos = new Dictionary<string, PathSaveInfo>();
    private static Dictionary<PathSaveInfo, string> ReversedPathSaveInfos = new Dictionary<PathSaveInfo, string>(new FieldAndObjectPathSaveInfoComparer());

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
    
    public static bool ObjectNameIsRegistered(string objectName)
    {
        return RegisteredObjects.ContainsKey(objectName);
    }

    private static string GetNonDuplicate(string folderName)
    {
        int currentSuffix = 0;
        while (true)
        {
            string attempt = folderName + "_" + currentSuffix.ToString();
            if (RegisteredObjects.ContainsKey(attempt))
            {
                currentSuffix++;
            }
            else
            {
                return attempt;
            }
        }
    }

    /// <summary>
    /// Register the specified object for saving.  All objects MUST be saved before any of thier data can be saved.
    /// </summary>
    /// <param name="ObjectToRegister">The object to be registered for saving.</param>
    /// <param name="IdentificationName">The NAME this object should be stored and referenced as.  To save data, each object (with SaveTo fields in it to save) MUST be registered with a unique identification name.  To check if a name is already registered, use the ObjectNameIsRegistered function.</param>
    /// <param name="AutomaticNoDuplicateSuffixes">With this parameter set to true, DataService will automatically add unique suffixes (for example, "IdName" becomes "IdName_0", "IdName_1","IdName_2", etc.) if the supplied IdentificationName already exists.  If this is false, Duplicate Identification Names will throw an ArgumentException</param>
    /// <exception cref="ArgumentException">Throws if a duplicate IdentificationName is supplied, and AutomaticNoDuplicateSuffixes is false. Use ObjectNameIsRegistered to ensure IdentificationNames aren't duplicates.</exception>
    /// <returns>The Identitification Name of the new object (If AutomaticNoDuplicateSuffixes is false, this will always be equal to the supplied IdentificationName)</returns>
    /// <seealso cref="ObjectNameIsRegistered(string)"/>
    public static string Register(object ObjectToRegister,string IdentificationName,bool AutomaticNoDuplicateSuffixes = false)
    {
        string FolderName = IdentificationName;
        if (RegisteredObjects.ContainsKey(FolderName))
        {
            if (AutomaticNoDuplicateSuffixes)
            {
                FolderName = GetNonDuplicate(FolderName);   
            }
            else
            {
                throw new ArgumentException("Folder with name: " + FolderName + " already exists.  Please select a different folder name");
            }
        }
        Type oType = ObjectToRegister.GetType();
        //We don't load all Type Infos off the bat; instead, we load the infos as soon an object of a certain type is registered, and then we add it to a dictionary so the TypeSaveInfo object only has to be created once.
        if (!TypeSaveInfos.ContainsKey(oType))
        {
            TypeSaveInfos.Add(oType, new TypeSaveInfo(oType));
        }
        ObjectSaveInfo osi = new ObjectSaveInfo(ObjectToRegister, TypeSaveInfos[oType], FolderName);
        RegisteredObjects.Add(FolderName, osi);
        FindPaths(osi);
        return FolderName;
    }

    private static void FindPaths(ObjectSaveInfo osi)
    {
        string basePath = Path.Combine(ApplicationPersistentDataPath, osi.FolderName);
        foreach(var FieldPathGroup in osi.TypeSaveInfo.SavedFields)
        {
            string filepath = Path.Combine(basePath, FieldPathGroup.Value.filename);
            PathSaveInfo pathSaveInfo = new PathSaveInfo(osi, FieldPathGroup.Key, filepath);
            PathSaveInfos.Add(filepath, pathSaveInfo);
            ReversedPathSaveInfos.Add(pathSaveInfo, filepath);
        }
    }

    public static void SaveAll()
    {
        foreach (var RegisteredObject in RegisteredObjects.Values)
        {
            Save(RegisteredObject);
        }
    }

    public static void Save(string FolderName, bool throwIfObjectNotRegistered = true)
    {
        if (!RegisteredObjects.ContainsKey(FolderName))
        {
            ThrowNotRegisteredException(FolderName, "Object",throwIfObjectNotRegistered);
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
            Save(RegisteredObject, RegisteredObject.TypeSaveInfo.SavedFields[FieldName]);
        }
    }

    public static void Save(string FolderName, string FieldName,bool throwIfObjectNotRegistered=true, bool throwIfFieldNotRegistered= true)
    {
        if (!RegisteredObjects.ContainsKey(FolderName))
        {
            ThrowNotRegisteredException(FolderName, "Object", throwIfObjectNotRegistered);
        }
        else
        {
            Save(RegisteredObjects[FolderName],FieldName,throwIfFieldNotRegistered);
        }
    }

    public static void Save(ObjectSaveInfo RegisteredObject,string FieldName,bool throwIfFieldNotRegistered = true)
    {
        if (RegisteredObject.TypeSaveInfo.SavedFields.ContainsKey(FieldName))
        {
            FieldSaveInfo fieldSaveInfo = RegisteredObject.TypeSaveInfo.SavedFields[FieldName];            
            Save(RegisteredObject,fieldSaveInfo);
        }
        else
        {
            ThrowNotRegisteredException(FieldName, "Field",throwIfFieldNotRegistered);
        }
    }

    private static void Save(ObjectSaveInfo RegisteredObject, FieldSaveInfo fieldSaveInfo)
    {
        PathSaveInfo psi = new PathSaveInfo(RegisteredObject, fieldSaveInfo.field.Name);
        if (!ReversedPathSaveInfos.ContainsKey(psi))
        {
            //If this happens, it's time to complain to the Tech Lead.
            throw new KeyNotFoundException("Internal Path Logic Error.  No Path Could be found (to SAVE to) for Registered Object:  " + RegisteredObject.FolderName + " Field:  " + fieldSaveInfo.field.Name + ", most likely because of an internal path handling error.");
        }
        else
        {
            psi.Filepath = ReversedPathSaveInfos[psi];
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.Open(psi.Filepath, FileMode.OpenOrCreate);
        saveFile.SetLength(0);
        bf.Serialize(saveFile, fieldSaveInfo.field.GetValue(RegisteredObject.Object));
        saveFile.Close();
    }

    public static void LoadAll()
    {
        foreach (var loadDir in Directory.GetDirectories(ApplicationPersistentDataPath))
        {
            string foldername = Path.GetFileName(loadDir);
            if (RegisteredObjects.ContainsKey(foldername))
            {
                Load(RegisteredObjects[foldername],false);
            }
        }
    }

    public static void Load(string objectName,bool throwIfNotRegistered =true,bool throwIfNotExists = true)
    {
        if (RegisteredObjects.ContainsKey(objectName))
        {
            Load(RegisteredObjects[objectName],throwIfNotExists);
        }
        else
        {
            ThrowNotRegisteredException(objectName, "Object",throwIfNotRegistered);
        }
    }

    public static void Load(ObjectSaveInfo RegisteredObject, bool throwIfNotExists = true)
    {
        foreach(var field in RegisteredObject.TypeSaveInfo.SavedFields)
        {
            Load(RegisteredObject, field.Value,throwIfNotExists);
        }
    }
    public static void Load(string objectName, string FieldName, bool throwIfNotRegistered = true, bool throwIfNotExists = true)
    {
        if (RegisteredObjects.ContainsKey(objectName))
        {
            Load(RegisteredObjects[objectName], FieldName,throwIfNotExists);
        }
        else
        {
            ThrowNotRegisteredException(objectName, "Object", throwIfNotRegistered);
        }
    }
    public static void Load(ObjectSaveInfo RegisteredObject, string FieldName,bool throwIfNotExists = true)
    {
        FieldSaveInfo fieldSaveInfo = RegisteredObject.TypeSaveInfo.SavedFields[FieldName];
        Load(RegisteredObject, fieldSaveInfo,throwIfNotExists);
    }
    private static void Load(ObjectSaveInfo RegisteredObject,FieldSaveInfo fieldSaveInfo,bool throwIfNotExists)
    {
        string filePath = Path.Combine(ApplicationPersistentDataPath, fieldSaveInfo.filename);
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream loadFile = File.Open(filePath, FileMode.Open);
            fieldSaveInfo.field.SetValue(RegisteredObject.Object, bf.Deserialize(loadFile));
            loadFile.Close();
        }
        else
        {
            throw new FileNotFoundException("Could not load file " + filePath + " into Field " + fieldSaveInfo.field.Name + " because the desired file was not found");
        }
    }

    private static void ThrowNotRegisteredException(string objectName,string unregisteredItemTypeName,bool throwIfNotRegistered)
    {
        if (throwIfNotRegistered)
            throw new KeyNotFoundException(unregisteredItemTypeName+":  " + objectName + " cannot be loaded, because it has no relevant Registered "+unregisteredItemTypeName +".  (In other words, you need to register the "+unregisteredItemTypeName+" you're trying to load/save before saying, 'LOAD IT!!' [Or 'SAVE IT!!'])");
        else
            return;//Insert whatever log logic we come up with here
    }
}
