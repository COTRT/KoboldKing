using Assets.Scripts.Item;
using UnityEngine;

// do my players inherit from mobs or do we need a base class.

namespace Assets.Scripts.TimWork.Mobs
{
    public class MobAttribute
    {
        [SerializeField] private string _attributeName;
        [SerializeField] private int _currentValue;
        [SerializeField] private int _baseValue;

        //[SerializeField] private Icon _icon;

        //[SerializeField] private AttributeTypes _attribute;


        // do we want them in an enum for faster ref
        public string Name
        {
            get { return _attributeName;}
            set { _attributeName = value; }
        }
        public int BaseValue
        {
            get { return _baseValue; }
            set { _baseValue = value; }
        }
        public void AddToBaseValue(int amount)
        {
            _baseValue += amount;
        }
        
        public int CurrentValue
        {
            get { return _currentValue; }
            set { _currentValue = value; }
        }
        public void AddToCurrentValue(int amount)
        {
            _currentValue += amount;
        }

    }
}
