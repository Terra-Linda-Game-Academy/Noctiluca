using System.Collections;
using System.Collections.Generic;
using Levels;
using UnityEngine;

public class IceFloeController : MonoBehaviour//<E, C> : MonoBehaviour 
   // where E : TileEntity<E, C> 
    //where C : TileEntityController<E, C>
{
    // Start is called before the first frame update
    public int size = 5; // size of the square

    public List<Vector2> points = new List<Vector2>();
    public float planeHeight = 0.0f;

    private void Start()
    {
        VoronoiNoise noise = new VoronoiNoise(100, 0f, 10f);

        points = noise.GetVoronoiCell(1);
        Debug.Log(points.Count);

        // Create a new mesh
        Mesh mesh = new Mesh();

        // Convert Vector2 points to Vector3 points with planeHeight
        Vector3[] vertices = new Vector3[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            vertices[i] = new Vector3(points[i].x, planeHeight, points[i].y);
        }

        // Define the triangles that make up the mesh
        int[] triangles = new int[(points.Count - 2) * 3];
        int index = 0;
        for (int i = 1; i < points.Count - 1; i++)
        {
            triangles[index] = 0;
            triangles[index + 1] = i;
            triangles[index + 2] = i + 1;
            index += 3;
        }

        // Define the normals of the mesh
        Vector3[] normals = new Vector3[vertices.Length];
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = Vector3.up;
        }

        // Set the mesh data
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;

        // Create a new game object to hold the mesh
        GameObject plane = new GameObject("Plane");

        // Add a MeshFilter component to the game object
        MeshFilter meshFilter = plane.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        // Add a MeshRenderer component to the game object
        MeshRenderer meshRenderer = plane.AddComponent<MeshRenderer>();

        // Set the material of the mesh renderer
        meshRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }


}

// public class IceChunk {
//     public Vector2[] shapePoints;

//     public IceChunk(Vector2[] shapePoints) {
//         this.shapePoints = shapePoints;
//     }
// }

// public class IceSheet {
//      public IceChunk[] iceChunks;
//      public IceSheet(int numChunks, Vector2 dimesions) {
//         GenerateChunks(numChunks, dimesions);
//     }

//     public void GenerateChunks(int numChunks, Vector2 dimesions) {

//     }
// }

public class VoronoiNoise {

    private List<Vector2> points;

    public VoronoiNoise(int numPoints, float minRange, float maxRange) {
        // Generate random points
        points = new List<Vector2>();
        for (int i = 0; i < numPoints; i++) {
            float x = Random.Range(minRange, maxRange);
            float y = Random.Range(minRange, maxRange);
            points.Add(new Vector2(x, y));
        }
    }

    public List<Vector2> GetVoronoiCell(int index) {
        // Compute Voronoi diagram
        List<Vector2> vertices = new List<Vector2>();
        for (int i = 0; i < points.Count; i++) {
            if (i == index) continue;
            Vector2 dir = points[i] - points[index];
            Vector2 mid = (points[i] + points[index]) / 2f;
            vertices.Add(mid + dir.normalized * 1000f);
        }
        List<Vector2> cell = new List<Vector2>();
        for (int i = 0; i < vertices.Count; i++) {
            for (int j = i+1; j < vertices.Count; j++) {
                Vector2 circumcenter = ComputeCircumcenter(points[index], vertices[i], vertices[j]);
                if (!float.IsNaN(circumcenter.x) && !float.IsNaN(circumcenter.y)) {
                    cell.Add(circumcenter);
                }
            }
        }
        return cell;
    }

    private Vector2 ComputeCircumcenter(Vector2 a, Vector2 b, Vector2 c) {
        float d = 2f * (a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y));
        if (d == 0f) {
            return Vector2.zero;
        }
        float ux = ((a.x * a.x + a.y * a.y) * (b.y - c.y) + (b.x * b.x + b.y * b.y) * (c.y - a.y) + (c.x * c.x + c.y * c.y) * (a.y - b.y)) / d;
        float uy = ((a.x * a.x + a.y * a.y) * (c.x - b.x) + (b.x * b.x + b.y * b.y) * (a.x - c.x) + (c.x * c.x + c.y * c.y) * (b.x - a.x)) / d;
        return new Vector2(ux, uy);
    }
}



