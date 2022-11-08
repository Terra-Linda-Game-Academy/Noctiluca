using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RecipePool", menuName="Inventory System/Crafting/Recipe Pool",order = 2)]
public class RecipePool : ScriptableObject
{

    //TODO: Determine if Dictionary should use Fluids & Items or be name based, determine how fluids & items are stored.

    public RecipePool()
    {
        recipeMatrix = new Dictionary<(Fluid, Item), Fluid>() {
            { (air, exampleItem), exampleFluid }
        };
    }
    Dictionary<(Fluid, Item), Fluid> recipeMatrix;

    public Fluid Search(Fluid fluid, Item item)
    {
        return recipeMatrix.ContainsKey((fluid, item)) ? recipeMatrix[(fluid, item)] : goop ;
        //"goop" is spicy water (potion doesn't exist)
    }

    //placeholder Example Fluids & Items
    public Fluid air;
    public Fluid goop;
    public Fluid exampleFluid;

    public Item exampleItem;
    public List<Recipe> testList;

    [System.Serializable]
    public struct Recipe
    {
        public Fluid fluid;
        public Item item;
        public Fluid product;
        public Recipe(Fluid fluid, Item item,  Fluid product)
        {
            this.fluid = fluid;
            this.item = item;
            this.product = product;
        }
    }
}
