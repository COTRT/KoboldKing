using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Item
{
    [RequireComponent(typeof(Damageable))]
    [System.Serializable]
    public class Mob : MonoBehaviour
    {
        [SerializeField] private string _mobName;
        [SerializeField] private int _level;
        [SerializeField] private int _attackRating;
        [SerializeField] private int _minDamage;
        [SerializeField] private int _maxDamage;
        [SerializeField] private int _defenseRating;
        [SerializeField] private int _luckRating;
        [SerializeField] private int _meleeDamageBonus;
        [SerializeField] private int _rangedDamageBonus;
        [SerializeField] private int _magicDamageBonus;
        [SerializeField] private int _expValue;

        [SerializeField] private string _mobTag = "Enemy";

        //[SerializeField] private GameObject _prefab;
        
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

        public int AttackRating
        {
            get { return _attackRating; }
            set { _attackRating = value; }
        }
        public int MinDamage
        {
            get { return _minDamage; }
            set { _minDamage = value; }
        }
        public int MaxDamage
        {
            get { return _maxDamage; }
            set { _maxDamage = value; }
        }

        public int DefenseRating
        {
            get { return _defenseRating; }
            set { _defenseRating = value; }
        }

        public int LuckRating
        {
            get { return _luckRating; }
            set { _luckRating = value; }
        }

        public int MeleeDamageBonus
        {
            get { return _meleeDamageBonus; }
            set { _meleeDamageBonus = value; }
        }

        public int RangedDamageBonus
        {
            get { return _rangedDamageBonus; }
            set { _rangedDamageBonus = value; }
        }
        
        public int MagicDamageBonus
        {
            get { return _magicDamageBonus; }
            set { _magicDamageBonus = value; }
        }

        public int ExpValue
        {
            get { return _expValue; }
            set { _expValue = value; }
        }


    }
}
