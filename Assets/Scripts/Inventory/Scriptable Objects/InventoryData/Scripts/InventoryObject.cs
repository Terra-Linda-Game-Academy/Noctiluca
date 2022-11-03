using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryObject : ScriptableObject
{
    public List<ItemObject> container = new List<ItemObject>();
}

[System.Serializable]
public class PotionSlot
{
    public PotionObject potion;
    public int capacity;
    public int ammount;

    public PotionSlot(PotionObject potion, int ammount)
    {
        
    }

    public void AddAmmount(int value)
    {
        ammount += value;
    }
}