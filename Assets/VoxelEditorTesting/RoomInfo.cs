using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RoomInfo", menuName = "Dungeon Generation/New Room")]
public class RoomInfo : ScriptableObject
{
    public TextAsset file;

    [Range(0f,1f)]public float rarity = 0.5f;


    public RoomInfo[] nextRooms;

}
