using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IceSheetController : MonoBehaviour
{
    public DelaunayTriangulationTester delaunayTriangulationTester;
    public PolygonCollider2D shape;


    public List<FrostPoint> frostPoints = new List<FrostPoint>();

    public float maxParticleLife = 5f;

    public MeshFilter meshFilter;



    public void AddPointToShape(Vector3 point)
    {

        //shape.points = shape.points.Concat(new Vector2[]{point}).ToArray();

        frostPoints.Add(new FrostPoint(point));

        UpdatePoints();


    }

    public void UpdatePoints()
    {
        if (frostPoints.Count > 2)
        {
            meshFilter.gameObject.SetActive(true);
            shape.points = frostPoints.ConvertAll<Vector2>((fp) => new Vector2(fp.position.x, fp.position.z)).ToArray();
            delaunayTriangulationTester.RunTestPolygonColliders();
        } else
        {
            meshFilter.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
