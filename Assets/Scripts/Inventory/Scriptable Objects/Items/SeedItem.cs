using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Seed Object", menuName = "Inventory System/Items/Seed")]
public class SeedItem : Item
{
    
    public void Awake()
    {
        type = ItemType.Seed;
    }
}
