using UnityEngine;

public class BezierSpline : MonoBehaviour
{
    [SerializeField]
    public Vector3[] points;

    [SerializeField, Range(0, 1)]
    private float tension = 0.5f;

    public BezierSpline(Vector3[] points)
    {
        this.points = points;
    }

    public Vector3 Evaluate(float t)
    {
        t = Mathf.Clamp01(t);

        int n = points.Length - 1;
        int index = Mathf.FloorToInt(t * (float)n);
        float u = t * (float)n - (float)index;

        Vector3 p0 = points[ClampIndex(index - 1)];
        Vector3 p1 = points[ClampIndex(index)];
        Vector3 p2 = points[ClampIndex(index + 1)];
        Vector3 p3 = points[ClampIndex(index + 2)];

        return 0.5f * ((2 * p1) + (-p0 + p2) * u + (2 * p0 - 5 * p1 + 4 * p2 - p3) * u * u + (-p0 + 3 * p1 - 3 * p2 + p3) * u * u * u);
    }

    private int ClampIndex(int index)
    {
        int n = points.Length;
        if (index < 0)
        {
            return 0;
        }
        if (index >= n)
        {
            return n - 1;
        }
        return index;
    }


    public float GetTAtDistance(float distance)
    {
        int numSamples = 1000;
        float stepSize = 1f / numSamples;
        float totalLength = 0f;
        float t = 0f;
        Vector3 lastSample = Evaluate(0f);

        for (int i = 1; i <= numSamples; i++)
        {
            Vector3 newSample = Evaluate(i * stepSize);
            float segmentLength = Vector3.Distance(lastSample, newSample);
            if (totalLength + segmentLength >= distance)
            {
                float tStep = (distance - totalLength) / segmentLength;
                t = (i - 1) * stepSize + tStep * stepSize;
                break;
            }
            totalLength += segmentLength;
            lastSample = newSample;
        }

        return t;
    }

    public float GetPathLength()
    {
        int numSamples = 1000;
        float stepSize = 1f / numSamples;

        float totalLength = 0f;

        Vector3 lastSample = Evaluate(0f);

        for (int i = 1; i <= numSamples; i++)
        {
            Vector3 newSample = Evaluate(i * stepSize);
            float segmentLength = Vector3.Distance(lastSample, newSample);
            totalLength += segmentLength;

            lastSample = newSample;
        }

        return totalLength;
    }

}

