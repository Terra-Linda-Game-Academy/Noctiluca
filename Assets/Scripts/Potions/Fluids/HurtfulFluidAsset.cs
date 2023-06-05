using UnityEngine;

namespace Potions.Fluids {
    [CreateAssetMenu(fileName = "Hurtful Fluid", menuName = "Hurtful Fluid", order = 0)]
    public class HurtfulFluidAsset : FluidAsset {
        [SerializeField] private int damage;
        
        protected override Fluid GetFluid(
            Gradient primaryColor, Gradient secondaryColor, float cooldown, float size, 
            float sizeRandom, float lifetime, float lifetimeRandom, float exponent
        ) => new HurtfulFluid(
            damage, primaryColor, secondaryColor, cooldown, 
            size, sizeRandom, lifetime, lifetimeRandom, exponent
        );
    }
}