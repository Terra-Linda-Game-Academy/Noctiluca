using UnityEngine;

namespace Potions.Fluids {
    [CreateAssetMenu(fileName = "Stun Fluid", menuName = "Stun Fluid", order = 0)]
    public class StunFluidAsset : FluidAsset {
        [SerializeField] private float time;

        protected override Fluid GetFluid(
            Gradient primaryColor, Gradient secondaryColor, float cooldown, float size,
            float sizeRandom, float lifetime, float lifetimeRandom, float exponent
        ) => new StunFluid(
            time, primaryColor, secondaryColor, cooldown, size,
            sizeRandom, lifetime, lifetimeRandom, exponent
        );
    }
}