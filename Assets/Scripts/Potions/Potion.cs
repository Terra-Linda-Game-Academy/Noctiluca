using Potions.Fluids;
using UnityEditor;

namespace Potions {
    public class Potion {
        public readonly Fluid Fluid;
        public readonly float Capacity;
        public float Remaining { get; }

        public float NormalizedRemaining => Remaining / Capacity;

        public string Name() {
            string fluidTypeName = Fluid.GetType().Name;

            string[] words = ObjectNames.NicifyVariableName(fluidTypeName).Split(" ");

            string name = "";

            for (int i = 0; i < words.Length - 1; i++) {
                name += words[i] + " ";
            }

            name += "Potion";
            
            return name;
        }

        public Potion(Fluid fluid, float capacity, float remaining) {
            Fluid = fluid;
            Capacity = capacity;
            Remaining = remaining;
        }
    }
}