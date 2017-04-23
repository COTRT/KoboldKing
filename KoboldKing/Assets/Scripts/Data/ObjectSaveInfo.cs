using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Data
{
    public class ObjectSaveInfo
    {
        public ObjectSaveInfo()
        {

        }
        public ObjectSaveInfo(object Object, TypeSaveInfo TypeSaveInfo,string FolderName)
        {
            this.Object = Object;
            this.TypeSaveInfo = TypeSaveInfo;
            this.FolderName = FolderName;
        }
        public object Object { get; set; }
        public virtual TypeSaveInfo TypeSaveInfo { get; set; }
        public string FolderName { get; set; }
    }
}
