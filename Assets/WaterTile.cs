using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels;

public class WaterTile : SimpleTile
{
    public override string Name => name;

    public override Vector2Int Position => position;

    public override void Init(GameObject obj, Room room)
    {
        foreach(ITile tile in room.AllTiles)
        {
            
        }
    }

    [SerializeField] private Vector2Int position;
    [SerializeField] private string name;
}
