using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedView : AView
{
    // ----- FIELDS ----- //
    public float Yaw;
    public float Pitch;
    public float Roll;

    public float FOV;
    // ----- FIELDS ----- //

    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration config = new CameraConfiguration();

        config.Yaw = Yaw;
        config.Pitch = Pitch;
        config.Roll = Roll;

        config.FOV = FOV;

        config.Pivot = transform.position;
        config.Distance = 0f;

        return config;
    }
}
