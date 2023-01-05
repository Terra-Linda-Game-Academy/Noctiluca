using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public string name;
    public float growStages;
    public float timeSpent;
    public PlantRepository.Item product; //product of plant
    public int lowProd, highProd; //range of items produced
    public List<GameObject> plantStagePrefabs;
}
