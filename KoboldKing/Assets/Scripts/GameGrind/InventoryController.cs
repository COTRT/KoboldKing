using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public PlayerWeaponController playerWeaponController;
    public Item sword;
    public Item staff;
    public Item PotionLog;

    void Start()
    {
        playerWeaponController = GetComponent<PlayerWeaponController>();
        List<BaseStat> swordStats = new List<BaseStat>();
        swordStats.Add(new BaseStat(6, "Power", "Your power level."));
        staff = new Item(swordStats, "staff");
        sword = new Item(swordStats, "sword");

        PotionLog = new Item(new List<BaseStat>(), "potion_log", "Drink this to log something cool", "Drink", "Log Potion", false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            playerWeaponController.EquipWeapon(sword);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerWeaponController.EquipWeapon(staff);
        }
    }

}
