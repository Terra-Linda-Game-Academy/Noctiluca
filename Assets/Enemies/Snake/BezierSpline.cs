using System;
using System.Linq;
using UnityEngine;


// using UnityEngine;



public class CatmullRomSpline
{
    public Vector3[] points;

    public CatmullRomSpline(Vector3[] points)
    {
        this.points = points;
    }

    public Vector3 Evaluate(float t)
    {
        //Debug.Log("Points : " + points.Length);

        if(points.Length == 0) {
            return Vector3.zero;
        } else if(points.Length == 1) {
            return points[0];
        }

        t = Mathf.Clamp01(t);

        int n = points.Length - 1;
        int index = Mathf.FloorToInt(t * (float)n);
        float u = t * (float)n - (float)index;

        Vector3 p0 = points[ClampIndex(index - 1)];
        Vector3 p1 = points[ClampIndex(index)];
        Vector3 p2 = points[ClampIndex(index + 1)];
        Vector3 p3 = points[ClampIndex(index + 2)];

        float t2 = u * u;
        float t3 = t2 * u;

        Vector3 m1 = (p2 - p0) * 0.5f;
        Vector3 m2 = (p3 - p1) * 0.5f;

        return (2 * p1 - 2 * p2 + m1 + m2) * t3 + (-3 * p1 + 3 * p2 - 2 * m1 - m2) * t2 + m1 * u + p1;
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
    int numSamples = 5000;
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

public static class Cubic
{
    /// <summary>
    /// Generate a smooth (interpolated) curve that follows the path of the given X/Y points
    /// </summary>
    public static (double[] xs, double[] ys) InterpolateXY(double[] xs, double[] ys, int count)
    {
        if (xs is null || ys is null || xs.Length != ys.Length)
            throw new ArgumentException($"{nameof(xs)} and {nameof(ys)} must have same length");

        int inputPointCount = xs.Length;
        double[] inputDistances = new double[inputPointCount];
        for (int i = 1; i < inputPointCount; i++)
        {
            double dx = xs[i] - xs[i - 1];
            double dy = ys[i] - ys[i - 1];
            double distance = Math.Sqrt(dx * dx + dy * dy);
            inputDistances[i] = inputDistances[i - 1] + distance;
        }

        double meanDistance = inputDistances.Last() / (count - 1);
        double[] evenDistances = Enumerable.Range(0, count).Select(x => x * meanDistance).ToArray();
        double[] xsOut = Interpolate(inputDistances, xs, evenDistances);
        double[] ysOut = Interpolate(inputDistances, ys, evenDistances);
        return (xsOut, ysOut);
    }

    private static double[] Interpolate(double[] xOrig, double[] yOrig, double[] xInterp)
    {
        (double[] a, double[] b) = FitMatrix(xOrig, yOrig);

        double[] yInterp = new double[xInterp.Length];
        for (int i = 0; i < yInterp.Length; i++)
        {
            int j;
            for (j = 0; j < xOrig.Length - 2; j++)
                if (xInterp[i] <= xOrig[j + 1])
                    break;

            double dx = xOrig[j + 1] - xOrig[j];
            double t = (xInterp[i] - xOrig[j]) / dx;
            double y = (1 - t) * yOrig[j] + t * yOrig[j + 1] +
                t * (1 - t) * (a[j] * (1 - t) + b[j] * t);
            yInterp[i] = y;
        }

        return yInterp;
    }

    private static (double[] a, double[] b) FitMatrix(double[] x, double[] y)
    {
        int n = x.Length;
        double[] a = new double[n - 1];
        double[] b = new double[n - 1];
        double[] r = new double[n];
        double[] A = new double[n];
        double[] B = new double[n];
        double[] C = new double[n];

        double dx1, dx2, dy1, dy2;

        dx1 = x[1] - x[0];
        C[0] = 1.0f / dx1;
        B[0] = 2.0f * C[0];
        r[0] = 3 * (y[1] - y[0]) / (dx1 * dx1);

        for (int i = 1; i < n - 1; i++)
        {
            dx1 = x[i] - x[i - 1];
            dx2 = x[i + 1] - x[i];
            A[i] = 1.0f / dx1;
            C[i] = 1.0f / dx2;
            B[i] = 2.0f * (A[i] + C[i]);
            dy1 = y[i] - y[i - 1];
            dy2 = y[i + 1] - y[i];
            r[i] = 3 * (dy1 / (dx1 * dx1) + dy2 / (dx2 * dx2));
        }

        dx1 = x[n - 1] - x[n - 2];
        dy1 = y[n - 1] - y[n - 2];
        A[n - 1] = 1.0f / dx1;
        B[n - 1] = 2.0f * A[n - 1];
        r[n - 1] = 3 * (dy1 / (dx1 * dx1));

        double[] cPrime = new double[n];
        cPrime[0] = C[0] / B[0];
        for (int i = 1; i < n; i++)
            cPrime[i] = C[i] / (B[i] - cPrime[i - 1] * A[i]);

        double[] dPrime = new double[n];
        dPrime[0] = r[0] / B[0];
        for (int i = 1; i < n; i++)
            dPrime[i] = (r[i] - dPrime[i - 1] * A[i]) / (B[i] - cPrime[i - 1] * A[i]);

        double[] k = new double[n];
        k[n - 1] = dPrime[n - 1];
        for (int i = n - 2; i >= 0; i--)
            k[i] = dPrime[i] - cPrime[i] * k[i + 1];

        for (int i = 1; i < n; i++)
        {
            dx1 = x[i] - x[i - 1];
            dy1 = y[i] - y[i - 1];
            a[i - 1] = k[i - 1] * dx1 - dy1;
            b[i - 1] = -k[i] * dx1 + dy1;
        }

        return (a, b);
    }
}
// public class BezierSpline
// {
//     public Vector3[] controlPoints;
//     public Transform refrenceTransform;

//     public int ControlPointCount
//     {
//         get { return controlPoints.Length; }
//     }

//     public Vector3 GetControlPoint(int index)
//     {
//         return controlPoints[index];
//     }

//     public void SetControlPoint(int index, Vector3 point)
//     {
//         controlPoints[index] = point;
//     }

//     public Vector3[] GetSpline(float resolution)
//     {
//         int pointCount = Mathf.RoundToInt(1f / resolution) + 1;
//         Vector3[] splinePoints = new Vector3[pointCount];

//         for (int i = 0; i < pointCount; i++)
//         {
//             float t = i * resolution;
//             splinePoints[i] = GetPoint(t);
//         }

//         return splinePoints;
//     }

//     public Vector3 GetPoint(float t)
//     {
//         if (ControlPointCount < 5)
//         {
//             return controlPoints[0];
//         }

//         int i;
//         if (t >= 1f)
//         {
//             t = 1f;
//             i = controlPoints.Length - 4;
//         }
//         else
//         {
//             t = Mathf.Clamp01(t) * ControlPointCount;
//             i = (int)t;
//             t -= i;
//             i *= 3;
//         }
//         return refrenceTransform.TransformPoint(Bezier.GetPoint(controlPoints[i], controlPoints[i + 1], controlPoints[i + 2], controlPoints[i + 3], t));
//     }

//     public static class Bezier
//     {
//         public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
//         {
//             t = Mathf.Clamp01(t);
//             float oneMinusT = 1f - t;
//             return oneMinusT * oneMinusT * oneMinusT * p0 + 3f * oneMinusT * oneMinusT * t * p1 + 3f * oneMinusT * t * t * p2 + t * t * t * p3;
//         }
//     }
    
    
// }


