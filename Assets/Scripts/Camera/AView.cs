using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AView : MonoBehaviour
{
    // ----- FIELDS ----- //
    public float Weight;

    public bool IsActiveOnStart;
    // ----- FIELDS ----- //

    private void Start()
    {
        SetActive(IsActiveOnStart);
    }

    public abstract CameraConfiguration GetConfiguration();

    public void SetActive(bool active)
    {
        if (active)
        {
            CameraController.Instance.AddView(this);
        }
        else
        {
            // remove
        }
    }
}
