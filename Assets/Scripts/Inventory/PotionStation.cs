using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public class PotionStation : MonoBehaviour
{
    //Replace with type fluid. Will be null(?) if empty
    protected Fluid currentFluid;
    public RecipePool recipePoolScriptableObject;
    public IEnumerator AddIngredient(Item item, Fluid fluid)
    {
        Fluid newFluid = recipePoolScriptableObject.Search(item, fluid);
        yield return LerpFluids(currentFluid, newFluid);
        //Lerp Cauldron Material (Art Team Stuff) <- (not me)

        currentFluid = newFluid;
    }
    void Drain()
    {
        currentFluid = recipePoolScriptableObject.air;
    }
    void Retrieve()
    {
        //get the fluid container from player (or somethin idfk)
        //send the fluid data from currentFluid

        Drain();
    }
    IEnumerator LerpFluids(Fluid start, Fluid end)
    {
        //do stuff to Material.lerp (maybe overflow with particles depending on fillLevel)

        float duration = 5f;
        float elapsed = 0f;
        while (elapsed > duration)
        {
            elapsed += Time.deltaTime;
            //lerp time=(elapsed/duration)
            yield return null;
        }
    }

    //placeholder Fluid&Item

    public readonly struct Fluid
    {
        public readonly string name;
        public readonly float fillLevel;

        public Fluid(string name, float fillLevel)
        {
            this.name = name;
            this.fillLevel = fillLevel;
        }
    }

    public readonly struct Item
    {
        public readonly string name;
        public readonly float randomThingyToDebug;

        public Item(string name, float randomThingyToDebug)
        {
            this.name = name;
            this.randomThingyToDebug = randomThingyToDebug;
        }
    }

}
