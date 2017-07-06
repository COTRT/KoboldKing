using Assets.Scripts.Magic.Spells;

namespace Assets.Scripts.Managers
{
    [Manager(Priority = 0)]
    public class SpellManager : NamedPrefabManager<Spell>
    {
        //    public Dictionary<string,Spell> Spells;

        //    public Spell Create(string spell)
        //    {
        //        return Instantiate(Get(spell));
        //    }
        //    public Spell Get(string spell)
        //    {
        //        return Spells[spell];
        //    }
        //    /// <summary>
        //    /// Warning:  This --> CREATES &lt-- (aka instantiates) the specified effect.  Use Get() to get the (unstantiated) prefab
        //    /// </summary>
        //    public Spell this[string spell]
        //    {
        //        get
        //        {
        //            return Create(spell);
        //        }
        //    }
        //    public override void Startup(DataService dataService)
        //    {
        //        dataService.Register(this, "Magic");
        //        LoadSpells();
        //        Startup_Complete();
        //    }
        //    private void LoadSpells()
        //    {
        //        Spells = Resources.LoadAll<GameObject>("Spells")
        //            .ToDictionary(
        //                s => s.name,
        //                s => s.GetComponent<Spell>());

        //    }
        //    [On(StartupEvent.MANAGERS_STARTED)]
        //    public void OnAllStarted()
        //    {
        //        Debug.Log("MagicManager has recieved a StartupEvent.MANAGERS_STARTED event!");
        //    }

        //    public bool Exists(string spell)
        //    {
        //        return Spells.ContainsKey(spell);
        //    }
        //}
    }
}