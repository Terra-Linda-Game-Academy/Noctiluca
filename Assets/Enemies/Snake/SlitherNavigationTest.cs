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

    [Header("Path Settings")]
    public int pathBlendDistanceIndex;

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

    public float pathRegenerationBuffer = 2f;
    
    public float segmentSize = 0.1f;

    private SnakePath m_SnakePath = new SnakePath(new Vector3[0], new Vector3[0], 0f);
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
    [SerializeField] Vector3 spawnSeperationOffset;
    [SerializeField] Vector3 offset;


    bool snakeMoving = true;



    [SerializeField] private float pathRecalculationTime = 1f;
    private float pathRecalculationTimer = 0f;
    

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
        pathRecalculationTimer = pathRecalculationTime+0.1f;
        navMeshAgent = GetComponent<NavMeshAgent>();
        linearPath = new NavMeshPath();

        for(int i = 0; i < bodyParts.Count; i++) {
            snakeBody.Add(Instantiate(bodyParts[i], transform.position + spawnSeperationOffset * i + offset, transform.rotation));

            //bodyParts.RemoveAt(0);
        }
        bodyParts.Clear();
    }
   // NavMeshPathStatus navMeshPathStatus;



    public void CalculatePath() {

        //if(snakePath.bezierPath.Length > 2)
        //    transform.position = GetPointAtDistance(snakePath.splinePath, currentDistance - (snakeBody.Count-1)*Vector3.Distance(spawnOffset, Vector3.zero));


        transform.position = snakeBody[0].transform.position;


        //Debug.Break();

        currentDistance = (snakeBody.Count-1) * Vector3.Distance(spawnSeperationOffset, Vector3.zero);


        navMeshAgent.CalculatePath(targetDestination.position, linearPath);
        calculatedSplinePath = false;
        snakeMoving = false;
    }

    bool calculatedSplinePath = false;
    // Update is called once per frame
    void Update()
    {
        if(alwaysCalculatePath) {
            snakeMoving = false;
            CalculatePath();
        }

        if(lastDestination != targetDestination.position && !alwaysCalculatePath && pathRecalculationTimer > pathRecalculationTime)
        {
            lastDestination = targetDestination.position;
            //navMeshAgent.SetDestination(targetDestination.position);
            //navMeshAgent.SetDestination(targetDestination.position);
            pathRecalculationTimer = 0f;
            snakeMoving = false;
            CalculatePath();
            
        }

        if(linearPath.status == NavMeshPathStatus.PathComplete && !calculatedSplinePath){
            calculatedSplinePath = true;
            if(linearPath.corners.Length < 2)
                return;



            transform.position = snakeBody[0].transform.position;

            Vector3[] points = new Vector3[linearPath.corners.Length + 1];

            Vector3[] currentSnakePositionPoints = new Vector3[0];



            float sinValueHead = Mathf.Sin(snakePath.distance * waveFrequency) * waveAmplitide * Mathf.Clamp01((currentDistance - snakePath.distance) / (endFadeBuffer));
            if (snakePath.splinePath.Length > 2)
            {

               
                Vector3 tailPoint = GetIndexedPointAtDistance(snakePath.splinePath, currentDistance - (snakeBody.Count - 1) * Vector3.Distance(spawnSeperationOffset, Vector3.zero));
                Vector3 headPoint = GetIndexedPointAtDistance(snakePath.splinePath, currentDistance);
                //Debug.Log($"tail:{tailPoint}, head:{headPoint}");
                int tailIndex = Array.IndexOf(snakePath.splinePath, tailPoint);
                int headIndex = Array.IndexOf(snakePath.splinePath, headPoint);
                //Debug.Log($"tail index:{tailIndex}, head index:{headIndex}");
                currentSnakePositionPoints = snakePath.splinePath[tailIndex..headIndex];

                //Debug.Log("SnakePos : " + currentSnakePositionPoints.Length);
                /*
                points[0] = headPoint;
                points[1] = headPoint + snakeBody[0].transform.forward*pathRegenerationBuffer;


                for (int i = 1; i < linearPath.corners.Length; i++)
                {
                    points[i + 1] = linearPath.corners[i];
                }
                */

            } else
            {
                points = linearPath.corners;
            }
            points = linearPath.corners;





            /*
            


            float sinValueHead = Mathf.Sin(snakePath.distance * waveFrequency) * waveAmplitide *  Mathf.Clamp01((currentDistance - snakePath.distance) / (endFadeBuffer)); //* Mathf.Clamp01(((curveReductionTreshold - MathF.Abs((direction-lastDirection).magnitude) ) / curveReductionTreshold));
            float sinValueTail= Mathf.Sin(snakePath.distance * waveFrequency) * waveAmplitide *  Mathf.Clamp01(((currentDistance - (snakeBody.Count-1)*Vector3.Distance(spawnSperationOffset, Vector3.zero)) - snakePath.distance) / (endFadeBuffer)); //* Mathf.Clamp01(((curveReductionTreshold - MathF.Abs((direction-lastDirection).magnitude) ) / curveReductionTreshold));

            points[0] = snakeBody[snakeBody.Count-1].transform.position - (snakeBody[snakeBody.Count-1].transform.right * sinValueTail);
            points[1] = snakeBody[0].transform.position - (snakeBody[0].transform.right * sinValueHead) + snakeBody[0].transform.forward * pathRegeneratBuffer;
            

            for(int i = 1; i < linearPath.corners.Length; i++) {
                points[i+1] = linearPath.corners[i];
            }
            */



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

            

            if(currentSnakePositionPoints.Length < sinPath.Length && currentSnakePositionPoints.Length > pathBlendDistanceIndex)
            {
                List<Vector3> updatedSinPath = new List<Vector3>();
                updatedSinPath.AddRange(currentSnakePositionPoints);
                updatedSinPath.AddRange(sinPath);
                sinPath = updatedSinPath.ToArray();

                /*
                updatedSinPath.AddRange(currentSnakePositionPoints[0..(currentSnakePositionPoints.Length - pathBlendDistanceIndex)]);
                List<Vector3> blendedRegion = new List<Vector3>();
                for (int i = 0; i < pathBlendDistanceIndex; i++)
                {
                    try
                    {
                        blendedRegion.Add(((currentSnakePositionPoints[(currentSnakePositionPoints.Length - pathBlendDistanceIndex)..currentSnakePositionPoints.Length][i] / 2f) + (sinPath[i] / 2f)));
                    }
                    catch (Exception e) { Debug.LogError(e.Message); }
                }
                updatedSinPath.AddRange(blendedRegion);
                updatedSinPath.AddRange(sinPath[pathBlendDistanceIndex..sinPath.Length]);
                */
            }


            /*
            //splic Paths
             if(snakePath.splinePath.Length > 500) {
                 Vector3[] splicedPath = new Vector3[snakePath.bezierPath.Length + sinPath.Length];
                 Array.Copy(snakePath.bezierPath[0..500], splicedPath, snakePath.bezierPath[0..500].Length);
                 Array.Copy(sinPath[500..sinPath.Length], 0, splicedPath, snakePath.bezierPath[0..500].Length, sinPath[500..sinPath.Length].Length);
                 //sinPath = snakePath.bezierPath[0..100] + sinPath[100..sinPath.Length];

                 snakePath = new SnakePath(splicedPath, bezierPath, GetTotalDistance(sinPath));
             } else {
                snakePath = new SnakePath(sinPath, bezierPath, GetTotalDistance(sinPath));
            }
            */

            snakePath = new SnakePath(sinPath, bezierPath, GetTotalDistance(sinPath));




            // BezierSpline bezier = new BezierSpline();
            // bezier.controlPoints = linearPath.corners;
            // bezier.refrenceTransform = transform;
            // Vector3[] sinPath = AddSinWave(bezier.GetSpline(0.1f), waveAmplitide, waveFrequency);
            // snakePath = new SnakePath(sinPath, GetTotalDistance(sinPath));

            snakeMoving = true;
        }

        Debug.DrawLine(snakeBody[snakeBody.Count-1].transform.position, snakeBody[snakeBody.Count-1].transform.position + Vector3.up * 2, Color.yellow);
        Debug.DrawLine(snakeBody[0].transform.position, snakeBody[0].transform.position + Vector3.up * 2, Color.yellow);

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

        for(int i = 0; i < snakePath.splinePath.Length-1; i++)
        {
            Debug.DrawLine(snakePath.splinePath[i], snakePath.splinePath[i+1], Color.green);
        }

        if(snakeMoving) {
            if(snakePath != null && snakePath.splinePath.Length > 1) 
            {
                currentDistance += Time.deltaTime * speed;
                if(currentDistance > snakePath.distance) 
                    currentDistance = snakePath.distance;

                //transform.position = GetPointAtDistance(snakePath.path, currentDistance) + pathOffset;
            }

            for(int i = 0; i < snakeBody.Count; i++) {
                snakeBody[i].transform.position = GetPointAtDistance(snakePath.splinePath, currentDistance - i*Vector3.Distance(spawnSeperationOffset, Vector3.zero)) + offset;
                Quaternion targetLookDir = Quaternion.LookRotation(GetPointAtDistance(snakePath.splinePath, currentDistance - i*Vector3.Distance(spawnSeperationOffset, Vector3.zero) + 0.1f) - GetPointAtDistance(snakePath.splinePath, currentDistance - i*Vector3.Distance(spawnSeperationOffset, Vector3.zero)));
                snakeBody[i].transform.rotation = Quaternion.Lerp(snakeBody[i].transform.rotation, targetLookDir, Time.deltaTime * rotationSpeed);
            }

            transform.position = snakeBody[0].transform.position;
        }
        //snakeBody[0].transform.LookAt(targetDestination);

        //transform.position = snakeBody[0].transform.position;
        pathRecalculationTimer+=Time.fixedDeltaTime;
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

    public static Vector3 GetIndexedPointAtDistance(Vector3[] points, float distance)
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
                return points[i];
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
    public Vector3[] splinePath = new Vector3[0];
    public Vector3[] bezierPath = new Vector3[0];
    
    public float distance = 0f;

    public SnakePath(Vector3[] splinePath, Vector3[] bezierPath, float distance) {
        this.splinePath = splinePath;
        this.bezierPath = bezierPath;
        this.distance = distance;
    }

}
