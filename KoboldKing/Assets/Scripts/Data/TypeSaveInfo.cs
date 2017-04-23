using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Data
{
    public class TypeSaveInfo
    {
        public TypeSaveInfo(Type t)
        {
            SavedFields = new Dictionary<string, string>();
            foreach (var Field in t.GetFields().Where(f => Attribute.IsDefined(f, typeof(SaveToAttribute))))
            {
                SavedFields.Add(Field.Name, Field.GetCustomAttributes(typeof(SaveToAttribute), false).Single().ToString());
            }
            ReversedSavedFields = SavedFields.ToDictionary(sfm => sfm.Value, sfm => sfm.Key);
        }
        /// <summary>
        /// Field name v. Path (local, of course)
        /// </summary>
        public Dictionary<string,string> SavedFields;
        /// <summary>
        /// Path (local, of course) v. Field Name
        /// </summary>
        public Dictionary<string, string> ReversedSavedFields;
    }
}
