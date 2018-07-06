using System;
using Assets.Scripts.Data;
using Assets.Scripts.Events;
using Assets.Scripts.Managers;
using UnityEngine;
using System.Linq;

[Manager]
public class GiveItem : ManagerBase
{
    public override void Startup(DataService dataService)
    {
        Messenger<string[]>.AddListener(DialogueAction.GiveItem, HandleGiveItemAction);
    }
    private void HandleGiveItemAction(string[] arguments)
    {
        foreach(string argument in arguments)
        {
            InventoryController.Instance.GiveItem(argument);
        }
    }
}

