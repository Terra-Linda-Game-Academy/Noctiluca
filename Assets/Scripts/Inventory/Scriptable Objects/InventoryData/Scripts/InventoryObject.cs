using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();
    public void AddItem(ItemObject _item, int _ammount)
    {
        bool hasItem = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].AddAmmount(_ammount);
                hasItem = true;
                break;
            }
        }

        if (!hasItem)
        {
            Container.Add(new InventorySlot(_item, _ammount));
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int capacity;
    public int ammount;
    public InventorySlot(ItemObject _item, int _ammount)
    {
        item = _item;
        ammount = _ammount;
    }

    public void AddAmmount(int value)
    {
        ammount += value;
    }
}