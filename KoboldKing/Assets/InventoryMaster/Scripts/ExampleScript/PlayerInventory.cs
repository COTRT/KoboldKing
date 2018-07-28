using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System;

public class PlayerInventory : MonoBehaviour
{
    public Inventory craftSystemInv;
    [HideInInspector]
    public CraftSystem craftSystem;
    public Inventory mainInventory;
    public  Inventory characterSystem;
    private Tooltip toolTip;

    private InputManager inputManagerDatabase;

    public GameObject HPMANACanvas;

    Text hpText;
    Text manaText;
    Image hpImage;
    Image manaImage;

    public float maxHealth = 100;
    public float maxMana = 100;
    public float maxDamage = 0;
    public float maxArmor = 0;

    public float currentHealth = 60;
    public float currentMana = 100;
    public float currentDamage = 0;
    public float currentArmor = 0;

    int normalSize = 16;

    public static PlayerInventory Instance;

    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Start()
    {
        if (HPMANACanvas != null)
        {
            hpText = HPMANACanvas.transform.GetChild(1).GetChild(0).GetComponent<Text>();

            manaText = HPMANACanvas.transform.GetChild(2).GetChild(0).GetComponent<Text>();

            hpImage = HPMANACanvas.transform.GetChild(1).GetComponent<Image>();
            manaImage = HPMANACanvas.transform.GetChild(1).GetComponent<Image>();

            UpdateHPBar();
            UpdateManaBar();
        }
        else
        {
            throw new Exception("Please provide PlayerInventory with HPMANACanvas");
        }

        inputManagerDatabase = (InputManager)Resources.Load("InputManager");

        if (new[] { craftSystemInv, mainInventory, characterSystem }.Any(i => i == null))
        {
            throw new ArgumentException("Please supply the PlayerInventory Script with the Craft System, Character System, and Main Inventory panels in the inspector");
        }
        craftSystem = craftSystemInv.GetComponent<CraftSystem>();

        if (GameObject.FindGameObjectWithTag("Tooltip") != null)
        {
            toolTip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
        }
        else
        {
            throw new Exception("No GameObject found with 'Tooltip' tag.  That's needed.");
        }

        normalSize = mainInventory.width * mainInventory.height;
    }

    public void OnEnable()
    {
        Inventory.ItemEquip += OnBackpack;
        Inventory.ItemUnequip += UnEquipBackpack;

        Inventory.ItemEquip += OnGearItem;
        Inventory.ItemConsume += OnConsumeItem;
        Inventory.ItemUnequip += OnUnEquipItem;

        Inventory.ItemEquip += EquipWeapon;
        Inventory.ItemUnequip += UnEquipWeapon;
    }

    public void OnDisable()
    {
        Inventory.ItemEquip -= OnBackpack;
        Inventory.ItemUnequip -= UnEquipBackpack;

        Inventory.ItemEquip -= OnGearItem;
        Inventory.ItemConsume -= OnConsumeItem;
        Inventory.ItemUnequip -= OnUnEquipItem;

        Inventory.ItemUnequip -= UnEquipWeapon;
        Inventory.ItemEquip -= EquipWeapon;
    }

    void EquipWeapon(Item item)
    {
        if (item.Type == ItemType.Weapon)
        {
            //add the weapon if you unequip the weapon
        }
    }

    void UnEquipWeapon(Item item)
    {
        if (item.Type == ItemType.Weapon)
        {
            //delete the weapon if you unequip the weapon
        }
    }

    void OnBackpack(Item item)
    {
        if (item.Type == ItemType.Backpack)
        {
            mainInventory.SortItems();
            var slots = item.itemAttributes.Find(att => att.attributeName == "Slots");
            mainInventory.ChangeInventorySize(normalSize + slots.attributeValue);
        }
    }

    void UnEquipBackpack(Item item)
    {
        if (item.Type == ItemType.Backpack)
            mainInventory.ChangeInventorySize(normalSize);
    }



   

    void UpdateHPBar()
    {
        hpText.text = (currentHealth + "/" + maxHealth);
        hpImage.fillAmount = currentHealth / maxHealth;
    }

    void UpdateManaBar()
    {
        manaText.text = (currentMana + "/" + maxMana);
        manaImage.fillAmount = currentMana / maxMana;
    }


    public void OnConsumeItem(Item item)
    {
        var HealthAtt = item.itemAttributes.Find(att => att.attributeName == "Health");
        if (HealthAtt != null)
        {
            currentHealth = Mathf.Max(currentHealth + HealthAtt.attributeValue, maxHealth);
        }
        var ManaAtt = item.itemAttributes.Find(att => att.attributeName == "Mana");
        if (ManaAtt != null)
        {
            currentMana = Mathf.Max(currentMana + ManaAtt.attributeValue, maxMana);
        }
        var ArmorAtt = item.itemAttributes.Find(att => att.attributeName == "Armor");
        if (ArmorAtt != null)
        {
            currentArmor = Mathf.Max(currentArmor + ArmorAtt.attributeValue, maxArmor);
        }
        var DamageAtt = item.itemAttributes.Find(att => att.attributeName == "Damage");
        if (DamageAtt != null)
        {
            currentDamage = Mathf.Max(currentDamage + DamageAtt.attributeValue, maxDamage);
        }

        //if (HPMANACanvas != null)
        //{
        //    UpdateManaBar();
        //    UpdateHPBar();
        //}
    }

    public void OnGearItem(Item item)
    {
        AdjustMaximums(item, 1);
        //if (HPMANACanvas != null)
        //{
        //    UpdateManaBar();
        //    UpdateHPBar();
        //}
    }
    private void AdjustMaximums(Item item, float multiplier)
    {
        foreach (var itemAttribute in item.itemAttributes)
        {
            if (itemAttribute.attributeName == "Health")
                maxHealth += itemAttribute.attributeValue * multiplier;
            if (itemAttribute.attributeName == "Mana")
                maxMana += itemAttribute.attributeValue * multiplier;
            if (itemAttribute.attributeName == "Armor")
                maxArmor += itemAttribute.attributeValue * multiplier;
            if (itemAttribute.attributeName == "Damage")
                maxDamage += itemAttribute.attributeValue * multiplier;
        }
    }

    public void OnUnEquipItem(Item item)
    {
        AdjustMaximums(item, -1);
        //if (HPMANACanvas != null)
        //{
        //    UpdateManaBar();
        //    UpdateHPBar();
        //}
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(inputManagerDatabase.CharacterSystemKeyCode))
        {
            if (!characterSystem.gameObject.activeSelf)
            {
                if (!mainInventory.gameObject.activeSelf) mainInventory.OpenInventory();
                characterSystem.OpenInventory();
            }
            else
            {
                if (toolTip != null)
                    toolTip.deactivateTooltip();
                characterSystem.CloseInventory();
            }
        }

        if (Input.GetKeyDown(inputManagerDatabase.InventoryKeyCode))
        {
            if (!mainInventory.gameObject.activeSelf)
            {
                mainInventory.OpenInventory();
            }
            else
            {
                if (toolTip != null)
                    toolTip.deactivateTooltip();
                mainInventory.CloseInventory();
            }
        }

        if (Input.GetKeyDown(inputManagerDatabase.CraftSystemKeyCode))
        {
            if (!craftSystemInv.gameObject.activeSelf)
            {
                if (!mainInventory.gameObject.activeSelf) mainInventory.OpenInventory();
                craftSystemInv.OpenInventory();
            }
            else
            {
                if (craftSystem != null)
                    craftSystem.BackToInventory();
                if (toolTip != null)
                    toolTip.deactivateTooltip();
                craftSystemInv.CloseInventory();
            }
        }

    }

}
