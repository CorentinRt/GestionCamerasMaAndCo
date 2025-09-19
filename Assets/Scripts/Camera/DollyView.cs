using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyView : AView
{
    // ----- FIELDS ----- //
    public float Roll;
    public float Distance;
    public float FOV;

    public Transform Target;

    public Rail Rail;
    public float DistanceOnRail;
    public float Speed;

    public bool IsAuto;

    private Vector3 _nearestPoint;
    // ----- FIELDS ----- //

    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration config = new CameraConfiguration();
        
        if (IsAuto)
        {
            config.Pivot = Rail.GetPosition(GetNearestPointOnRail());
        }
        else
        {
            config.Pivot = Rail.GetPosition(DistanceOnRail);
        }
        
        Vector3 targetDirection = Target.position - config.Pivot;
        targetDirection = Vector3.Normalize(targetDirection);

        float targetYaw = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
        config.Yaw = targetYaw;

        float targetPitch = -Mathf.Asin(targetDirection.y) * Mathf.Rad2Deg;
        config.Pitch = targetPitch;

        Debug.Log($"Pitch : {targetPitch}, Yaw : {targetYaw}");

        config.Roll = Roll;

        config.FOV = FOV;

        config.Distance = Distance;

        return config;
    }

    private void Update()
    {
        if (!IsAuto)
        {
            if (Input.GetKey(KeyCode.A))
            {
                // Less distance
                DistanceOnRail -= Speed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D))
            {
                // More distance
                DistanceOnRail += Speed * Time.deltaTime;
            }
        }
    }

    public float GetNearestPointOnRail()
{
    float closestDistanceToTarget = float.MaxValue;
    float distanceAlongRailToClosestPoint = 0f;

    float accumulatedDistance = 0f;

    List<Transform> points = Rail.GetRailPoints();
    for (int i = 0; i < points.Count - 1; i++)
    {
        Vector3 a = points[i].position;
        Vector3 b = points[i + 1].position;

        Vector3 nearest = MathUtils.GetNearestPointOnSegment(a, b, Target.position);
        float distanceToTarget = Vector3.Distance(Target.position, nearest);

        float segmentLength = Vector3.Distance(a, b);
        float t = Vector3.Distance(a, nearest) / segmentLength;

        if (distanceToTarget < closestDistanceToTarget)
        {
            closestDistanceToTarget = distanceToTarget;
            distanceAlongRailToClosestPoint = accumulatedDistance + t * segmentLength;
            _nearestPoint = nearest; 
        }

        accumulatedDistance += segmentLength;
    }

    return distanceAlongRailToClosestPoint;
}


    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_nearestPoint, .5f);
    }

}
