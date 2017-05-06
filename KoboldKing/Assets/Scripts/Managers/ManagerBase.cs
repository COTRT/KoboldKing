using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Data;
using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public abstract class ManagerBase : MonoBehaviour,IGameManager
    {
        private ManagerStatus _status;
        private ManagerStatus oldStatus;
        public event UnhandledExceptionEventHandler OnException;

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

        public abstract void Startup(DataService dataService);
        

        
    }
}
