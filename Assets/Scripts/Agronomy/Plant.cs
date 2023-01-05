using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant", menuName = "Plants/Plant", order = 1)]
public class Plant : ScriptableObject
{
    public string name;
    public float growStages;
    public float timeSpent;
    //public Item product; //product of plant
    public int lowProd, highProd; //range of items produced
    public List<GameObject> plantStagePrefabs;
}
