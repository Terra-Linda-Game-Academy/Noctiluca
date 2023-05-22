using Misc;
using UnityEngine;

namespace Potions.Fluids {
    public class StunFluid : Fluid {
        private readonly float time;

        public StunFluid(
            float time, Gradient primaryColor, Gradient secondaryColor, float cooldown, float size,
            float sizeRandom, float lifetime, float lifetimeRandom, float exponent
        ) : base(primaryColor, secondaryColor, cooldown, size, sizeRandom, lifetime, lifetimeRandom, exponent) {
            this.time = time;
        }

        public override void ApplyEffect(GameObject obj, float lifetimeProgress) {
            if (obj.GetComponent<Stun>() is { } stun) stun.TimeRemaining += time;
        }
        
        
        
    }
}