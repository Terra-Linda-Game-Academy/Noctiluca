using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class SlitherNavigationTest : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    [SerializeField] private Transform targetDestination;
    private Vector3 lastDestination = Vector3.zero;


    private NavMeshPath linearPath;


    private float currentDistance = 0f;

    public float speed = 5f;
    public float rotationSpeed = 5f;
    public Vector3 pathOffset;



    public bool enableBezier;
    public bool enableSine;

    [Header("Sine Settings")]

    public float waveFrequency = 1f;
    public float waveAmplitide = 1f;
    public float endFadeBuffer = 2.5f;

    [Header("Debbuging")]
    public bool alwaysCalculatePath = false;
    


    // private float m_WaveFrequency = 1f;
    // public float waveFrequency
    // {
    //     get { return m_WaveFrequency; }
    //     set { m_WaveFrequency = value; 
    //     CalculatePath(); }
    // }

    // private float m_waveAmplitide = 1f;
    // public float waveAmplitide
    // {
    //     get { return m_waveAmplitide; }
    //     set { m_waveAmplitide = value; 
    //     CalculatePath(); }
    // }
    
    // private float m_endFadeBuffer = 2.5f;
    // public float endFadeBuffer
    // {
    //     get { return m_endFadeBuffer; }
    //     set { m_endFadeBuffer = value; 
    //     CalculatePath(); }
    // }

    
    public float segmentSize = 0.1f;

    private SnakePath m_SnakePath;
    public SnakePath snakePath
    {
        get { return m_SnakePath; }
        set { m_SnakePath = value; 
        //currentDistance = 0f; 
        
        }
    }
    
    private Vector3[] bezierPath = new Vector3[0];
    private Vector3[] linearPathPoints = new Vector3[0];


    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    List<GameObject> snakeBody = new List<GameObject>();
    [SerializeField] Vector3 spawnOffset;
    
    

    public static List<Vector3> PathAsSteps(Vector3[] path, float stepLength)
    {
        List<Vector3> steps = new List<Vector3>();
        if (stepLength <= 0)
        {
            throw new ArgumentException("stepLength must be > 0");
        }
        Vector3 p0 = path[0];
        steps.Add(p0);
        int s = 1;
        while (s < path.Length)
        {
            // work on path segment of p0 to p1
            Vector3 p1 = path[s];
            float segmentLength = Vector3.Magnitude(p1 - p0);
            // bite off a stepSize (eg 1 meter) piece of the segment to get the next step
            if (segmentLength >= stepLength)
            {
                p0 = Vector3.MoveTowards(p0, p1, stepLength);
                steps.Add(p0);
            }
            else
            {
                float carryover = stepLength - segmentLength;
                p0 = p1;
                s++;
                if (s < path.Length)
                {
                    p1 = path[s];
                    p0 = Vector3.MoveTowards(p0, p1, carryover);
                    steps.Add(p0);
                }
            }
        }
        return steps;
    }



    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        linearPath = new NavMeshPath();

        for(int i = 0; i < bodyParts.Count; i++) {
            snakeBody.Add(Instantiate(bodyParts[i], transform.position + spawnOffset * i, transform.rotation));

            //bodyParts.RemoveAt(0);
        }
        bodyParts.Clear();

    }
   // NavMeshPathStatus navMeshPathStatus;



    public void CalculatePath() {
        transform.position = snakeBody[snakeBody.Count-1].transform.position;

        currentDistance = (snakeBody.Count-1) * Vector3.Distance(spawnOffset, Vector3.zero);


        navMeshAgent.CalculatePath(targetDestination.position, linearPath);
        calculatedSplinePath = false;
    }

    bool calculatedSplinePath = false;
    // Update is called once per frame
    void Update()
    {
        if(alwaysCalculatePath) {
            CalculatePath();
        }

        if(lastDestination != targetDestination.position && !alwaysCalculatePath)
        {
            lastDestination = targetDestination.position;
            //navMeshAgent.SetDestination(targetDestination.position);
            //navMeshAgent.SetDestination(targetDestination.position);
            CalculatePath();
        }

        if(linearPath.status == NavMeshPathStatus.PathComplete && !calculatedSplinePath){
            calculatedSplinePath = true;
            if(linearPath.corners.Length < 2)
                return;
            


            //Make the snake maintain rotation when the gamobject teleports
            Vector3[] points = new Vector3[linearPath.corners.Length+1];
            points[0] = snakeBody[snakeBody.Count-1].transform.position;
            points[1] = snakeBody[0].transform.position + snakeBody[0].transform.forward*2;

            for(int i = 1; i < linearPath.corners.Length; i++) {
                points[i+1] = linearPath.corners[i];
            }

            linearPathPoints = points;

            List<Vector3> newPath = new List<Vector3>();


            //SPLINE CONVERSION, CAN BE SWAPPED WITH ANY ALGORITHM
            //---------------------------------------------

            double[] xConerPositions = new double[points.Length];
            double[] yPositions = new double[points.Length];
            double[] zConerPositions = new double[points.Length];

            for(int i = 0; i < points.Length; i++) {
                xConerPositions[i] = points[i].x;
                yPositions[i] = points[i].y;
                zConerPositions[i] = points[i].z;
            }

            //                                                                                           number of segments  |
            (double[] xSplinePositions, double[] zSplinePositions) = Cubic.InterpolateXY(xConerPositions, zConerPositions, 1000);

            for(int i = 0; i < xSplinePositions.Length; i++) {
                newPath.Add(new Vector3((float)xSplinePositions[i], 0f, (float)zSplinePositions[i]));
            }

            //-----------------------------------------------------------------------


            // CatmullRomSpline bezier = new CatmullRomSpline(linearPath.corners);
            // float totalDistance = bezier.GetPathLength();
            // for (float i = 0; i < totalDistance; i += segmentSize)
            // {
            //     newPath.Add(bezier.Evaluate(i / totalDistance));
            // }
            bezierPath = newPath.ToArray();
            Vector3[] sinPath = AddSinWave(newPath.ToArray(), waveAmplitide, waveFrequency, endFadeBuffer);
            snakePath = new SnakePath(sinPath, GetTotalDistance(sinPath));


            // BezierSpline bezier = new BezierSpline();
            // bezier.controlPoints = linearPath.corners;
            // bezier.refrenceTransform = transform;
            // Vector3[] sinPath = AddSinWave(bezier.GetSpline(0.1f), waveAmplitide, waveFrequency);
            // snakePath = new SnakePath(sinPath, GetTotalDistance(sinPath));
        }

        if (linearPath != null && linearPathPoints.Length > 2) 
        {
            for (int i = 0; i < linearPathPoints.Length - 1; i++)
            {
                Debug.DrawLine(linearPathPoints[i], linearPathPoints[i + 1]);
                Debug.DrawLine(linearPathPoints[i], linearPathPoints[i] + Vector3.up, Color.red);
            }
            Debug.DrawLine(linearPathPoints[linearPathPoints.Length - 1], linearPathPoints[linearPathPoints.Length - 1] + Vector3.up, Color.red);
        }

        for (int i = 0; i < bezierPath.Length - 1; i++)
        {
            Debug.DrawLine(bezierPath[i], bezierPath[i + 1], Color.blue);
        }

        for (int i = 0; i < bezierPath.Length - 1; i++)
        {
            Debug.DrawLine(bezierPath[i], bezierPath[i + 1], Color.blue);
        }
 
        //navMeshAgent.CalculatePath(targetDestination.position, Path);

        for(int i = 0; i < snakePath.path.Length-1; i++)
        {
            Debug.DrawLine(snakePath.path[i], snakePath.path[i+1], Color.green);
        }

        if(snakePath != null && snakePath.path.Length > 1) 
        {
            currentDistance += Time.deltaTime * speed;
            if(currentDistance > snakePath.distance) 
                currentDistance = snakePath.distance;

            //transform.position = GetPointAtDistance(snakePath.path, currentDistance) + pathOffset;
        }

        for(int i = 0; i < snakeBody.Count; i++) {
            snakeBody[i].transform.position = GetPointAtDistance(snakePath.path, currentDistance - i*Vector3.Distance(spawnOffset, Vector3.zero));
            Quaternion targetLookDir = Quaternion.LookRotation(GetPointAtDistance(snakePath.path, currentDistance - i*Vector3.Distance(spawnOffset, Vector3.zero) + 0.1f) - GetPointAtDistance(snakePath.path, currentDistance - i*Vector3.Distance(spawnOffset, Vector3.zero)));
            snakeBody[i].transform.rotation = Quaternion.Lerp(snakeBody[i].transform.rotation, targetLookDir, Time.deltaTime * rotationSpeed);
        }
        //snakeBody[0].transform.LookAt(targetDestination);

        //transform.position = snakeBody[0].transform.position;
        
    }

     public static Vector3[] AddSinWave(Vector3[] points, float waveHeight, float waveFrequency, float endFadeBuffer)
    {
        Vector3[] newPoints = new Vector3[points.Length];

        float totalDistance = 0f;
        Vector3 currentPoint = Vector3.zero;
        Vector3 nextPoint = Vector3.zero;

        Vector3 lastDirection = Vector3.zero;
        

        float pathDistance = GetTotalDistance(points);


        for (int i = 0; i < points.Length - 1; i++)
        {
            currentPoint = points[i];
            nextPoint = points[i + 1];
            

            Vector3 direction = (nextPoint - currentPoint).normalized;
            Vector3 perpendicular = Vector3.Cross(direction, Vector3.up).normalized;

            float segmentDistance = Vector3.Distance(currentPoint, nextPoint);
            totalDistance += segmentDistance;

            float sinValue = Mathf.Sin(totalDistance * waveFrequency) * waveHeight *  Mathf.Clamp01((pathDistance - totalDistance) / (endFadeBuffer)); //* Mathf.Clamp01(((curveReductionTreshold - MathF.Abs((direction-lastDirection).magnitude) ) / curveReductionTreshold));
            newPoints[i] = currentPoint + perpendicular * sinValue;

            lastDirection = direction;
        }

        newPoints[points.Length - 1] = points[points.Length - 1];
        return newPoints;
    }

    public static Vector3 GetPointAtDistance(Vector3[] points, float distance)
    {
        float totalDistance = 0f;
        Vector3 currentPoint = Vector3.zero;
        Vector3 nextPoint = Vector3.zero;

        for (int i = 0; i < points.Length - 1; i++)
        {
            currentPoint = points[i];
            nextPoint = points[i + 1];

            float segmentDistance = Vector3.Distance(currentPoint, nextPoint);
            if (totalDistance + segmentDistance >= distance)
            {
                float remainingDistance = distance - totalDistance;
                float t = remainingDistance / segmentDistance;
                Vector3 direction = (nextPoint - currentPoint).normalized;
                return Vector3.Lerp(currentPoint, nextPoint, t);
            }
            totalDistance += segmentDistance;
        }

        return Vector3.zero;
    }


    public static float GetTotalDistance(Vector3[] points)
    {
        float totalDistance = 0f;
        Vector3 currentPoint = Vector3.zero;
        Vector3 nextPoint = Vector3.zero;

        for (int i = 0; i < points.Length - 1; i++)
        {
            currentPoint = points[i];
            nextPoint = points[i + 1];

            float segmentDistance = Vector3.Distance(currentPoint, nextPoint);
            totalDistance += segmentDistance;
        }

        return totalDistance;
    }



}


public class SnakePath {
    public Vector3[] path;
    public float distance;

    public SnakePath(Vector3[] path, float distance) {
        this.path = path;
        this.distance = distance;
    }

}
