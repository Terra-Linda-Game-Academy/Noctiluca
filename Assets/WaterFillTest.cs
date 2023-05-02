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
        CreateWater(waterSlices);
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
    
    public void CreateWater(List<WaterSlice> waterSlices) {
        //create a new game object add a mesh and creat a mesh based on the water slices
        GameObject water = new GameObject("Water");
        water.transform.position = new Vector3(-0.5f, 0f, -0.5f);
        MeshFilter meshFilter = water.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = water.AddComponent<MeshRenderer>();
        Mesh mesh = CreateWaterMesh(waterSlices);
        meshFilter.mesh = mesh;
        meshRenderer.material.color = Color.blue;


    }
    public WaterSlice GetAdjacentWaterSlice(List<WaterSlice> waterSlices, Vector2Int position, int direction) {
        foreach (WaterSlice waterSlice in waterSlices) {
            //direction 0 = (1,0) 1=(0,1) 2 = (-1,0) 3 = (0,-1)
            Vector2Int displacement = Vector2Int.zero;
            if (direction == 0) {
                displacement = new Vector2Int(1, 0);
            }
            else if (direction == 1) {
                displacement = new Vector2Int(0, 1);
            }
            else if (direction == 2) {
                displacement = new Vector2Int(-1, 0);
            }
            else if (direction == 3) {
                displacement = new Vector2Int(0, -1);
            }
            Vector2Int adjacentPosition = position + displacement;
            if (waterSlice.position == adjacentPosition) {
                return waterSlice;
            }
        }
        return null;
    }
    //combine meshes from list of meshes
    public Mesh CombineMeshes(List<Mesh> meshes) {
        CombineInstance[] combine = new CombineInstance[meshes.Count];
        for (int i = 0; i < meshes.Count; i++) {
            combine[i].mesh = meshes[i];
            combine[i].transform = Matrix4x4.identity;
        }
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);
        return combinedMesh;
    }

    

    public Mesh ScaleMesh(Mesh mesh, Vector3 scale) {
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++) {
            vertices[i] = Vector3.Scale(vertices[i], scale);
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        return mesh;
    }

    //create a cube mesh
    private Mesh CreateCube () {
		Vector3[] vertices = {
			new Vector3 (0, 0, 0),
			new Vector3 (1, 0, 0),
			new Vector3 (1, 1, 0),
			new Vector3 (0, 1, 0),
			new Vector3 (0, 1, 1),
			new Vector3 (1, 1, 1),
			new Vector3 (1, 0, 1),
			new Vector3 (0, 0, 1),
		};

		int[] triangles = {
			0, 2, 1, //face front
			0, 3, 2,
			2, 3, 4, //face top
			2, 4, 5,
			1, 2, 5, //face right
			1, 5, 6,
			0, 7, 4, //face left
			0, 4, 3,
			5, 4, 7, //face back
			5, 7, 6,
			0, 6, 7, //face bottom
			0, 1, 6
		};

			
		Mesh mesh = new Mesh ();
		mesh.Clear ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.Optimize ();
		mesh.RecalculateNormals ();
        mesh = MoveMesh(mesh, new Vector3(-0.5f, -0.5f, -0.5f));
        return mesh;
	}


    //Create box mesh based on a vector3, size
    public Mesh CreateBox(Vector3 size) {
        return ScaleMesh(CreateCube(), size);
    }
        
    //Move Mesh
    public Mesh MoveMesh(Mesh mesh, Vector3 offset) {
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++) {
            vertices[i] += offset;
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        return mesh;
    }

    public Mesh CreateWaterMesh(List<WaterSlice> waterSlices) {
        List<Mesh> boxMeshes = new List<Mesh>();

        foreach (WaterSlice waterSlice in waterSlices) {
            //difference between waterSlice.topHeight and waterSlice.bottomHeight is the height of the water slice
            float height = waterSlice.topHeight - waterSlice.bottomHeight;
            Vector3 size = new Vector3(1f, height, 1f);
            Mesh boxMesh = CreateBox(size);
            float middleHeight = (waterSlice.topHeight + waterSlice.bottomHeight) / 2;
            boxMesh = MoveMesh(boxMesh, new Vector3(waterSlice.position.x, middleHeight, waterSlice.position.y));
            boxMeshes.Add(boxMesh);
        }
        Mesh finalMesh = CombineMeshes(boxMeshes);
        finalMesh.Optimize ();
        finalMesh.RecalculateNormals();
        
        return finalMesh;
    }



    //create boxes for each water slice, but only creat the side faces of the box if there is not another water tile touching that face





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
        List<Vector2Int> waterPositions = GetWaterPositions(pos, height);
        List<WaterSlice> waterSlices = ConvertToWaterSlices(waterPositions, height);
        return waterSlices;
    }
}
