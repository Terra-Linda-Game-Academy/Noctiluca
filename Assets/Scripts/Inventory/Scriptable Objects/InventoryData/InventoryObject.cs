using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> ItemContainer = new List<InventorySlot>();
    public List<PotionSlot> PotionContainer = new List<PotionSlot>();
    public void AddItem(Item _item, int _ammount)
    {
        bool hasItem = false;
        for (int i = 0; i < ItemContainer.Count; i++)
        {
            if (ItemContainer[i].item == _item)
            {
                ItemContainer[i].AddAmmount(_ammount);
                hasItem = true;
                break;
            }
        }

        if (!hasItem)
        {
            ItemContainer.Add(new InventorySlot(_item, _ammount));
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int capacity;
    public int ammount;
    public InventorySlot(Item _item, int _ammount)
    {
        item = _item;
        ammount = _ammount;
    }

    public void AddAmmount(int value)
    {
        ammount += value;
    }
}

[System.Serializable]
public class PotionSlot
{
    public Potion potion;
    public float capacity;
    public PotionSlot(Potion _potion, float _capacity)
    {
        potion = _potion;
        capacity = _capacity;
    }

    public void Consume(int value)
    {
        capacity -= value;
    }
}