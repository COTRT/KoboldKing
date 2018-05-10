using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public GameObject playerHand;
    public GameObject EquippedWeapon { get; set; }


    Transform spawnProjectile;
    private Item currentlyEquippedItem;
    private IWeapon equippedWeapon;

    private CharacterStats characterStats;

    void Start()
    {
        spawnProjectile = transform.Find("ProjectileSpawn");
        characterStats = GetComponent<Player>().characterStats;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            PerformWeaponAttack();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            PerformWeaponSpecialAttack();
        }
    }

    public void PerformWeaponAttack()
    {
        equippedWeapon.PerformAttack(CalculateDamage());
    }

    public void PerformWeaponSpecialAttack()
    {
        equippedWeapon.PerformSpecialAttack();
    }

    public void EquipWeapon(Item itemToEquip)
    {
        if (EquippedWeapon != null)
        {
            UnequipWeapon();
        }


        EquippedWeapon = (GameObject)Instantiate(Resources.Load<GameObject>("GameGrind/Weapons/" + itemToEquip.ObjectSlug),
            playerHand.transform.position, playerHand.transform.rotation);
        equippedWeapon = EquippedWeapon.GetComponent<IWeapon>();

        if (EquippedWeapon.GetComponent<IProjectileWeapon>() != null)
        {
            EquippedWeapon.GetComponent<IProjectileWeapon>().ProjectileSpawn = spawnProjectile;
        }
        EquippedWeapon.GetComponent<IWeapon>().Stats = itemToEquip.Stats;
        EquippedWeapon.transform.SetParent(playerHand.transform);
        equippedWeapon.Stats = itemToEquip.Stats;
        currentlyEquippedItem = itemToEquip;
        characterStats.AddStatBonus(itemToEquip.Stats);
        UIEventHandler.ItemEquipped(itemToEquip);
        UIEventHandler.StatsChanged();
        //Debug.Log(equippedWeapon.Stats[0].GetCalculatedStatValue());
    }

    public void UnequipWeapon()
    {
        InventoryController.Instance.GiveItem(currentlyEquippedItem.ObjectSlug);
        characterStats.RemoveStatBonus(EquippedWeapon.GetComponent<IWeapon>().Stats);
        Destroy(playerHand.transform.GetChild(0).gameObject);
        UIEventHandler.StatsChanged();
    }

    private int CalculateDamage()
    {
        int damageToDeal = (characterStats.GetStat(BaseStat.BaseStatType.Power).GetCalculatedStatValue() * 2) + Random.Range(2, 8);
        damageToDeal += CalculateCrit(damageToDeal);

        Debug.Log("Damage dealt: " + damageToDeal);
        return damageToDeal;
    }

    private int CalculateCrit(int damage)
    {
        // crit chance = 10% in this example.  Obviously this could be altered via modifiers.
        if (Random.value <= .10f)
        {
            int critDamage = (int)(damage * Random.Range(.25f, .75f));
            return critDamage;

        }

        return 0;
    }

}
