using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Data;
using Assets.Scripts.Events;
using UnityEngine;
using System.Reflection;

namespace Assets.Scripts.Managers
{
    public abstract class ManagerBase : MonoBehaviour,IGameManager
    {
        private ManagerStatus _status;
        private ManagerStatus oldStatus;
        public event UnhandledExceptionEventHandler OnException;

        public ManagerBase()
        {
            //Get all methods that implement OnAttribute, then turn that into a grouping mapping event names to methods
            //StackOverflow: http://stackoverflow.com/questions/43860262/mapping-a-single-method-to-multiple-strings-dictionary-to-a-single-string-to-mul
            var methods = this.GetType().GetMethods()
                 .Where(m => Attribute.IsDefined(m, typeof(OnAttribute))&&m.GetParameters().Count()==0&&m.ReturnType==typeof(void)) //Filter to methods with OnAttributes.  Also, ban parameters and return types. (yes, know that Messenger supports return types, but they can be of any type under the sun, so if you really want to sign up for one, just sign up yourself)
                 .SelectMany(m => m.GetCustomAttributes(typeof(OnAttribute), true)
                         .Select(a => new KeyValuePair<string,MethodInfo>(((OnAttribute)a).EventName,m)))
                 .GroupBy(g => g.Key, g => g.Value);

            foreach (var eventname in methods)
            {
                foreach (var method in eventname)
                {
                    Messenger.AddListener(eventname.Key, (Action)Delegate.CreateDelegate(typeof(Action), this, method));
                }
            }
            Status = ManagerStatus.STARTING;
        }

        public ManagerStatus Status
        {
            get
            {
                return _status;
            }
            protected set
            {
                _status = value;
                if (value != oldStatus)
                    Messenger<ManagerStatus>.Broadcast(ManagerEvent.MANAGER_STATE_CHANGED, value, MessengerMode.DONT_REQUIRE_LISTENER);
                if (value == ManagerStatus.STARTED)
                    Messenger<IGameManager>.Broadcast(ManagerEvent.MANAGER_STARTED, this);
                oldStatus = value;
            }
        }

        //You'll notice this is somewhat modeled after IAsyncResult
        public Exception Exception { get; set; }

        /// <summary>
        /// Make sure you put a "return" or other terminating statement after this one if you want the program to terminate, as this function may not.
        /// </summary>
        /// <param name="e">The exception you want to throw (instead of throw new Exception(...), use Manager_Throw(new Exception(...)...)</param>
        /// <param name="isTerminating">Whether or not the manager intends to terminate after the Manager_Throw call.</param>
        protected void Manager_Throw(Exception e, bool isTerminating = false)
        {
            if (OnException == null)
            {
                this.Status = ManagerStatus.FAULTED;
                this.Exception = e;
                Messenger<IGameManager, Exception>.Broadcast(ManagerEvent.MANAGER_FAULTED, this, e, MessengerMode.DONT_REQUIRE_LISTENER);
                throw e;
            }
            else
            {
                OnException(this, new UnhandledExceptionEventArgs(e, isTerminating));
                Messenger<IGameManager, Exception>.Broadcast(ManagerEvent.MANAGER_FAULT_HANDLED, this, e, MessengerMode.DONT_REQUIRE_LISTENER);
            }
        }

        protected abstract void StartManager(DataService dataService);
        public void Startup(DataService dataService)
        {
            StartManager(dataService);
            Startup_Complete();
        }

        protected void Startup_Complete()
        {
            this.Status = ManagerStatus.STARTED;
        }


        [System.AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
        protected sealed class OnAttribute : Attribute
        {
            // See the attribute guidelines at 
            //  http://go.microsoft.com/fwlink/?LinkId=85236
            readonly string _eventName;

            // This is a positional argument
            public OnAttribute(string EventName)
            {
                this._eventName = EventName;
            }

            public string EventName
            {
                get { return _eventName; }
            }
        }

        
    }

}
