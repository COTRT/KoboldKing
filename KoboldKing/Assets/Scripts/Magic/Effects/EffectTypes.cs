namespace Assets.Scripts.Magic.Effects
{
    public enum EffectClass
    {
        /// <summary>
        /// Increases or Decreases your odds of...  whatever.  (getting hit by enemy bullets, for example)
        /// </summary>
        ChanceModifier,
        /// <summary>
        /// Damage over long period of time (poison?)
        /// </summary>
        ExtendedDamage,
        /// <summary>
        /// Preventing something from happening (THOU SHALT NOT EAT spell.)
        /// </summary>
        Impedement,
        /// <summary>
        /// Increases or Decreases your skill at... whatever. (jump height, speed, etc.)
        /// </summary>
        SkillModifier,
        /// <summary>
        /// This effect does nothing but change appearances.  If it deals any damage or anything that could fall under another category, other than appearance, place it under the other category
        /// </summary>
        Appearance,
    }
    public enum EffectType
    {
        Positive, Negative, Neutral
    }
}