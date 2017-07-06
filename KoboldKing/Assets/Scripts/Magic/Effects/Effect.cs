using System;
using System.ComponentModel;
using UnityEngine;

namespace Assets.Scripts.Magic.Effects
{
    public abstract class Effect : MonoBehaviour
    {
        public EffectClass effectClass;
        public EffectType effectType;
        public float magnitude;
        public EffectApplicationType effectApplicationType = EffectApplicationType.GradualDamage;
        public DamageType gradualDamageType;

        private ParticleSystem partSys;
        protected GameObject target;
        private void Start()
        {
            partSys = GetComponent<ParticleSystem>();
            if (partSys != null)
            {
                var m = partSys.main;
                m.playOnAwake = false;
            }
        }
        /// <summary>
        /// Applies This effect to a recieving object.  
        /// </summary>
        /// <param name="Target">The <see cref="GameObject"/> to Apply this <see cref="Effect"/> to.</param>
        /// <param name="magnitude">The Magnitude of this effect (like severity of poison)</param>
        /// <param name="animateIfNotCompatible">If I can't apply myself to this gameobject (maybe I'm supposed to poison it, but it lacks a Damageable), should I still animate?</param>
        /// <returns>Whether or not this effect was compatible with the Target.</returns>
        public bool Apply(GameObject target, float magnitude = 1f, bool animateIfNotCompatible = false)
        {
            transform.SetParent(target.transform, false);

            bool compatible;
            switch (effectApplicationType)
            {
                case EffectApplicationType.None:
                    compatible = false;
                    break;
                case EffectApplicationType.GradualDamage:
                    compatible = true;
                    break;
                case EffectApplicationType.Custom:
                    throw new NotImplementedException("Custom Effect Types are currently not supported.  I suggest you use 'SendMessage()'  to implement it if you want it");
                default:
                    throw new InvalidEnumArgumentException("The Supplied EffectApplicationType is not valid");
            }
            if (compatible || animateIfNotCompatible)
            {
                partSys.Play();
            }
            return compatible;
        }
        

        protected bool TryDealDamage(float amount, out float actualAmount, DamageType damageType = DamageType.Effectual)
        {
            Damageable damageable = GetComponent<Damageable>();
            if (damageable == null)
            {
                actualAmount = 0;
                return false;
            }
            else
            {
                actualAmount = damageable.DealDamage(damageType, amount);
                return true;
            }
        }

        /// <summary>
        /// Use this method to apply whatever this effect is supposed to do (e.g. slowly damage the target).  Everything else should be covered by the base <see cref="Effect"/> class.
        /// </summary>
        /// <returns>Could the <see cref="Effect"/> be applied? (aka was this <see cref="Effect"/> compatible with the <see cref="GameObject"/>?</returns>
        protected abstract bool ApplyEffect(GameObject target, float magnitude);
    }

    public enum EffectApplicationType
    {
        None,
        /// <summary>
        /// This is like the default in that it uses typical damage.
        /// </summary>
        GradualDamage,
        Custom
    }
}
