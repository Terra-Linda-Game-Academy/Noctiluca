using UnityEngine;

namespace Agronomy {
    public class Pot : MonoBehaviour
    {
        public Plant plant;
        public float stagesGrown;
        public bool  harvestable;

        void Start()
        {
            //todo: load plant from savesystem
            if (plant != null)
                Grow();
        }

        void Grow()
        {
            //add growtime between instances to plant
            //random number between 1 and # of floors fought

            //if "fertilizer" used, number of floors fought is zero
            stagesGrown += Random.Range(1, 2);

            if (stagesGrown > plant.growStages)
                harvestable = true;
        }

        void Plant()
        {
            //get plant from seed item
        }

        void Harvest()
        {
            //send Random.Range(lowProd, highProd) # of plant.product to inventory
            plant = null;
        }
    }
}
