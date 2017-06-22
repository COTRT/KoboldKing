using Assets.Scripts.Data;
using Assets.Scripts.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class Managers : MonoBehaviour
    {
        public Dictionary<Type, IGameManager> AllManagers;

        public IGameManager this[Type type]
        {
            get
            {
                return AllManagers[type];
            }
        }

        private void Awake()
        {
            AllManagers = new Dictionary<Type, IGameManager>();
            //AWESOME!!!!!!!!  This is, like, the best part of C# I've across to date
            //This is also kinda complicated (I copied it out of a codebase made by some $125-an-hour mega-programmers) so I'll walk you through it:
            var prioritizedGroups = from a in AppDomain.CurrentDomain.GetAssemblies() //Get Every Assembly (like System.Linq and KoboldKing) in the current Application Domain
                         from t in a.GetTypes() //Go through each of the underlying types in each Assembly (like all the classes in KoboldKing)
                         where !t.IsAbstract && typeof(IGameManager).IsAssignableFrom(t) && typeof(MonoBehaviour).IsAssignableFrom(t) && t.GetConstructor(Type.EmptyTypes) != null //Filter down to only types that a) aren't abstract, b) are IGameManager's, and c) can be constructed (as in new Manager()) without any parameters
                         let attributes = t.GetCustomAttributes(typeof(ManagerAttribute), false) //Get all ManagerAttribute's applied to the now-filtered types
                         where attributes != null && attributes.Length == 1 //Filter once more, this time narrowing down to ONLY types with AN Attribute.  (The ManagerAttribute can't have multiple per type, so there's no need to comphesate for multiple ManagerAttribute's on a single type
                         group t by ((ManagerAttribute)attributes.Single()).Priority into pGroups//Group each manager-type by its priority, as defined in its ManagerAttribute
                         orderby pGroups.Key //Order each priority group of Managers by each group's priority
                         select pGroups; //Return the now filtered, grouped, and ordered list of manager-types.

            StartCoroutine(StartupManagers(prioritizedGroups));
        }
        private IEnumerator StartupManagers(IEnumerable<IGrouping<int,Type>> prioritizedTypes)
        {
            DataService dataService = new DataService(Application.persistentDataPath);

            int totalManagers = prioritizedTypes.Sum(pGroup => pGroup.Count());
            int completedManagers = 0;
            int currentCompletedManagers = 0;
            int currentTotalManagers = 0;
            int currentPriorityLevel = 0; //This guy transfers priority level from the loop to the OnManagerStarted function

            Debug.Log("Starting up " + totalManagers + " Manager(s) over "+prioritizedTypes.Count()+" priority level(s).");

            Action<IGameManager> OnManagerStarted = (manager) =>
            {
                currentCompletedManagers++;
                completedManagers++;
                Messenger<int, int>.Broadcast(StartupEvent.MANAGERS_PROGRESS, currentCompletedManagers, currentTotalManagers,MessengerMode.DONT_REQUIRE_LISTENER);
                Debug.Log("Manager Startup Progress:  " + completedManagers.ToString() + "/" + totalManagers + ". (Currently " + currentCompletedManagers.ToString() + "/" + currentTotalManagers.ToString() + " on Priority Level " + currentPriorityLevel + ")");
            };
            Messenger<IGameManager>.AddListener(ManagerEvent.MANAGER_STARTED, OnManagerStarted);

            //The managers have been prioritized, so all we have to do is start each priority level individually, and they'll be in order.
            foreach (var pGroup in prioritizedTypes)
            {
                //We add the extra .ToList() to prevent a funky issue:
                //  When you just leave it as ".Select(...)",  The expression tree you specify gets executed each time you use/iterate over the IEnumerable.
                //  That means that, in the following code, we'd get two IGameManager's added per Type, but only one would be Started Up.
                //Also of Note in these lines of code:  We add the manager instance to AllManagers here, at the same time as getting starting instances, 
                //  because this is the only point in this program where both the starting type and the starting instance are available.
                var startingManagers = pGroup.Select(mType => {
                        var m = (IGameManager)gameObject.AddComponent(mType);
                        AllManagers.Add(mType, m);
                        return m;
                    }).ToList();
                currentTotalManagers = startingManagers.Count();
                currentCompletedManagers = 0;
                //Start dem up!
                foreach (var sm in startingManagers) sm.Startup(dataService);
                //Wait until all (current priority-level) managers are finished starting.  Since we've signed up for alerts and have a function covering them, we don't have to do anything here.
                while (currentCompletedManagers < currentTotalManagers) yield return new WaitForSeconds(.1f);
                Debug.Log("Priority Level " + pGroup.Key + " Loaded.");
            }

            Messenger<IGameManager>.RemoveListener(ManagerEvent.MANAGER_STARTED, OnManagerStarted);
            Debug.Log("Startup Complete!");
            Messenger.Broadcast(StartupEvent.MANAGERS_STARTED,MessengerMode.DONT_REQUIRE_LISTENER);
        }
    }
}
