using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Devices
{
    public class DeviceTrigger : MonoBehaviour
    {

        //list of target objects that this trigger will activate
        [SerializeField]
        private GameObject[] targets;

        public bool requireKey;

        //OnTriggerEnter() is called when another object enters the trigger volume
        void OnTriggerEnter(Collider other)
        {
            //if (requireKey && Managers.Inventory.equippedItem != "key")
            //{
            //    return;
            //}

            //foreach (GameObject target in targets)
            //{
            //    target.SendMessage("Activate");
            //}
        }

        //whereas onTriggerExit() is called when an object leaves the trigger volume
        void OnTriggerExit(Collider other)
        {
            foreach (GameObject target in targets)
            {
                target.SendMessage("Deactivate");
            }
        }

    }

}
