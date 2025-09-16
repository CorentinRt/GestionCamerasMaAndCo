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
    // ----- FIELDS ----- //

    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration config = new CameraConfiguration();

        Vector3 targetDirection = Target.position - transform.position;
        targetDirection = Vector3.Normalize(targetDirection);

        float targetYaw = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
        config.Yaw = targetYaw;

        float targetPitch = -Mathf.Asin(targetDirection.y) * Mathf.Rad2Deg;
        config.Pitch = targetPitch;

        config.Roll = Roll;

        config.FOV = FOV;

        config.Pivot = Rail.GetPosition(DistanceOnRail);
        config.Distance = Distance;

        return config;
    }

}
