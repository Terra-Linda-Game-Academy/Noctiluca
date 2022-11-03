using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Inventory
{
    public List<PotionItem> potionItems = new List<PotionItem>();
    public List<SeedItem> seedItems = new List<SeedItem>();
}
