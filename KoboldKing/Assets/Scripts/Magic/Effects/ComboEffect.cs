using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Magic.Effects
{
    public class ComboEffect : Effect
    {
        public Effect[] Effects { get; set; }
        public ComboEffect()
        {

        }
        public ComboEffect(Effect[] Effects)
        {
            this.Effects = Effects;
        }

        protected override bool ApplyEffect()
        {
            bool wasCompatible = true;
            foreach (var effect in Effects)
            {
                wasCompatible = wasCompatible && effect.Apply(target);
            }
            return wasCompatible;
        }
    }
}
