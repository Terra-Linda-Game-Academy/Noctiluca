using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PotionTypes
{
    Throw,
    Spill,
    Splash,
    AOE
}

public class PotionType : ScriptableObject
{
    public Fluid fluid;
    //reference fluid for potion material

    public float capacity;
    public PotionTypes type;
}

public abstract class Potion : ScriptableObject
{
    public PotionType type;
    public float capacityRemaining;
}