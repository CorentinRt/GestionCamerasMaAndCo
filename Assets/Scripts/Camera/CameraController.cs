using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // ----- FIELDS ----- //
    public static CameraController Instance;

    public Camera Camera;
    private CameraConfiguration CameraConfiguration;

    private List<AView> _fixedViews = new List<AView>();
    // ----- FIELDS ----- //

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
            
        Instance = this;
    }

    private void Update()
    {
        ApplyConfiguration();
        CameraConfiguration = ComputeAverage();
    }

    private void ApplyConfiguration()
    {
        Camera.transform.position = CameraConfiguration.GetPosition();
        Camera.transform.rotation = CameraConfiguration.GetRotation();
        Camera.fieldOfView = CameraConfiguration.FOV;
    }

    public void AddView(AView view)
    {
        if (view == null) return;
        if (_fixedViews.Contains(view)) return;

        _fixedViews.Add(view);  
    }

    public void RemoveView(AView view)
    {
        if (view == null) return;
        if (!_fixedViews.Contains(view)) return;

        _fixedViews.Remove(view);
    }

    #region Compute Average
    public CameraConfiguration ComputeAverage()
    {
        Vector3 positionsAverage = ComputePositionAverage();
        Quaternion rotationAverage = ComputeRotationAverage();
        float distanceAverage = ComputeDistanceAverage(); 
        float fovAverage = ComputeFOVAverage(); 

        CameraConfiguration config = new CameraConfiguration();

        config.Pivot = positionsAverage;

        config.Yaw = rotationAverage.eulerAngles.y;
        config.Pitch = rotationAverage.eulerAngles.x;
        config.Roll = rotationAverage.eulerAngles.z;

        config.Distance = distanceAverage;
        config.FOV = fovAverage;

        return config;
    }

    private Vector3 ComputePositionAverage()
    {
        Vector3 positionsSum = Vector3.zero;
        float weightSum = 0;

        foreach (AView view in _fixedViews)
        {
            positionsSum += view.GetConfiguration().GetPosition() * view.Weight;
            weightSum += view.Weight;
        }

        return positionsSum / weightSum;
    }
    private Quaternion ComputeRotationAverage()
    {
        float pitchSum = 0;
        float rollSum = 0;

        float weightSum = 0;

        foreach (AView view in _fixedViews)
        {
            pitchSum += view.GetConfiguration().Pitch * view.Weight;
            rollSum += view.GetConfiguration().Roll * view.Weight;

            weightSum += view.Weight;
        }

        return Quaternion.Euler(ComputeAverageYaw(), pitchSum / weightSum, rollSum / weightSum);
    }

    public float ComputeAverageYaw()
    {
        Vector2 sum = Vector2.zero;
        foreach (AView view in _fixedViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            sum += new Vector2(Mathf.Cos(config.Yaw * Mathf.Deg2Rad), Mathf.Sin(config.Yaw * Mathf.Deg2Rad)) * view.Weight;
        }
        return Vector2.SignedAngle(Vector2.right, sum);
    }

    private float ComputeDistanceAverage()
    {
        float distanceSum = 0;
        float distanceWeightSum = 0;

        foreach(AView view in _fixedViews)
        {
            distanceSum += view.GetConfiguration().Distance * view.Weight;
            distanceWeightSum += view.Weight;
        }

        return distanceSum / distanceWeightSum; 
    }

    private float ComputeFOVAverage()
    {
        float fovSum = 0;
        float fovWeightSum = 0;

        foreach (AView view in _fixedViews)
        {
            fovSum += view.GetConfiguration().FOV * view.Weight;
            fovWeightSum += view.Weight;
        }

        return fovSum / fovWeightSum;
    }
    #endregion
}
