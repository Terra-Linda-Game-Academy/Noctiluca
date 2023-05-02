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
public abstract class Fluid : ScriptableObject
{
    public FluidType type;
    public int fluidId;
    public Color fluidColor;
    public Material fluidMaterial;

    public abstract void ApplyEffect(GameObject obj);
}
