using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


using System;




public class IceSheetController : MonoBehaviour
{
    public DelaunayTriangulationTester delaunayTriangulationTester;
    public PolygonCollider2D shape;


    public List<FrostPoint> frostPoints = new List<FrostPoint>();

    public float maxParticleLife = 5f;

    public MeshFilter meshFilter;
    public MeshCollider meshCollider;


    bool hasChanged = false;

    private Vector3[] points = new Vector3[1000];
    private int pointCount = 0;


    public float minPointDist = 0.25f;

    public int ClosestNode(Vector3 node, Vector3[] nodes)
    {
        Vector3[] nodesArr = nodes.ToArray(); // Convert to array
        float[] distSqr = new float[nodesArr.Length];
        for (int i = 0; i < pointCount; i++)
        {
            distSqr[i] = Mathf.Pow(Vector3.Distance(nodesArr[i], node), 2);
        }
        return Array.IndexOf(distSqr, distSqr.Min());
    }


    public void AddPointToShape(Vector3 point)
    {
        if(pointCount < 3 ||Vector3.Distance(points[ClosestNode(point, points)], point) > minPointDist)
        {
            frostPoints.Add(new FrostPoint(point));


            hasChanged = true;
        }

        //shape.points = shape.points.Concat(new Vector2[]{point}).ToArray();

        

    }

    public void UpdatePoints()
    {
        if (frostPoints.Count > 2)
        {
            meshFilter.gameObject.SetActive(true);
            shape.points = frostPoints.ConvertAll<Vector2>((fp) => new Vector2(fp.position.x, fp.position.z)).ToArray();
            delaunayTriangulationTester.RunTestPolygonColliders();
            meshCollider.sharedMesh = meshFilter.mesh;
        } else
        {
            meshFilter.gameObject.SetActive(false);
        }
    }

    int tick = 0;

    // Update is called once per frame
    void FixedUpdate()
    {        if (hasChanged && tick >= 20)
        {
            UpdatePoints();
            hasChanged = false;
            tick = 0;
        }
        tick++;

        bool updatePoints = false;
        foreach (FrostPoint frostPoint in frostPoints)
        {
            frostPoint.currentLifeTime += Time.fixedDeltaTime;

            if(frostPoint.currentLifeTime >= maxParticleLife)
            {
                updatePoints = true;
            }
        }

        if(updatePoints)
        {
            frostPoints.RemoveAll((fp) => (fp.currentLifeTime >= maxParticleLife));
            UpdatePoints();
        }
        
    }


}

public class FrostPoint
{
    public Vector3 position;
    public float currentLifeTime = 0f;

    public FrostPoint(Vector3 position)
    {
        this.position = position;
    }


}
