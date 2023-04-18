using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestTile {
    public Vector2Int position;
    public float height;
}


public class WaterFillTest : MonoBehaviour
{
    List<TestTile> tiles = new List<TestTile>();

    public Vector2 noiseScale = new Vector2(5f, 5f);
    public float noiseMultiplier = 5f;


    public Vector2Int waterTile = new Vector2Int(0, 0);
    public float waterHeight = 1f;

    //fill the tiles list with some test data
    void Start()
    {
        for (int i = 0; i < 100; i++) {
            for (int j = 0; j < 100; j++) {
                TestTile tile = new TestTile();
                tile.position = new Vector2Int(i, j);
                //perlin noise
                tile.height = Mathf.PerlinNoise(i / noiseScale.x, j / noiseScale.y) * noiseMultiplier;

                if(i == 0 || i == 99 || j == 0 || j == 99)
                    tile.height = 25f;
                tiles.Add(tile);
                
            }
        }
        SpawnTiles();
        
        List<WaterSlice> waterSlices = GetWaterSlices(waterTile, waterHeight);
        Debug.Log(waterSlices.Count);
        SpawnWaterSlices(waterSlices);
    }

    public TestTile GetTileAtPosition(Vector2Int pos) {
        foreach (TestTile tile in tiles) {
            if (tile.position == pos) {
                return tile;
            }
        }
        return null;
    }

    public void SpawnTile(TestTile tile) {
        //Spawn primative cube with height and make its bottom at 0
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        cube.transform.position = new Vector3(tile.position.x, tile.height / 2, tile.position.y);
        cube.transform.localScale = new Vector3(1, tile.height, 1);

        
    }

    public void SpawnTiles() {
        foreach (TestTile tile in tiles) {
            SpawnTile(tile);
        }
    }
    public void SpawnWaterSlice(WaterSlice waterSlice) {
        //Spawn primative cube with height and make its bottom at 0
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(waterSlice.position.x, waterSlice.topHeight / 2, waterSlice.position.y);
        cube.transform.localScale = new Vector3(1, waterSlice.topHeight - waterSlice.bottomHeight, 1);

        //set color to blue
        cube.GetComponent<Renderer>().material.color = Color.blue;

        cube.name = "Water";
    }

    public void SpawnWaterSlices(List<WaterSlice> waterSlices) {
        foreach (WaterSlice waterSlice in waterSlices) {
            SpawnWaterSlice(waterSlice);
        }
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
        return GetFloodFill(pos, (Vector2Int p) => GetTileAtPosition(p).height < height);
    }

    public WaterSlice ConverToWaterSlice(Vector2Int position, float topHeight) {
        float bottomHeight = GetTileAtPosition(position).height;
        return new WaterSlice(position, topHeight, bottomHeight);
    }

    public List<WaterSlice> ConvertToWaterSlices(List<Vector2Int> positions, float topHeight) {
        List<WaterSlice> waterSlices = new List<WaterSlice>();
        foreach (Vector2Int pos in positions) {
            WaterSlice waterSlice = ConverToWaterSlice(pos, topHeight);
            waterSlices.Add(waterSlice);
        }
        return waterSlices;
    }

    public List<WaterSlice> GetWaterSlices(Vector2Int pos, float height) {
        Debug.Log("Starting");
        List<Vector2Int> waterPositions = GetWaterPositions(pos, height);
        Debug.Log("Positions: " + waterPositions.Count);
        List<WaterSlice> waterSlices = ConvertToWaterSlices(waterPositions, height);
        Debug.Log("Slices: " + waterSlices.Count);
        return waterSlices;
    }
}
