using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FluidType
{
    Base,
    Goop,
    Mix
}

[CreateAssetMenu(fileName = "New Fluid", menuName = "Inventory System/Fluids/Fluid")]
public class Fluid : ScriptableObject
{
    public FluidType type;
    public int fluidId;
    public Sprite icon;
    public Color fluidColor;
    public Material fluidMaterial;
    //public PotionItem potionItem;
    //(^ either PotionItem or this should have corresponding variable)
}
