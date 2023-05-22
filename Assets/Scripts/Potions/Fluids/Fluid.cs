using UnityEngine;


namespace Potions.Fluids {
    public abstract class Fluid {
        public readonly Gradient PrimaryColor;
        public readonly Gradient SecondaryColor;
        public readonly float Cooldown;
        private readonly float size;
        private readonly float sizeRandom;
        private readonly float lifetime;
        private readonly float lifetimeRandom;
        private readonly float exponent;
        
        protected Fluid(
            Gradient primaryColor, Gradient secondaryColor, float cooldown, 
            float size, float sizeRandom, float lifetime, float lifetimeRandom, float exponent
        ) {
            PrimaryColor = primaryColor;
            SecondaryColor = secondaryColor;
            Cooldown = cooldown;
            this.size = size;
            this.sizeRandom = sizeRandom;
            this.lifetime = lifetime;
            this.lifetimeRandom = lifetimeRandom;
            this.exponent = exponent;
        }

        public float InitialSize => Random.Range(size - sizeRandom, size + sizeRandom);
        public float InitialLifetime => Random.Range(lifetime - lifetimeRandom, lifetime + lifetimeRandom);
        
        public float LifeProgress(float trueLifetime, float secondsActive) => 
            Mathf.Clamp01(Mathf.Pow(secondsActive / trueLifetime, exponent));

        public abstract void ApplyEffect(GameObject obj, float lifetimeProgress);
    }
}