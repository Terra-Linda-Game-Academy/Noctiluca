using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Ingredient,
    Potion,
    Seed
}
public abstract class Item : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;
    public int itemId;
    public Sprite icon;
}
