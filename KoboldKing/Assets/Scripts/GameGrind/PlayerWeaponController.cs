using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public GameObject playerHand;
    public GameObject EquippedWeapon { get; set; }
    private IWeapon equippedWeapon;

    private CharacterStats characterStats;

    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
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
        equippedWeapon.PerformAttack();
    }

    public void PerformWeaponSpecialAttack()
    {
        equippedWeapon.PerformAttack();
    }

    public void EquipWeapon(Item itemToEquip)
    {
        if (EquippedWeapon != null)
        {
            characterStats.RemoveStatBonus(EquippedWeapon.GetComponent<IWeapon>().Stats);
            Destroy(playerHand.transform.GetChild(0).gameObject);
        }


        EquippedWeapon = (GameObject)Instantiate(Resources.Load<GameObject>("GameGrind/Weapons/" + itemToEquip.ObjectSlug),
            playerHand.transform.position, playerHand.transform.rotation);
        equippedWeapon = EquippedWeapon.GetComponent<IWeapon>();
        EquippedWeapon.GetComponent<IWeapon>().Stats = itemToEquip.Stats;
        EquippedWeapon.transform.SetParent(playerHand.transform);
        characterStats.AddStatBonus(itemToEquip.Stats);
        Debug.Log(equippedWeapon.Stats[0].GetCalculatedStatValue());
    }

}
