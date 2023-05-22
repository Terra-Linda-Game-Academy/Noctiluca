using UnityEngine;

namespace Potions.Fluids {
    public class BouncyFluid : Fluid {
        private readonly float strength;

        public BouncyFluid(
            float strength, Gradient primaryColor, Gradient secondaryColor, float cooldown, 
            float size, float sizeRandom, float lifetime, float lifetimeRandom, float exponent
        ) : base(primaryColor, secondaryColor, cooldown, size, sizeRandom, lifetime, lifetimeRandom, exponent) {
            this.strength = strength;
        }

        public override void ApplyEffect(GameObject obj, float lifetimeProgress) {
            if (obj.GetComponent<Rigidbody>() is { } rb) 
                rb.AddForce(Vector3.up * strength, ForceMode.Impulse);
        }
    }
}