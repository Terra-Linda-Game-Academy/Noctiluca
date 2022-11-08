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
    public GameObject prefab;
    public FluidType type;
    public int fluidId;
    public Sprite icon;
    public Color fluidColor;
    public Material fluidMaterial;
}
