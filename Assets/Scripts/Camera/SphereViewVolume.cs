using Unity.VisualScripting;
using UnityEngine;

public class SphereViewVolume : AViewVolume
{
    // ----- FIELDS ----- //
    public GameObject Target;

    public float OuterRadius;
    public float InnerRadius;

    private float _distance;
    // ----- FIELDS ----- //

    private void Update()
    {
        _distance = Vector3.Distance(transform.position, Target.transform.position);

        if (_distance <= OuterRadius && !_isActive)
        {
            SetActive(true);
        }
        else if (_distance > OuterRadius && _isActive)
        {
            SetActive(false);
        }
    }

    public override float ComputeSelfWeight()
    {
        if (_distance >= OuterRadius)
            return 0f; 

        if (_distance <= InnerRadius)
            return 1f; 

        float t = (OuterRadius - _distance) / (OuterRadius - InnerRadius);
        //Debug.Log(Mathf.Clamp01(t));
        return Mathf.Clamp01(t);
    }

    private void OnDrawGizmos()
    {
        DrawGizmos(Color.cyan);
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;

        Gizmos.DrawWireSphere(transform.position, OuterRadius);
        Gizmos.DrawWireSphere(transform.position, InnerRadius);
    }
}
