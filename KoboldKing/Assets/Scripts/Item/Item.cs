using UnityEngine;
using System.Collections.Generic;
namespace Assets.Scripts.Item
{
    [System.Serializable]
    public class Item : MonoBehaviour
    {
        [SerializeField] private string _itemName;
        [SerializeField] private string _description;

        [SerializeField] Icon _icon;
        [SerializeField] private List<Buff> _buffs;

        [SerializeField] ItemTypes _itemTypes;




        //item type
        //-weapon
        //-armor
        //-wearables
        //-potions/scrolls - consumeables

        //effects / buffs
        //tooltips/tooltip replacements - compares its stats to what you are wearing.

        public string Name
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        public string Description
        {
            get { return _description;}
            set { _description = value; }
        }

        public ICon Thumbnail
        {
            get { return _icon; }
            set { _icon = value; }
        }

        public ItemTypes Type
        {
            get { return _itemTypes; }
        }

    }
}
