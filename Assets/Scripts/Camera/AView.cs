using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AView : MonoBehaviour
{
    // ----- FIELDS ----- //
    public float Weight;

    public bool IsCutOnSwitch;
    // ----- FIELDS ----- //

    public abstract CameraConfiguration GetConfiguration();

    public void SetActive(bool active)
    {
        if (active)
        {
            CameraController.Instance.AddView(this);
        }
        else
        {
            CameraController.Instance.RemoveView(this);
        }

        if (IsCutOnSwitch)
        {
            ViewVolumeBlender.Instance.UpdateVolumes();
            CameraController.Instance.Cut();
        }
    }

    protected virtual void OnDrawGizmos()
    {
        GetConfiguration().DrawGizmos(Color.red);
    }
}
