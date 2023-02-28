using System;
using System.Collections.Generic;
using UnityEngine;

namespace Snake.Utility
{
    public static class PathTools
    {
        public static Vector3[] AddSineWave(Vector3[] points, float waveHeight, float waveFrequency, float endFadeBuffer, float obstacle = 0f, float offset = 0f, float rotationSinFade = 0.001f)
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

                float sineValue = Mathf.Sin((totalDistance + offset) * waveFrequency) * waveHeight *  Mathf.Clamp01((pathDistance - totalDistance) / (endFadeBuffer)); //* Mathf.Clamp01(((curveReductionTreshold - MathF.Abs((direction-lastDirection).magnitude) ) / curveReductionTreshold));
                newPoints[i] = currentPoint + perpendicular * sineValue;

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
                Vector3 p1 = path[s];
                float segmentLength = Vector3.Magnitude(p1 - p0);
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

    }
}