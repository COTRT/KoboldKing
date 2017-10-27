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
    public string ItemType;
    /// <summary>
    /// This tells if it is a weapon or not, this lets us be able to assign other varibles acording to if it is a weapon.
    /// </summary>
    public bool IsWeapon;
    /// <summary>
    /// If it is a weapon it will have like some sort of damage number.
    /// </summary>
    public int Damage;
    /// <summary>
    /// This will be the image that people see in the inventory for this object.
    /// </summary>
    public Sprite Inventoryimg;
    /// <summary>
    /// This is the prefab of the object
    /// </summary>
    public GameObject Prefab;
    /// <summary>
    /// This simply says can we pick up this object
    /// </summary>
    public bool CanGet;




    void Start () {
		
	}
	

	void Update () {
		
	}
}
