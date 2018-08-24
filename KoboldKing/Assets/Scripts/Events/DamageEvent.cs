namespace Assets.Scripts.Events
{
    public static class DamageEvent
    {
        /// <summary>
        /// Called whenever a Damageable is damaged, whether or not the object in question is broken
        /// </summary>
        public const string DAMAGE_DEALT = "DAMAGE_DEALT";
        /// <summary>
        /// Called whenever a Damageable is damaged, but NOT broken
        /// </summary>
        public const string DAMAGED = "DAMAGED";
        /// <summary>
        /// Called whenever a Damageable is damaged, AND broken
        /// </summary>
        public const string BROKEN = "BROKEN";
    }
}