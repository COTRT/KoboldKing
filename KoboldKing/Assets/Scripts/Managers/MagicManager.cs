using System;
using Assets.Scripts.Data;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Managers
{
    [Manager(Priority = 0)]
    public class MagicManager : ManagerBase
    {
        public bool hothStarted = false;
        public override void Startup(DataService dataService)
        {
            hothStarted = true;
            StartCoroutine(LongRunner());
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }
        IEnumerator LongRunner()
        {
            Debug.Log(Fibonacci(40));
            yield return null;
            this.Status = ManagerStatus.STARTED;
            yield return null;
        }
        long Fibonacci(int x)
        {
            if(x == 0 ||x == 1)
            {
                return 1;
            }
            return Fibonacci(x - 1) + Fibonacci(x - 2);
        }
    }
}