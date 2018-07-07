using System;
using Assets.Scripts.Data;
using Assets.Scripts.Events;
using Assets.Scripts.Managers;
using UnityEngine;
using System.Linq;

[Manager]
public class GiveItem : ManagerBase
{
    protected override void StartManager(DataService dataService)
    {
        Messenger<string[],GameObject>.AddListener(DialogueAction.GiveItem, HandleGiveItemAction);
    }
    private void HandleGiveItemAction(string[] arguments,GameObject speaker)
    {
        foreach(string argument in arguments)
        {
            InventoryController.Instance.GiveItem(argument);
        }
    }
}

