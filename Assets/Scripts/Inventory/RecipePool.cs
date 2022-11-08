using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PotionStation;

[CreateAssetMenu(fileName ="RecipePool", menuName="Inventory/Recipe Pool",order = 2)]
public class RecipePool : ScriptableObject
{

    //TODO: Determine if Dictionary should use Fluids & Items or be name based, determine how fluids & items are stored.

    public RecipePool()
    {
        recipeMatrix = new Dictionary<(string, string), Fluid>() {
            { ("water", "thing"), exampleFluid }
            //I am mega dumb somehow need list of fluids
        };
    }
    Dictionary<(string, string), Fluid> recipeMatrix;

    public Fluid Search(Item item, Fluid fluid)
    {
        return recipeMatrix.ContainsKey((item.name, fluid.name)) ? recipeMatrix[(item.name, fluid.name)] : goop ;
        //"goop" is spicy water (potion doesn't exist)
    }

    //placeholder Example Fluids & Items
    public Fluid air = new Fluid("air", 0f);
    public Fluid goop = new Fluid("goop", 1f);
    public Fluid exampleFluid = new Fluid("exampleFluid", 0.5f);

    public Item exampleItem = new Item("debugItem", 1f);
}
