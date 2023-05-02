using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RecipePool", menuName="Inventory System/Crafting/Recipe Pool",order = 2)]
public class RecipePool : ScriptableObject
{
    public List<Recipe> recipeList;

    public Fluid Search(Fluid fluid, Item item)
    {
        //Goes through list and finds compatable recipe, maybe refactor to use list.contains()
        foreach (Recipe recipe in recipeList) {
            if (recipe.fluid == fluid && recipe.item == item)
                return recipe.product;
        }

        return null; //potion doesn't exist (replace with goop (make goop))
    }

    [System.Serializable]
    public struct Recipe
    {
        public Fluid fluid;
        public Item item;
        public Fluid product;
        public string description;
        public Recipe(Fluid fluid, Item item,  Fluid product, string description)
        {
            this.fluid = fluid;
            this.item = item;
            this.product = product;
            this.description = description;
        }
    }
}
