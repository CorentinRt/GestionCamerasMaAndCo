using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AViewVolume : MonoBehaviour
{
    // ----- FIELDS ----- //
    public int Priority = 0;
    public AView View;

    private int _uID;

    public static int NextUID = 0;

    protected bool _isActive { get; private set; }
    // ----- FIELDS ----- //

    private void Awake()
    {
        _uID = NextUID;
        NextUID++;
    }

    public int GetUID()
    {
        return _uID;
    }

    public virtual float ComputeSelfWeight()
    {
        return 1f;
    }

    protected void SetActive(bool active)
    {
        if (active)
        {
            ViewVolumeBlender.Instance.AddVolume(this);
        }
        else
        {
            ViewVolumeBlender.Instance.RemoveVolume(this);

        }

        _isActive = active;
    }
}
