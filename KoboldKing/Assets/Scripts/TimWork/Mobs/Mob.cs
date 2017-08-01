using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Item
{
    [System.Serializable]
    public class Mob : MonoBehaviour
    {
        [SerializeField] private string _mobName;
        [SerializeField] private int _currentHealth;
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _level;
        [SerializeField] private int _attackRating;
        [SerializeField] private int _defenseRating;
        [SerializeField] private int _luckRating;
        [SerializeField] private int _meleeDamageBonus;
        [SerializeField] private int _rangedDamageBonus;
        [SerializeField] private int _magicDamageBonus;
        [SerializeField] private int _expValue;

        [SerializeField] private string _mobTag = "Enemy";

        [SerializeField] private GameObject _prefab;
        
        [SerializeField] private Icon _icon;
        [SerializeField] private List<Buff> _buffs;
        [SerializeField] private MobTypes _mobType;
        [SerializeField] private MobRanks _mobRank;
        
        // add loot tables

        private void Awake()
        {
            gameObject.tag = _mobTag;
        }





        public string Name
        {
            get { return _mobName; }
            set { _mobName = value; }
        }

        public int CurrentHealth
        {
            get { return _currentHealth ; }
            set {
                    if (value < 0)
                    {
                        _currentHealth = 0;
                    }
                    else if (_currentHealth > _maxHealth)
                    {
                        _currentHealth = _maxHealth;
                    }
                    else
                    {
                        _currentHealth = value;
                    }
                }
        }

        public int MaxHealth
        {
            get { return _maxHealth;}
            set { _maxHealth = value < 1 ? 1 : value; }
        }

        public int Level
        {
            get { return _level; }
            set { _level = value < 0 ? 0 : value; }
        }

        public void LevelUp()
        {
            _level++;
        }

        public Icon ThumbNail
        {
            get { return _icon; }
            set { _icon = value; }
        }

        public MobTypes MobType
        {
            get { return _mobType; }
            set { _mobType = value; }
        }

        public MobRanks MobRank
        {
            get { return _mobRank; }
            set { _mobRank = value; }
        }

        public List<Buff> Buffs
        {
            get { return _buffs; }
            set { _buffs = value; }
        }

        public void AddBuff(Buff buff)
        {
            _buffs.Add(buff);
        }

        //TODO:
        //_attackRating;
        //_defenseRating;
        //_luckRating;
        //_meleeDamageBonus;
        //_rangedDamageBonus;
        //_magicDamageBonus;

        public int ExpValue
        {
            get { return _expValue; }
            set { _expValue = value; }
        }


    }
}
