using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryClassifier : MonoBehaviour {

    public string Name;         //Name of object
    public string Description;  //Description of object
    public string ItemType;     //This will help us classify if it is a weapon, coin, armor, etc.
    public bool IsWeapon;       //This tells if it is a weapon or not, this lets us be able to assign other varibles acording to if it is a weapon.
    public int Damage;          //If it is a weapon it will have like some sort of damage number.
    public Sprite Inventoryimg; //This will be the image that people see in the inventory for this object.
    public GameObject Prefab;   //This is the prefab of the object
    public bool CanGet;         //This simply says can we pick up this object




	void Start () {
		
	}
	

	void Update () {
		
	}
}
