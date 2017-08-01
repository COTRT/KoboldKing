using System.Collections.Generic;
using Assets.Scripts.Item;
using UnityEngine;

namespace Assets.Scripts.TimWork
{
    public class Encounter : MonoBehaviour
    {
        [SerializeField] private int _encoutnerNumber;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private int _mobCount;
        [SerializeField] private MobTypes _mobTypes;
        [SerializeField] private MobRanks _mobRanks;
        [SerializeField] private List<int> _xPos;
        [SerializeField] private List<int> _yPos;
        [SerializeField] private List<int> _zPos;
        [SerializeField] private List<int> _xRotation;
        [SerializeField] private List<int> _yRotation;
        [SerializeField] private List<int> _zRotation;

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                switch (_encoutnerNumber)
                {
                    case 1:

                        break;
                    default:
                        break;
                }

            }
        }

        //private void ()




    }
}
