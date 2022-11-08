using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobManagerExample : MonoBehaviour
{
    public List<GameObject> mobs = new List<GameObject>();

    [ConsoleCommand("spawnmob", "spawns a mob", true, "Mob spawn succeful")]
    public void SpawnMob(MobSpawnInfo mobSpawnInfo)
    {
        GameObject mob = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mob.name = mobSpawnInfo.name;
        mob.transform.position = mobSpawnInfo.positon;
        mob.GetComponent<MeshRenderer>().material.color = mobSpawnInfo.color;
        mobs.Add(mob);
    }

    [ConsoleCommand("killmob", "kills a mob by name", true, "killed mob named ^0^")]
    public void KillMob(string mobName)
    {
        //mobs.RemoveAll(mob => mob.name == mobName);
        var mobsToRemove = mobs.FindAll(mob => mob.name == mobName);
        mobsToRemove.ForEach(mob => { mobs.Remove(mob); Destroy(mob); });
    }

    [ConsoleCommand("mobcount", "gets number of mobs")]
    public int GetMobCount()
    {
        return mobs.Count;
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
        ConsoleArgument positionArgument = BaseConsoleParameters.ConsoleConvertVector3(args[1..4]);
        ConsoleArgument colorArgument = BaseConsoleParameters.ConsoleConvertColor(args[4..5]);
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
