using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ConsoleCommand("spawncoolcube", "spawns cool cube", requiresCheats = true, executionValue = "Spawned cool cube named {name}")]
    void SpawnCoolCube(string name, Color color, Vector3 pos)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = name;
        cube.GetComponent<MeshRenderer>().material.color = color;
        cube.transform.position = pos;
    }
}
