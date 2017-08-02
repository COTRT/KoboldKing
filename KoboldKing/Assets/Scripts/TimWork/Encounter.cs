using System.Collections.Generic;
using Assets.Scripts.Item;
using UnityEngine;

namespace Assets.Scripts.TimWork
{
    public class Encounter : MonoBehaviour
    {
        [SerializeField] private int _encounterNumber;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private int _mobCount;
        [SerializeField] private MobTypes _mobTypes;
        [SerializeField] private MobRanks _mobRanks;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private List<int> _xPos;
        [SerializeField] private List<int> _yPos;
        [SerializeField] private List<int> _zPos;
        [SerializeField] private List<int> _xRotation;
        [SerializeField] private List<int> _yRotation;
        [SerializeField] private List<int> _zRotation;

        [SerializeField] GameObject gameObject;

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                SpawnEncounter();
            }
        }

        private void SpawnEncounter()
        {
            for (var i = 0; i < _mobCount; i++)
            {
                var position = new Vector3(_xPos[i], _yPos[i], _zPos[i]);
                var rotation = new Quaternion(_xRotation[i], _yRotation[i], _zRotation[i], 0);
                Instantiate(_prefab, position, rotation);
            }

            Destroy(this.gameObject);
        }
    }
}
