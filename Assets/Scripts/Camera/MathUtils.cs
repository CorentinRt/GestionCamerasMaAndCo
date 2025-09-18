using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public static Vector3 GetNearestPointOnSegment(Vector3 a, Vector3 b, Vector3 target)
    {
        Vector3 ab = b - a;
        Vector3 atarget = target - a;

        Vector3 n = Vector3.Normalize(ab);
        float dot = Vector3.Dot(atarget, n);
        dot = Mathf.Clamp(dot , 0, ab.magnitude);
        Vector3 nearest = a + n * dot;

        Debug.DrawRay(a + nearest, Vector3.up, Color.red, 0f);

        return nearest;
    }

    #region Bezier
    public static Vector3 LinearBezier(Vector3 A, Vector3 B, float t)
    {
        return (1 - t) * A + t * B;
    }

    public static Vector3 QuadraticBezier(Vector3 A, Vector3 B, Vector3 C, float t)
    {
        return (1 - t) * LinearBezier(A, B, t) + t * LinearBezier(B, C, t);
    }

    public static Vector3 CubicBezier(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t)
    {
        return (1 - t) * QuadraticBezier(A, B, C, t) + t * QuadraticBezier(B, C, D, t);
    }
    #endregion
}
