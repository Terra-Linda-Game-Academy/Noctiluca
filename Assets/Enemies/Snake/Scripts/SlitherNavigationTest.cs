using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Snake.Utility;

namespace Snake
{
    public class SlitherNavigationTest : MonoBehaviour
    {
        private NavMeshAgent navMeshAgent;

        public Vector3 targetDestination;
        private Vector3 lastDestination = Vector3.zero;


        private NavMeshPath linearPath;


        public float currentDistance = 0f;
        private float totalDistanceTravelled = 0f;

        public float speed = 5f;
        public float followLerpAccuracy = 0.1f;
        public float rotationSpeed = 5f;
        public Vector3 pathOffset;



        public bool enableBezier;
        public bool enableSine;

        [Header("Path Settings")]
        public int pathBlendDistanceIndex;

        [Tooltip("Per 1m of linear path")]
        public int pathResolution = 25;

        [Header("Sine Settings")]

        public float waveFrequency = 1f;
        public float waveAmplitide = 1f;
        public float endFadeBuffer = 2.5f;
        public float obstacleBuffer = 1f;

        private float sineOffset = 0f;


        [Header("Debbuging")]
        public bool alwaysCalculatePath = false;
        [SerializeField] bool debugPathfinding = false;
        public float pathRegenerationAccuracy = 10f;

        public float pathRegenerationBuffer = 2f;
        
        public float segmentSize = 0.1f;

        public float rotationSinFade = 0.001f;

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
        [SerializeField] Vector3 rotationOffset;

        [SerializeField] NavMeshObstacle[] avoidanceAreas;


        public bool snakeMoving = true;



        [SerializeField] private float pathRecalculationTime = 1f;
        private float pathRecalculationTimer = 0f;


        private float snakeLength = Mathf.Infinity;


        void Start()
        {
            sineOffset= UnityEngine.Random.Range(0f, 500f);

            currentDistance = (snakeBody.Count - 1) * Vector3.Distance(spawnSeperationOffset, Vector3.zero);

            pathRecalculationTimer = pathRecalculationTime+0.1f;
            navMeshAgent = GetComponent<NavMeshAgent>();
            linearPath = new NavMeshPath();

            snakeLength = 0f;
            for(int i = 0; i < bodyParts.Count; i++) {
                if (bodyParts[i].scene.name == null)
                {
                    snakeBody.Add(Instantiate(bodyParts[i], transform.position + spawnSeperationOffset * i + offset, transform.rotation));
                } else
                {
                    snakeBody.Add(bodyParts[i]);
                    snakeBody[snakeBody.Count - 1].transform.position = transform.position + spawnSeperationOffset * i + offset;
                    snakeBody[snakeBody.Count - 1].transform.rotation = transform.rotation;
                }
                snakeLength+=spawnSeperationOffset.magnitude;
            }
            bodyParts.Clear();

            
        }

        public void ToggleAvoidance(bool toggle)
        {
            foreach(NavMeshObstacle obstacle in avoidanceAreas)
            {
                Debug.Log("Setting Active: " + toggle);
                obstacle.gameObject.SetActive(toggle);
            }
        }



        public void CalculatePath() {
            //Make sure it is not the first genration otherwise it may never generate
            
            if (snakePath.splinePath.Length > 100)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Ray(snakeBody[0].transform.position, snakeBody[0].transform.forward), out hit, pathRegenerationBuffer))
                {
                    Debug.Log("Return");
                    if(!hit.collider.gameObject.CompareTag("Snake"))
                        return;
                }
            }


            transform.position = snakeBody[0].transform.position;


            //spawn cubes to avoid insta 180
            //ToggleAvoidance(true);

            //navMeshAgent.setAreaCost



            navMeshAgent.CalculatePath(targetDestination, linearPath);
            calculatedSplinePath = false;
            snakeMoving = false;

            if(debugPathfinding)
                Debug.Log("Calculating Path...");
            //Debug.Break();
        }

        public void ConvertPath() {
            //Debug.Log("Converting Path...");

            if (linearPath.corners.Length < 2)
                    return;

           // ToggleAvoidance(false);
            calculatedSplinePath = true;



            transform.position = snakeBody[0].transform.position;

                Vector3[] points = new Vector3[linearPath.corners.Length + 1];

                Vector3[] currentSnakePositionPoints = new Vector3[0];


                Vector3 headPoint = Vector3.zero;

                

                if (PathTools.GetTotalDistance(snakePath.splinePath) > snakeBody.Count * spawnSeperationOffset.magnitude)
                {
                    Vector3 tailPoint = PathTools.GetIndexedPointAtDistance(snakePath.splinePath, currentDistance - (snakeBody.Count - 1) * Vector3.Distance(spawnSeperationOffset, Vector3.zero));
                    headPoint = PathTools.GetIndexedPointAtDistance(snakePath.splinePath, currentDistance);

                    int tailIndex = Array.IndexOf(snakePath.splinePath, tailPoint);
                    int headIndex = Array.IndexOf(snakePath.splinePath, headPoint);

                    if(headIndex+pathBlendDistanceIndex  < snakePath.splinePath.Length)
                        currentSnakePositionPoints = snakePath.splinePath[tailIndex..(headIndex+pathBlendDistanceIndex)];
                    else
                        currentSnakePositionPoints = snakePath.splinePath[tailIndex..headIndex];



                    points[0] = headPoint;
                    points[1] = headPoint + snakeBody[0].transform.forward*pathRegenerationBuffer;


                    for (int i = 1; i < linearPath.corners.Length; i++)
                    {
                        points[i + 1] = linearPath.corners[i];
                    }
                    

                } else
                {
                    points = linearPath.corners;
                }

                linearPathPoints = points;

                List<Vector3> newPath = new List<Vector3>();


                //SPLINE CONVERSION, CAN BE SWAPPED WITH ANY ALGORITHM (CURRENT ALGORITH IS ONLY 2D AND CAN OVER-CURVE INTO OBSTACLES)
                //---------------------------------------------

                double[] xConerPositions = new double[points.Length];
                double[] yPositions = new double[points.Length];
                double[] zConerPositions = new double[points.Length];

                for(int i = 0; i < points.Length; i++) {
                    xConerPositions[i] = points[i].x;
                    yPositions[i] = points[i].y;
                    zConerPositions[i] = points[i].z;
                }

                //                                              number of segments(resolution of spline no matter the distance)  |
                (double[] xSplinePositions, double[] zSplinePositions) = Cubic.InterpolateXY(xConerPositions, zConerPositions, pathResolution * (int)PathTools.GetTotalDistance(linearPathPoints));

                for(int i = 0; i < xSplinePositions.Length; i++) {
                    newPath.Add(new Vector3((float)xSplinePositions[i], 0f, (float)zSplinePositions[i]));
                }
                //---------------------------------------------
                
                bezierPath = newPath.ToArray();
                Vector3[] sinePath = PathTools.AddSineWave(newPath.ToArray(), waveAmplitide, waveFrequency, endFadeBuffer, obstacleBuffer, totalDistanceTravelled - currentDistance + sineOffset, rotationSinFade);
                
                
                //This code is resetting the position
                //----------------------------------------------------------------------------------------------------------------------------------
                if(currentSnakePositionPoints.Length > 1)
                {
                    List<Vector3> updatedSinePath = new List<Vector3>();
                    
                    int actualBlendDistanceIndex = Mathf.Min(Mathf.Clamp(currentSnakePositionPoints.Length, 0, pathBlendDistanceIndex), Mathf.Clamp(currentSnakePositionPoints.Length, 0, pathBlendDistanceIndex)); 

                    //Take the current region of the snake and blend it to the newly generate path
                    updatedSinePath.AddRange(currentSnakePositionPoints[0..(currentSnakePositionPoints.Length - pathBlendDistanceIndex)]);
                    List<Vector3> blendedRegion = new List<Vector3>();
                    int max = pathBlendDistanceIndex;
                    for (int i = 0; i < max; i++)
                    {
                        try
                        {
                            blendedRegion.Add(Vector3.Lerp(currentSnakePositionPoints[currentSnakePositionPoints.Length - pathBlendDistanceIndex + i], sinePath[i], (float)i/(float)max));
                        }
                        catch (Exception e) { Debug.LogError(e.Message); }
                    }
                    updatedSinePath.AddRange(blendedRegion);
                    updatedSinePath.AddRange(sinePath[pathBlendDistanceIndex..sinePath.Length]);


                    sinePath = updatedSinePath.ToArray();

                    currentDistance = (snakeBody.Count - 1) * Vector3.Distance(spawnSeperationOffset, Vector3.zero);
                }

                snakePath = new SnakePath(sinePath, bezierPath, PathTools.GetTotalDistance(sinePath));

                snakeMoving = true;
        }



        bool calculatedSplinePath = false;
        // Update is called once per frame
        void Update()
        {
            //timer is at top in case of error cascade
            pathRecalculationTimer += Time.deltaTime;

            if( ((Vector3.Distance(snakeBody[0].transform.position, targetDestination) > pathRegenerationAccuracy) &&
                Vector3.Distance(lastDestination, targetDestination) > pathRegenerationAccuracy )
            

                && pathRecalculationTimer > pathRecalculationTime)
                {
                    lastDestination = targetDestination;
                    pathRecalculationTimer = 0f;
                    
                    CalculatePath();
                    
                }//(Vector3.Distance(snakeBody[0].transform.position, targetDestination.position) < pathRegenerationAccuracy)



            if (linearPath.corners.Length < 2)
                return;


            //once NavMeshAgent calculation is done, convert the path
            if(linearPath.status == NavMeshPathStatus.PathComplete && !calculatedSplinePath){
                ConvertPath();
            }

            if(debugPathfinding) {
                Vector3 p = PathTools.GetIndexedPointAtDistance(snakePath.splinePath, currentDistance);
                Debug.DrawLine(p, p + new Vector3(0,5,0), Color.black);

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
        

                for(int i = 0; i < snakePath.splinePath.Length-1; i++)
                {
                    Debug.DrawLine(snakePath.splinePath[i], snakePath.splinePath[i+1], Color.green);
                }
            }
            

            //Needs optimization
            if(snakeMoving) {
                if(snakePath != null && snakePath.splinePath.Length > 1) 
                {
                    currentDistance += Time.deltaTime * speed;
                    totalDistanceTravelled += Time.deltaTime * speed;
                    if (currentDistance > snakePath.distance-0.1f) 
                        currentDistance = snakePath.distance-0.1f;
                }

                for(int i = 0; i < snakeBody.Count; i++) {
                    Quaternion targetLookDir = Quaternion.LookRotation(PathTools.GetPointAtDistance(snakePath.splinePath, currentDistance - i*Vector3.Distance(spawnSeperationOffset, Vector3.zero) + 0.1f) - PathTools.GetPointAtDistance(snakePath.splinePath, currentDistance - i*Vector3.Distance(spawnSeperationOffset, Vector3.zero)) + rotationOffset);
                    snakeBody[i].transform.rotation = Quaternion.Lerp(snakeBody[i].transform.rotation, targetLookDir, Time.deltaTime * rotationSpeed * speed);
                    snakeBody[i].transform.position = Vector3.Lerp(snakeBody[i].transform.position, PathTools.GetPointAtDistance(snakePath.splinePath, currentDistance - i * Vector3.Distance(spawnSeperationOffset, Vector3.zero)) + offset , speed * Time.deltaTime);
                }

                transform.position = snakeBody[0].transform.position;
            }
            
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
}
