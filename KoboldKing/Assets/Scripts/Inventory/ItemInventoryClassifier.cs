using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryClassifier : MonoBehaviour {

    /// <summary>
    /// Name of object
    /// </summary>
    public string Name;
    /// <summary>
    /// Description of object
    /// </summary>
    public string Description;
    /// <summary>
    /// This will help us classify if it is a weapon, coin, armor, etc.
    /// </summary>
    public ItemType ItemType;
    /// <summary>
    /// This will be the image that people see in the inventory for this object.
    /// </summary>
    public Sprite Inventoryimg;
    /// <summary>
    /// This is the prefab of the object
    /// </summary>
    public GameObject Prefab;


}

[Flags]
public enum ItemType
{
    Weapon = 2^0,
    Armor = 2^1,
    HealthRegenerator = 2^2,
    CoinsMoney = 2^3,

}