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

        [SerializeField] private Icon _icon;
        [SerializeField] private List<Buff> _buffs;


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
            set
            {
                if (value < 1)
                {
                    _maxHealth = 1;
                }
                else
                {
                    _maxHealth = value;
                }
            }
        }

        public int Level
        {
            get { return _level; }
            set
            {
                if (value < 0)
                {
                    _level = 0;
                }
                else
                {
                    _level = value;
                }

            }
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

        public List<Buff> Buffs
        {
            get { return _buffs; }
            set { _buffs = value; }
        }

        public void AddBuff(Buff buff)
        {
            _buffs.Add(buff);
        }
    }
}
