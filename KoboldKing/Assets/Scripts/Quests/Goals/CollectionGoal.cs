using Assets.Scripts.Events;
using Assets.Scripts.Quests;
using System.Collections.Generic;

public class Collection : Goal
{
    private readonly string itemId;
    public Collection(Dictionary<string, string> arguments) : base(arguments)
    {
        arguments.CheckConfigContains("Item");
        itemId = arguments["Item"];
        Messenger<Item>.AddListener(InventoryEvent.ITEM_GIVEN, (item) =>
        {
            if (item.ObjectSlug == itemId)
            {
                CurrentAmount++;
            }
        });
    }

    public override string Description { get; set; }
    public override int CurrentAmount { get; set; }
    public override int RequiredAmount { get; set; }
}

