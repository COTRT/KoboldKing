using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Assets.Scripts.Data
{
    internal class TypeSaveInfo
    {
        public TypeSaveInfo(Type t)
        {
            SavedFields = new Dictionary<string, FieldSaveInfo>();
            foreach (var Field in t.GetFields().Where(f => Attribute.IsDefined(f, typeof(SaveToAttribute))))
            {
                SavedFields.Add(Field.Name, new FieldSaveInfo(Field, Field.GetCustomAttributes(typeof(SaveToAttribute), false).Single().ToString(),this));
            }
            this.Type = t;
        }
        /// <summary>
        /// Field name v. Path (local, of course)
        /// </summary>
        public Dictionary<string,FieldSaveInfo> SavedFields;
        public Type Type;
    }

    internal class FieldSaveInfo
    {
        public FieldInfo field;
        public string filename;
        public TypeSaveInfo TypeSaveInfo { get; set; }

        public FieldSaveInfo(FieldInfo field, string filename, TypeSaveInfo TypeSaveInfo)
        {
            this.field = field;
            this.filename = filename;
            this.TypeSaveInfo = TypeSaveInfo;
        }
    }
}
