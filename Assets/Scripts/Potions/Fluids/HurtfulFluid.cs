using Enemies;
using Player;
using UnityEngine;

namespace Potions.Fluids {
    public class HurtfulFluid : Fluid {
        private readonly int damage;

        public HurtfulFluid(
            int damage, Gradient primaryColor, Gradient secondaryColor, float cooldown, 
            float size, float sizeRandom, float lifetime, float lifetimeRandom, float exponent
        ) : base(primaryColor, secondaryColor, cooldown, size, sizeRandom, lifetime, lifetimeRandom, exponent) {
            this.damage = damage;
        }

        public override void ApplyEffect(GameObject obj, float lifetimeProgress) {
            if (obj.GetComponent<PlayerHealthController>() is { } ph) {
                ph.Damage(1);
            } else if (obj.GetComponent<EnemyHealthController>() is { } eh) {
                eh.Health -= Mathf.CeilToInt(damage * (1 - lifetimeProgress));
            }
        }
    }
}