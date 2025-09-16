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
}
