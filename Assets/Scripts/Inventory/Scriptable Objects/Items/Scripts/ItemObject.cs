using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Default,
    Potion,
    Seed
}
public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;
    public int itemId;
}
