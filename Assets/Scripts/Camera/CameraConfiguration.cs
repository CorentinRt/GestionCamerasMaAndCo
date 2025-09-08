using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct CameraConfiguration 
{
    // ----- FIELDS ----- //
    public float Yaw;
    public float Pitch;
    public float Roll;

    public Vector3 Pivot;
    public float Distance;

    public float FOV;
    // ----- FIELDS ----- //

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(Yaw, Pitch, Roll);  
    }

    public Vector3 GetPosition()
    {
        Vector3 offset = GetRotation() * (Vector3.back * Distance);
        return Pivot + offset;
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;

        Gizmos.DrawSphere(Pivot, 0.25f);
        Gizmos.DrawLine(Pivot, GetPosition());

        Gizmos.matrix = Matrix4x4.TRS(GetPosition(), GetRotation(), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, FOV, 0.5f, 0f, Camera.main.aspect);
        Gizmos.matrix = Matrix4x4.identity;
    }

    public void OnDrawGizmos()
    {
        DrawGizmos(Color.blue);
    }
}
