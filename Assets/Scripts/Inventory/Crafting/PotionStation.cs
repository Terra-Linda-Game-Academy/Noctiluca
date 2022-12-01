using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public class PotionStation : MonoBehaviour
{
    public Fluid defaultFluid;
    protected Fluid currentFluid;
    public RecipePool recipePoolScriptableObject;

    //Should be run by player. Player should wait for this to stop executing before being able to run again
    public IEnumerator AddIngredient(Item item)
    {
        Fluid newFluid = recipePoolScriptableObject.Search(currentFluid, item);
        yield return LerpFluids(currentFluid, newFluid);
        //Lerp Cauldron Material (Art Team Stuff) <- (not me)

        currentFluid = newFluid;
    }
    void Drain()
    {
        currentFluid = defaultFluid;
    }
    void Retrieve()
    {
        //TODO:
        //get the fluid container from player (or somethin idk)
        //send the fluid data from currentFluid

        Drain();
    }

    //blend/lerp between fluid materials
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
