using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class Floor1SetupScript : MonoBehaviour
{
    [SerializeField] private RuntimeVar<int> healthVar;
    void Start()
    {
        healthVar.Value = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
