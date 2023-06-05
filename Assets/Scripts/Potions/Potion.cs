using Potions.Fluids;

namespace Potions {
    public class Potion {
        public readonly Fluid Fluid;
        public readonly float Capacity;
        public          float Remaining;

        public float NormalizedRemaining => Remaining / Capacity;

        public bool IsEmpty => Remaining <= 0;

        public string Name() {
            if (IsEmpty) return "Empty Potion";
            
            string fluidTypeName = Fluid.GetType().Name;

            string name = fluidTypeName[..^5];

            name += " Potion";
            
            return name;
        }

        public Potion(Fluid fluid, float capacity, float remaining) {
            Fluid = fluid;
            Capacity = capacity;
            Remaining = remaining;
        }
    }
}