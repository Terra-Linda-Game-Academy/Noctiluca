using Potions.Fluids;
using UnityEngine;

namespace Potions {
    public class Potion {
        public readonly Fluid Fluid;
        public readonly float Capacity;
        public float Remaining { get; private set; }

        public float NormalizedCapacity => Remaining / Capacity;

        public Potion(Fluid fluid, float capacity, float remaining) {
            Fluid = fluid;
            Capacity = capacity;
            Remaining = remaining;
        }
    }
}