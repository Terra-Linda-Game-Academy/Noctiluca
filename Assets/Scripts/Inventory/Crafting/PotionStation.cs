using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public class PotionStation : MonoBehaviour
{
    //Replace with type fluid. Will be null(?) if empty
    protected Fluid currentFluid;
    public RecipePool recipePoolScriptableObject;
    public IEnumerator AddIngredient(Fluid fluid, Item item)
    {
        Fluid newFluid = recipePoolScriptableObject.Search(fluid, item);
        yield return LerpFluids(currentFluid, newFluid);
        //Lerp Cauldron Material (Art Team Stuff) <- (not me)

        currentFluid = newFluid;
    }
    void Drain()
    {
        //set currentFluid to air
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
}
