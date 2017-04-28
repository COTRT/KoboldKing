using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Data
{
    internal class PathSaveInfo
    {
        public PathSaveInfo()
        {


        }
        public PathSaveInfo(ObjectSaveInfo ObjectSaveInfo,string FieldName, string filepath = null)
        {
            this.ObjectSaveInfo = ObjectSaveInfo;
            this.FieldName = FieldName;
            this.Filepath = filepath;
        }
        public ObjectSaveInfo ObjectSaveInfo { get; set; }
        public string FieldName { get; set; }
        public string Filepath { get; set; }
    }
    internal class FieldAndObjectPathSaveInfoComparer : IEqualityComparer<PathSaveInfo>
    {
        public bool Equals(PathSaveInfo x, PathSaveInfo y)
        {
            return x.ObjectSaveInfo.FolderName == x.ObjectSaveInfo.FolderName && x.FieldName == y.FieldName;
        }

        public int GetHashCode(PathSaveInfo obj)
        {
            return obj.GetHashCode();
        }
    }
}
