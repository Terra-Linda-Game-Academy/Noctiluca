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


    private NavMeshPath m_LinearPath;

    public NavMeshPath LinearPath
    {
        get { return m_LinearPath; }
        set {
            m_LinearPath = value;

            List<Vector3> newPath = new List<Vector3>();
            BezierSpline bezier = new BezierSpline(LinearPath.corners);
            float totalDistance = bezier.GetPathLength();
            for (float i = 0; i < totalDistance; i += 0.1f)
            {
                newPath.Add(bezier.Evaluate(i / totalDistance));
            }
        }
    }

    private NavMeshPath m_SlitherPath;

    public NavMeshPath SlitherPath
    {
        get { return m_SlitherPath; }
        set
        {
            m_SlitherPath = value;


        }
    }

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
        LinearPath = new NavMeshPath();

    }
   // NavMeshPathStatus navMeshPathStatus;

    // Update is called once per frame
    void Update()
    {

        if(lastDestination != targetDestination.position)
        {
            lastDestination = targetDestination.position;
            //navMeshAgent.SetDestination(targetDestination.position);
            //navMeshAgent.SetDestination(targetDestination.position);
            navMeshAgent.CalculatePath(targetDestination.position, LinearPath);

        }

        if (LinearPath != null)
        {
            for (int i = 0; i < LinearPath.corners.Length - 1; i++)
            {
                Debug.DrawLine(LinearPath.corners[i], LinearPath.corners[i + 1]);
            }
        }

        //navMeshAgent.CalculatePath(targetDestination.position, Path);

        if (SlitherPath != null)
        {
            for(int i = 0; i < SlitherPath.corners.Length-1; i++)
            {
                Debug.DrawLine(SlitherPath.corners[i], SlitherPath.corners[i+1]);
            }
        }

        
    }
}
