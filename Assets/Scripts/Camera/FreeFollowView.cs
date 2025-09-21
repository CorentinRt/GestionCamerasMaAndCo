using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFollowView : AView
{
    // ----- FIELDS ----- //
    public float[] Pitch = new float[3];
    public float[] Roll = new float[3];
    public float[] FOV = new float[3];

    private float _newPitch;
    private float _newRoll;
    private float _newFOV;

    public float Yaw;
    public float YawSpeed;

    public GameObject Target;
    public Curve Curve;
    public float CurvePosition;
    public float CurveSpeed;
    public float Distance;

    private Vector3 _newPosition;

    [Space]

    [Header("Mouse parameters")]
    [SerializeField] private float _mouseSensitivityX = 1f;
    [SerializeField] private float _mouseSensitivityY = 1f;
    [SerializeField] private float _delayBeforePlayerCanControlView;
    private bool _playerCanControlView;

    private Coroutine _delayBeforeEnableControlViewCoroutine;

    // ----- FIELDS ----- //

    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration config = new CameraConfiguration();

        config.Pivot = _newPosition;

        config.Yaw = Yaw;
        config.Pitch = _newPitch;
        config.Roll = _newRoll;

        config.FOV = _newFOV;

        config.Distance = Distance;

        return config;
    }

    private void Start()
    {
        // Little delay to prevent init values of Axis Mouse X/Y to disturb start CurvePosition by too high value
        StartDelayBeforeEnableControlViewCoroutine();
    }

    private void Update()
    {
        // Yaw
        float mouseXDelta = Input.GetAxis("Mouse X");
        float mouseYDelta = Input.GetAxis("Mouse Y");

        if (!_playerCanControlView)
        {
            mouseXDelta = 0f;
            mouseYDelta = 0f;
        }

        if (Mathf.Abs(mouseXDelta) > 0f)
        {
            Yaw += YawSpeed * Time.deltaTime * mouseXDelta * _mouseSensitivityX;
        }

        if (Mathf.Abs(mouseYDelta) > 0f)
        {
            CurvePosition += CurveSpeed * Time.deltaTime * -mouseYDelta * _mouseSensitivityY;
        }
        Debug.Log($"Curve position is {CurvePosition.ToString()}", this);

        CurvePosition = Mathf.Clamp01(CurvePosition);
        _newPosition = Curve.GetPosition(CurvePosition, GetCurveToWorldMatrix());

        // Pitch, Roll & FOV
        InterpolateRotation(CurvePosition); 
    }

    private void InterpolateRotation(float percent)
    {
        float localPercent = 0f;

        if (percent <= 0.5f)
        {
            localPercent = percent / 0.5f;

            _newPitch = Mathf.Lerp(Pitch[0], Pitch[1], localPercent);
            _newRoll = Mathf.Lerp(Roll[0], Roll[1], localPercent);
            _newFOV = Mathf.Lerp(FOV[0], FOV[1], localPercent);
        }
        else
        {
            localPercent = (percent - 0.5f) / 0.5f;

            _newPitch = Mathf.Lerp(Pitch[1], Pitch[2], localPercent);
            _newRoll = Mathf.Lerp(Roll[1], Roll[2], localPercent);
            _newFOV = Mathf.Lerp(FOV[1], FOV[2], localPercent);
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Curve.DrawGizmos(Color.magenta, GetCurveToWorldMatrix());  
    }

    private Matrix4x4 GetCurveToWorldMatrix()
    {
        return Matrix4x4.TRS(transform.position, Quaternion.Euler(0, Yaw, 0), Vector3.one);
    }

    private void StartDelayBeforeEnableControlViewCoroutine()
    {
        StopDelayBeforeEnableControlViewCoroutine();

        _delayBeforeEnableControlViewCoroutine = StartCoroutine(DelayBeforeEnableControlViewCoroutine());
    }

    private void StopDelayBeforeEnableControlViewCoroutine()
    {
        if (_delayBeforeEnableControlViewCoroutine != null)
        {
            StopCoroutine(_delayBeforeEnableControlViewCoroutine);
            _delayBeforeEnableControlViewCoroutine = null;
        }
    }

    private IEnumerator DelayBeforeEnableControlViewCoroutine()
    {
        yield return new WaitForSeconds(_delayBeforePlayerCanControlView);

        _playerCanControlView = true;

        StopDelayBeforeEnableControlViewCoroutine();
    }
}
