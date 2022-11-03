using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Static Inventory", menuName = "Inventory/Static Inventory", order = 0)]
public class StaticInventory : ScriptableObject
{
    public List<PotionItem> potionItems = new List<PotionItem>();
    public List<SeedItem> seedItems = new List<SeedItem>();
}
