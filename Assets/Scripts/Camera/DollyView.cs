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

    private float _currentRailDistance = 0f;
    // ----- FIELDS ----- //

    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration config = new CameraConfiguration();

        config.Pivot = Rail.GetPosition(DistanceOnRail);
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
