using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance { get; set; }
    private List<ItemX> Items { get; set; }
    public List<BaseStat> nothing { get; set; }




    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        BuildDatabase();
    }
    private void BuildDatabase()
    {
        Items = JsonConvert.DeserializeObject<List<ItemX>>(Resources.Load<TextAsset>("GameGrind/JSON/Items").ToString());
    }

    public ItemX GetItem(string itemSlug)
    {
        // TODO: Lamba expression with where clause could be faster?
        foreach (ItemX item in Items)
        {
            if (item.ObjectSlug == itemSlug)
                return item;
        }

        Debug.LogWarning("Couldn't find item: " + itemSlug);
        return null;

    }

}
