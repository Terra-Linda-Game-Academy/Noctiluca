using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public SnakeSegment snakeHead;
    public SnakeSegment snakeTail;

    public List<SnakeSegment> bodySegments;


    public Vector3 targetPosition;

    public Vector3[] pathPoints = new Vector3[0];
    public Vector3[] normalizedPathDirections = new Vector3[0];
    public float pathfindingResolution = 0.1f;
    private Vector3 currentPoint;
    private Vector3 currentDirection;

    public float speed = 5f;
    private bool pathfinding = false;

    public float swerveAmplitude = 5f;
    public float swerveScale = 5f;


    private void Start()
    {
        
    }
    private void Update()
    {
        if(pathPoints == null || pathPoints.Length == 0)
        {
            CalculatePath();
        } else
        {
            if(!pathfinding)
            {
                StartCoroutine(Pathfind());
            } else
            {
                snakeHead.transform.LookAt(targetPosition);
                snakeHead.transform.position = Vector3.Lerp(snakeHead.transform.position, currentPoint, Time.deltaTime * speed);
            }
        }
    }

    private IEnumerator Pathfind()
    {
        pathfinding = true;
        for (int i = 0; i < pathPoints.Length; i++)
        {
            currentPoint = pathPoints[i];
            currentDirection = new Vector3(normalizedPathDirections[i].x * 360f, normalizedPathDirections[i].y * 360f, normalizedPathDirections[i].z * 360f);
            yield return new WaitForSeconds(pathfindingResolution / speed);
        }
        pathPoints = new Vector3[0];
        pathfinding = false;
    }

    private void CalculatePath()
    {
        Vector3 currentPos = snakeHead.transform.position;
        float distanceToTarget = Vector3.Distance(currentPos, targetPosition);
        int pointCount = (int)(distanceToTarget / pathfindingResolution);

        pathPoints = new Vector3[pointCount];
        normalizedPathDirections = new Vector3[pointCount];

        Vector3 currentPoint = snakeHead.transform.position;
        Vector3 direction = targetPosition - currentPos;
        pathPoints[0] = currentPoint;
        for (int i = 1 ; i < pointCount; i++)
        {
            currentPoint += direction / (float)pathPoints.Length;

            //float theta = distanceToTarget / (float) i / sinSize;
            currentPoint = new Vector3(currentPoint.x + Mathf.Sin(distanceToTarget * (float)i / swerveAmplitude) * swerveScale, currentPoint.y, currentPoint.z + Mathf.Sin(distanceToTarget *(float)i / swerveAmplitude) * swerveScale);

            normalizedPathDirections[i-1] = (pathPoints[i - 1] - currentPoint).normalized;

            pathPoints[i] = currentPoint;
        }
        normalizedPathDirections[normalizedPathDirections.Length - 1] = normalizedPathDirections[normalizedPathDirections.Length - 2];
    }

    
}
