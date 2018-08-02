using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Linq;
using System;

public class Hotbar : MonoBehaviour
{

    [SerializeField]
    public KeyCode[] keyCodesForSlots = new KeyCode[10];
    Inventory inv;

    public void Start()
    {
        inv = GetComponent<Inventory>();
        inv.SizeChanged += UpdateKeyCodeCount;
        UpdateKeyCodeCount();
    }

    private void UpdateKeyCodeCount()
    {
        Array.Resize(ref keyCodesForSlots, inv.Size);
    }

    void Update()
    {
        for(int i = 0; i <inv.Size;i++) { 
            if (Input.GetKeyDown(keyCodesForSlots[i]))
            {
                Item item = inv.ItemsInInventory[i];
                if (item != null && item.Type != ItemType.UFPS_Ammo)
                {
                    inv.ConsumeItem(item);
                }
            }
        }
    }
}
