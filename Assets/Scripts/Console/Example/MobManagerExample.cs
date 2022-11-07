using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobManagerExample : MonoBehaviour
{
    public List<GameObject> mobs = new List<GameObject>();

    [ConsoleCommand("spawnmob", "spawns a mob")]
    public string SpawnMob(MobSpawnInfo mobSpawnInfo)
    {
        GameObject mob = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mob.name = mobSpawnInfo.name;
        mob.transform.position = mobSpawnInfo.positon;
        mob.GetComponent<MeshRenderer>().material.color = mobSpawnInfo.color;
        return "Spawned!";
    }

    public void Start()
    {
        SpawnMob(new MobSpawnInfo("Bob", new Vector3(0, 0, 0), Color.blue));
    }
}

public class MobSpawnInfo : CustomConsoleParameter
{
    public static string ConsoleFormat = "name[string] position[vector3] color[color]";
    public static ConsoleArgument ConsoleConvert(string[] args)
    {//spawnmob mike 3 2 1 yellow
        ConsoleArgument positionArgument = BaseConsoleParameters.ConsoleConvertVector3(args[1..3]);
        ConsoleArgument colorArgument = BaseConsoleParameters.ConsoleConvertColor(args[4..4]);
        return new ConsoleArgument(new MobSpawnInfo(args[0], (Vector3)positionArgument.value, (Color)colorArgument.value), 5);
    }



    public string name;
    public Vector3 positon;
    public Color color;

    public MobSpawnInfo(string name, Vector3 positon, Color color)
    {
        this.name = name;
        this.positon = positon;
        this.color = color;
    }
}
