namespace Assets.Scripts.Events
{
    public static class StartupEvent
    {
        /// <summary>
        /// Not to be confused with <see cref="ManagerEvent.MANAGER_STARTED"/> (not plural), which fires upon an individual Manager's startup.
        /// </summary>
        public const string MANAGERS_STARTED = "MANAGERS_STARTED";
        public const string MANAGERS_PROGRESS = "MANAGERS_PROGRESS";
    }
}
