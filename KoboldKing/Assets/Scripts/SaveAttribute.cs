[System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
internal sealed class SaveToAttribute : System.Attribute
{
    readonly string saveFileName;

    public SaveToAttribute (string saveFileName)
    {
        this.saveFileName = saveFileName;
    }
    
    public string SaveFileName
    {
        get { return saveFileName; }
    }

    public override string ToString(){
        return saveFileName;
    }
}