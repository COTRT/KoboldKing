using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interactable
{
    public ItemX ItemDrop { get; set; }

    public override void Interact()
    {
        InventoryController.Instance.GiveItem(ItemDrop);
        Destroy(gameObject);
    }


}
