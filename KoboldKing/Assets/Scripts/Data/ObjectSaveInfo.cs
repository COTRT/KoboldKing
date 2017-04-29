namespace Assets.Scripts.Data
{
    public class ObjectSaveInfo
    {
        internal ObjectSaveInfo()
        {

        }
        internal ObjectSaveInfo(object Object, TypeSaveInfo TypeSaveInfo,string FolderName)
        {
            this.Object = Object;
            this.TypeSaveInfo = TypeSaveInfo;
            this.FolderName = FolderName;
        }
        public object Object { get; set; }
        internal TypeSaveInfo TypeSaveInfo { get; set; }
        public string FolderName { get; set; }
    }
}
