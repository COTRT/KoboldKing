using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public List<BaseStat> Stats { get; set; }
    public string ObjectSlug { get; set; } // What the heck -- named object slug? (will be renamed in the future)


    public Item(List<BaseStat> _Stats, string _ObjectSlug)
    {
        this.Stats = _Stats;
        this.ObjectSlug = _ObjectSlug;
    }

}
