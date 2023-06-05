using UnityEngine;

namespace Potions.Fluids {
    [CreateAssetMenu(fileName = "Bouncy Fluid", menuName = "Bouncy Fluid", order = 0)]
    public class BouncyFluidAsset : FluidAsset {
        [SerializeField] private float strength;

        protected override Fluid GetFluid(
            Gradient primaryColor, Gradient secondaryColor, float cooldown, float size, 
            float sizeRandom, float lifetime, float lifetimeRandom, float exponent
        ) => new BouncyFluid(
            strength, primaryColor, secondaryColor, cooldown, 
            size, sizeRandom, lifetime, lifetimeRandom, exponent
        );
    }
}