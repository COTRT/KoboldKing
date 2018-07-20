using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Scripts.Data
{
    public class DataService
    {
        private Dictionary<string, ObjectSaveInfo> RegisteredObjects = new Dictionary<string, ObjectSaveInfo>();
        private Dictionary<Type, TypeSaveInfo> TypeSaveInfos = new Dictionary<Type, TypeSaveInfo>();
        private Dictionary<PathSaveInfo, string> ReversedPathSaveInfos = new Dictionary<PathSaveInfo, string>(new FieldAndObjectPathSaveInfoComparer());

        private string _saveDataPath;
        public string SaveDataPath
        {
            private get
            {
                if (_saveDataPath == null)
                {
                    throw new NullReferenceException("ApplicationPesistentDataPath is null.  It must be set to the Application Persistent Data Path before Saving, Registering, or Loading operations can occur");
                }
                return _saveDataPath;
            }
            set
            {
                _saveDataPath = value;
            }
        }
        public event LogFunc OnLogMessage;
        public DataService(string SaveDataPath)
        {
            this.SaveDataPath = SaveDataPath;
        }
        

        /// <summary>
        /// This function can be used from user code to create custom Identification/Folder names for objects without a) duplicates, or b) using the "AutomaticNoDuplicateSuffixes" option (under <see cref="Register(object, string, bool)"/>) to automatically add suffixes.
        /// </summary>
        /// <param name="identificationName">An Identification Name.</param>
        /// <returns>True if the objectName has already been taken by a registered object, and false if not</returns>
        public bool ObjectNameIsRegistered(string identificationName)
        {
            return RegisteredObjects.ContainsKey(identificationName);
        }

        private string GetNonDuplicate(string identificationName)
        {
            int currentSuffix = 0;
            while (true)
            {
                string attempt = identificationName + "_" + currentSuffix.ToString();
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
        private void Log(object message)
        {
            if (OnLogMessage != null)
                OnLogMessage.Invoke(message);
        }

        /// <summary>
        /// Register the specified object for saving.  All objects MUST be saved before any of thier data can be saved.
        /// </summary>
        /// <param name="objectToRegister">The object to be registered for saving.</param>
        /// <param name="identificationName">The NAME this object should be stored and referenced as.  To save data, each object (with SaveTo fields in it to save) MUST be registered with a unique identification name.  To check if a name is already registered, use the ObjectNameIsRegistered function.</param>
        /// <param name="automaticNoDuplicateSuffixes">With this parameter set to true, DataService will automatically add unique suffixes (for example, "IdName" becomes "IdName_0", "IdName_1","IdName_2", etc.) if the supplied IdentificationName already exists.  If this is false, Duplicate Identification Names will throw an ArgumentException</param>
        /// <exception cref="ArgumentException">Throws if a duplicate IdentificationName is supplied, and AutomaticNoDuplicateSuffixes is false. Use ObjectNameIsRegistered to ensure IdentificationNames aren't duplicates.</exception>
        /// <returns>The Identitification Name of the new object (If AutomaticNoDuplicateSuffixes is false, this will always be equal to the supplied IdentificationName)</returns>
        /// <seealso cref="ObjectNameIsRegistered(string)"/>
        public string Register(object objectToRegister, string identificationName, bool automaticNoDuplicateSuffixes = false)
        {
            string FolderName = identificationName;
            if (RegisteredObjects.ContainsKey(FolderName))
            {
                if (automaticNoDuplicateSuffixes)
                {
                    FolderName = GetNonDuplicate(FolderName);
                }
                else
                {
                    throw new ArgumentException("Folder with name: " + FolderName + " already exists.  Please select a different folder name");
                }
            }
            Type oType = objectToRegister.GetType();
            //We don't load all Type Infos off the bat; instead, we load the infos as soon an object of a certain type is registered, and then we add it to a dictionary so the TypeSaveInfo object only has to be created once.
            if (!TypeSaveInfos.ContainsKey(oType))
            {
                TypeSaveInfos.Add(oType, new TypeSaveInfo(oType));
            }
            Directory.CreateDirectory(Path.Combine(SaveDataPath, FolderName));
            ObjectSaveInfo osi = new ObjectSaveInfo(objectToRegister, TypeSaveInfos[oType], FolderName);
            RegisteredObjects.Add(FolderName, osi);
            FindPaths(osi);
            return FolderName;
        }

        private void FindPaths(ObjectSaveInfo osi)
        {
            string basePath = Path.Combine(SaveDataPath, osi.FolderName);
            foreach (var FieldPathGroup in osi.TypeSaveInfo.SavedFields)
            {
                string filepath = Path.GetFullPath(Path.Combine(basePath, FieldPathGroup.Value.filename));
                PathSaveInfo pathSaveInfo = new PathSaveInfo(osi, FieldPathGroup.Key, filepath);
                ReversedPathSaveInfos.Add(pathSaveInfo, filepath);
            }
        }
        private string GetPath(ObjectSaveInfo registeredObject, FieldSaveInfo fieldSaveInfo)
        {
            PathSaveInfo psi = new PathSaveInfo(registeredObject, fieldSaveInfo.field.Name);
            if (!ReversedPathSaveInfos.ContainsKey(psi))
            {
                //If this happens, it's time to complain to the Tech Lead.
                throw new KeyNotFoundException("Internal Path Logic Error.  No Path Could be found (to SAVE to) for Registered Object:  " + registeredObject.FolderName + " Field:  " + fieldSaveInfo.field.Name + ", most likely because of an internal path handling error.");
            }
            else
            {
                psi.Filepath = ReversedPathSaveInfos[psi];
            }

            return psi.Filepath;
        }

        /// <summary>
        /// Save every registered object.  
        /// </summary>
        public void SaveAll()
        {
            Log("**Beginning SaveAll Operation (saving to " + SaveDataPath + ")**");
            Stopwatch s = Stopwatch.StartNew();
            foreach (var RegisteredObject in RegisteredObjects.Values)
            {
                Save(RegisteredObject);
            }
            Log("**SaveAll Operation Completed in " + s.ElapsedMilliseconds.ToString() + "ms!");
        }

        /// <summary>
        /// Find the Registered Object with the supplied Identification Name and Save it.  If the Identification/Folder name supplied is not registered, the program will either throw an error or log one, dependant on the throwIfObjectNotRegistered parameter"/>
        /// </summary>
        /// <param name="folderName">The Identification/Folder name of the object to save</param>
        /// <param name="throwIfObjectNotRegistered">Whether to throw an error or log one, if the Identificaiton/Folder name supplied is not registered</param>
        public void Save(string identificationName, bool throwIfObjectNotRegistered = true)
        {
            if (!RegisteredObjects.ContainsKey(identificationName))
            {
                ThrowNotRegisteredException(identificationName, "Object", throwIfObjectNotRegistered);
            }
            else
            {
                Save(RegisteredObjects[identificationName]);
            }
        }

        private void Save(ObjectSaveInfo registeredObject)
        {
            foreach (var field in registeredObject.TypeSaveInfo.SavedFields.Values)
            {
                Save(registeredObject, field);
            }
        }

        /// <summary>
        /// Save the specified object (specified by FodlerName) and underlying Field (specified by FieldName).  If either are not registered (or do not exist) the program will either throw an error or log on, dependant on the throwifObjectNotRegistered and throwIfFieldNotRegistered fields.
        /// </summary>
        /// <param name="identificatioName">The Identification/Folder name of the object to save from.</param>
        /// <param name="fieldName">The name of the field (within the specified object) to save.</param>
        /// <param name="throwIfObjectNotRegistered">Whether to throw or log an error if the Object specified is not registered.</param>
        /// <param name="throwIfFieldNotRegistered">hether to throw or log an error if the Field specified is not registered.</param>
        public void Save(string identificatioName, string fieldName, bool throwIfObjectNotRegistered = true, bool throwIfFieldNotRegistered = true)
        {
            if (!RegisteredObjects.ContainsKey(identificatioName))
            {
                ThrowNotRegisteredException(identificatioName, "Object", throwIfObjectNotRegistered);
            }
            else
            {
                Save(RegisteredObjects[identificatioName], fieldName, throwIfFieldNotRegistered);
            }
        }

        private void Save(ObjectSaveInfo registeredObject, string fieldName, bool throwIfFieldNotRegistered = true)
        {
            if (registeredObject.TypeSaveInfo.SavedFields.ContainsKey(fieldName))
            {
                FieldSaveInfo fieldSaveInfo = registeredObject.TypeSaveInfo.SavedFields[fieldName];
                Save(registeredObject, fieldSaveInfo);
            }
            else
            {
                ThrowNotRegisteredException(fieldName, "Field", throwIfFieldNotRegistered);
            }
        }

        private void Save(ObjectSaveInfo registeredObject, FieldSaveInfo fieldSaveInfo)
        {
            Log("Saving Field with Name:  \"" + fieldSaveInfo.field.Name + "\" in Object with IdentificationName:  \"" + registeredObject.FolderName + "\"...");
            string filePath = GetPath(registeredObject, fieldSaveInfo);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream saveFile = File.Open(filePath, FileMode.OpenOrCreate);
            saveFile.SetLength(0);
            bf.Serialize(saveFile, fieldSaveInfo.field.GetValue(registeredObject.Object));
            saveFile.Close();
        }


        /// <summary>
        /// Load every Registered Object.  If a registered object's save files don't exist (because it has never been saved) it will be skipped.
        /// </summary>
        public void LoadAll()
        {
            Log("**Beginning LoadAll Operation (saving from " + SaveDataPath + ")**");
            Stopwatch s = Stopwatch.StartNew();
            foreach (var loadDir in Directory.GetDirectories(SaveDataPath))
            {
                string foldername = Path.GetFileName(loadDir);
                if (RegisteredObjects.ContainsKey(foldername))
                {
                    Load(RegisteredObjects[foldername], false);
                }
            }
            Log("**LoadAll Operation Completed in " + s.ElapsedMilliseconds.ToString() + "ms!");
        }

        /// <summary>
        /// Load every field in the specified object.  If the object isn't registered or has not been saved before, the throwIfNotRegistered and throwIfNotExists parameters (respectively) define wether to throw or log an error
        /// </summary>
        /// <param name="identificationName">The Identification/Folder name of the object to load</param>
        /// <param name="throwIfNotRegistered">Whether to throw or log an error if the specified objectName isn't registered</param>
        /// <param name="throwIfNotExists">Whether to throw or log an error if the specified object hasn't been saved before.</param>
        public void Load(string identificationName, bool throwIfNotRegistered = true, bool throwIfNotExists = true)
        {
            if (RegisteredObjects.ContainsKey(identificationName))
            {
                Load(RegisteredObjects[identificationName], throwIfNotExists);
            }
            else
            {
                ThrowNotRegisteredException(identificationName, "Object", throwIfNotRegistered);
            }
        }

        private void Load(ObjectSaveInfo registeredObject, bool throwIfNotExists = true)
        {
            foreach (var field in registeredObject.TypeSaveInfo.SavedFields)
            {
                Load(registeredObject, field.Value, throwIfNotExists);
            }
        }

        /// <summary>
        /// Load the specified object (specified by FodlerName) and underlying Field (specified by FieldName).  If either are not registered (or do not exist) the program will either throw an error or log on, dependant on the throwifObjectNotRegistered and throwIfFieldNotRegistered fields.
        /// </summary>
        /// <param name="identificationName">The Identification/Folder name of the object to save from.</param>
        /// <param name="fieldName">The name of the field (within the specified object) to save.</param>
        /// <param name="throwIfObjectNotRegistered">Whether to throw or log an error if the Object specified is not registered.</param>
        /// <param name="throwIfFieldNotRegistered">Whether to throw or log an error if the Field specified is not registered under.</param>
        /// <param name="throwIfNotExists">Whether to throw or log an error if there is no saved data for the specified object and field.</param>
        public void Load(string identificationName, string fieldName,bool throwIfObjectNotRegistered = true, bool throwIfFieldNotRegistered = true, bool throwIfNotExists = true)
        {
            if (RegisteredObjects.ContainsKey(identificationName))
            {
                Load(RegisteredObjects[identificationName], fieldName, throwIfFieldNotRegistered, throwIfNotExists);
            }
            else
            {
                ThrowNotRegisteredException(identificationName, "Object", throwIfObjectNotRegistered);
            }
        }

        private void Load(ObjectSaveInfo registeredObject, string fieldName, bool throwIfFieldNotRegistered, bool throwIfNotExists = true)
        {
            if (registeredObject.TypeSaveInfo.SavedFields.ContainsKey(fieldName))
            {
                FieldSaveInfo fieldSaveInfo = registeredObject.TypeSaveInfo.SavedFields[fieldName];
                Load(registeredObject, fieldSaveInfo, throwIfNotExists);
            }
            else
            {
                ThrowNotRegisteredException(fieldName, "Field", throwIfFieldNotRegistered);
            }
        }

        private void Load(ObjectSaveInfo registeredObject, FieldSaveInfo fieldSaveInfo, bool throwIfNotExists)
        {
            Log("Loading Field with Name:  \"" + fieldSaveInfo.field.Name + "\" in Object with IdentificationName:  \"" + registeredObject.FolderName + "\"...");
            string filePath = GetPath(registeredObject, fieldSaveInfo);
            if (File.Exists(filePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream loadFile = File.Open(filePath, FileMode.Open);
                fieldSaveInfo.field.SetValue(registeredObject.Object, bf.Deserialize(loadFile));
                loadFile.Close();
            }
            else
            {
                string errorString = "Could not load file " + filePath + " into Field " + fieldSaveInfo.field.Name + " because the desired file was not found";
                if (throwIfNotExists)
                {
                    throw new FileNotFoundException(errorString);
                }
                else
                {
                    Log(errorString);
                }
            }
        }

        private void ThrowNotRegisteredException(string identificationName, string unregisteredItemTypeName, bool throwIfNotRegistered)
        {
            string errorString = "ERROR:  " + unregisteredItemTypeName + ":  " + identificationName + " cannot be loaded, because it has no relevant Registered " + unregisteredItemTypeName + ".  (In other words, you need to register the " + unregisteredItemTypeName + " you're trying to load/save before saying, 'LOAD IT!!' [Or 'SAVE IT!!'])";
            if (throwIfNotRegistered)
                throw new KeyNotFoundException(errorString);
            else
                Log(errorString);
        }
    }
    
    /// <summary>
    /// This is like a template for a function that takes in a message and logs it, wherever it wants, without returning any information.
    /// </summary>
    /// <param name="message">The information to log.  Usually this is just a string, but it is given in object form to allow for more data transfer.</param>
    public delegate void LogFunc(object message);
}