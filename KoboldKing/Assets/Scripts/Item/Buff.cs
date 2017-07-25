using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class Buff : MonoBehaviour
    {
        [SerializeField] private string _buffName;
        [SerializeField] private Icon _icon;
        [SerializeField] private int _duration;
        [SerializeField] private int _currentDuration;
        [SerializeField] private bool _permanent = false;
    }
}
