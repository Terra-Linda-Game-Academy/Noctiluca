using UnityEngine;

namespace Potions.Fluids {
    public abstract class FluidAsset : ScriptableObject {
        [SerializeField] private Gradient primaryColor;
        [SerializeField] private Gradient secondaryColor;
        [SerializeField] private float cooldown;
        [SerializeField] private float size;
        [SerializeField] private float sizeRandom;
        [SerializeField] private float lifetime;
        [SerializeField] private float lifetimeRandom;
        [SerializeField] private float exponent;
        
        protected abstract Fluid GetFluid(
            Gradient primaryColor, Gradient secondaryColor, float cooldown,
            float size, float sizeRandom, float lifetime, float lifetimeRandom, float exponent
        );

        public Fluid GetFluid() =>
            GetFluid(primaryColor, secondaryColor, cooldown, size, sizeRandom, lifetime, lifetimeRandom, exponent);
    }
}