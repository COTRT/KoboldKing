namespace Assets.Scripts.Events
{
    public static class ManagerEvent
    {
        public const string MANAGER_STATE_CHANGED = "MANAGER_STATE_CHANGED";
        public const string MANAGER_FAULTED = "MANAGER_FAULTED";
        public const string MANAGER_FAULT_HANDLED = "MANAGER_FAULT_HANDLED";
        /// <summary>
        /// Not to be confused with <see cref="StartupEvent.MANAGERS_STARTED"/> (plural), which marks the startup of ALL managers.
        /// </summary>
        public const string MANAGER_STARTED = "MANAGER_STARTED";
    }
}
