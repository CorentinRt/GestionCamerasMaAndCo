using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedFollowView : AView
{
    // ----- FIELDS ----- //
    public float Roll;
    public float FOV;
    public Transform Target;

    [Header("Central Point")]
    [SerializeField] private GameObject _centralPoint;
    [SerializeField] private float _yawOffsetMax;
    [SerializeField] private float _pitchOffsetMax;
    // ----- FIELDS ----- //

    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration config = new CameraConfiguration();

        Vector3 centralDirection = _centralPoint.transform.position - transform.position;
        centralDirection = Vector3.Normalize(centralDirection);
        float centralYaw = Mathf.Atan2(centralDirection.x, centralDirection.z) * Mathf.Rad2Deg;
        float centralPitch = -Mathf.Asin(centralDirection.y) * Mathf.Rad2Deg;

        Vector3 targetDirection = Target.position - transform.position;
        targetDirection = Vector3.Normalize(targetDirection);
        float targetYaw = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
        float targetPitch = -Mathf.Asin(targetDirection.y) * Mathf.Rad2Deg; 

        if (Mathf.Abs(centralYaw - targetYaw) > _yawOffsetMax)
        {
            targetYaw = _yawOffsetMax * Mathf.Sign(targetYaw - centralYaw);
        }

        config.Yaw = targetYaw;

        if (Mathf.Abs(centralPitch - targetPitch) > _pitchOffsetMax)
        {
            targetPitch = _pitchOffsetMax * Mathf.Sign(targetPitch - centralPitch);
        }

        config.Pitch = targetPitch;
        //config.Pitch = Mathf.Clamp(targetPitch, centralPitch - _pitchOffsetMax, centralPitch + _pitchOffsetMax);

        config.Roll = Roll;

        config.FOV = FOV;

        config.Pivot = transform.position;
        config.Distance = 0f;

        return config;
    }

}
