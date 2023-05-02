using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels;


public class HeightSlice {

    public Vector2Int position;
    public float topHeight;
    public float bottomHeight;
    

    public HeightSlice(Vector2Int position, float topHeight, float bottomHeight) {
        this.position = position;
        this.topHeight = topHeight;
        this.bottomHeight = bottomHeight;
    }

}

public class WaterTile : SimpleTile
{

    [SerializeField] private Vector2Int position;
    [SerializeField] private string name;

    public override string Name => name;
    public override Vector2Int Position => position;


    private Room room;
    

    public override void Init(GameObject obj, Room room)
    {
        this.room = room;
        
    }

    private Vector2Int[] adjecentPositions = new Vector2Int[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0)
    };

    public List<Vector2Int> GetAdjecentPoints(Vector2Int pos) {
        List<Vector2Int> adjecentPoints = new List<Vector2Int>();
        foreach (Vector2Int adjecentPos in adjecentPositions) {
            Vector2Int adjecentPoint = pos + adjecentPos;
            adjecentPoints.Add(adjecentPoint);
        }
        return adjecentPoints;
    }


    public List<Vector2Int> FilterPositions(List<Vector2Int> positions, System.Func<Vector2Int, bool> filter) {
        List<Vector2Int> filteredPositions = new List<Vector2Int>();
        foreach (Vector2Int pos in positions) {
            if (filter(pos)) {
                filteredPositions.Add(pos);
            }
        }
        return filteredPositions;
    }

    public List<Vector2Int> GetAdjecentFiltered(Vector2Int pos, System.Func<Vector2Int, bool> filter) {
        List<Vector2Int> adjecentPoints = GetAdjecentPoints(pos);
        List<Vector2Int> filteredPositions = FilterPositions(adjecentPoints, filter);
        return filteredPositions;
    }


    public List<Vector2Int> GetFloodFill(Vector2Int pos, System.Func<Vector2Int, bool> filter) {
        List<Vector2Int> floodFill = new List<Vector2Int>();
        List<Vector2Int> toCheck = new List<Vector2Int>();
        toCheck.Add(pos);
        while (toCheck.Count > 0) {
            Vector2Int currentPos = toCheck[0];
            toCheck.RemoveAt(0);
            if (filter(currentPos)) {
                floodFill.Add(currentPos);
                List<Vector2Int> adjecentPoints = GetAdjecentPoints(currentPos);
                foreach (Vector2Int adjecentPoint in adjecentPoints) {
                    if (!floodFill.Contains(adjecentPoint) && !toCheck.Contains(adjecentPoint)) {
                        toCheck.Add(adjecentPoint);
                    }
                }
            }
        }
        return floodFill;
    }

    public List<Vector2Int> GetWaterPositions(Vector2Int pos, float height) {
        return GetFloodFill(pos, (Vector2Int p) => room.GetTileAt(p.x, p.y).Height < height);
    }

    public HeightSlice ConverToWaterSlice(Vector2Int position, float topHeight) {
        float bottomHeight = room.GetTileAt(position.x , position.y).Height;
        return new HeightSlice(position, topHeight, bottomHeight);
    }

    public List<HeightSlice> ConvertToWaterSlices(List<Vector2Int> positions, float topHeight) {
        List<HeightSlice> waterSlices = new List<HeightSlice>();
        foreach (Vector2Int pos in positions) {
            HeightSlice waterSlice = ConverToWaterSlice(pos, topHeight);
            waterSlices.Add(waterSlice);
        }
        return waterSlices;
    }

    public List<HeightSlice> GetWaterSlices(Vector2Int pos, float height) {
        List<Vector2Int> waterPositions = GetWaterPositions(pos, height);
        List<HeightSlice> waterSlices = ConvertToWaterSlices(waterPositions, height);
        return waterSlices;
    }

}


