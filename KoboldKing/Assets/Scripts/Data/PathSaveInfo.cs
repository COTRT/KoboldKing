using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Data
{
    public class PathSaveInfo
    {
        public PathSaveInfo()
        {


        }
        public PathSaveInfo(ObjectSaveInfo ObjectSaveInfo,string FieldName)
        {
            this.ObjectSaveInfo = ObjectSaveInfo;
            this.FieldName = FieldName;
        }
        public ObjectSaveInfo ObjectSaveInfo { get; set; }
        public string FieldName { get; set; }
    }
}
