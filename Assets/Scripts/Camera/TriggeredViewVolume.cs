using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggeredViewVolume : AViewVolume
{
    // ----- FIELDS ----- //
    public GameObject Target;
    // ----- FIELDS ----- //

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Target)
        {
            SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Target)
        {
            SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        DrawGizmos(Color.green);
    }

    private void DrawGizmos(Color color)
    {
        if (!TryGetComponent<Collider>(out Collider collider))
            return;

        Gizmos.color = color;
        Gizmos.matrix = transform.localToWorldMatrix;

        if (collider is BoxCollider box)
        {
            Gizmos.DrawWireCube(box.center, box.size);
        }
        else if (collider is SphereCollider sphere)
        {
            Gizmos.DrawWireSphere(sphere.center, sphere.radius);
        }
        else if (collider is CapsuleCollider capsule)
        {
            Vector3 up = Vector3.up * (capsule.height / 2 - capsule.radius);

            switch (capsule.direction)
            {
                case 0: up = Vector3.right * (capsule.height / 2 - capsule.radius);
                    break;

                case 2: up = Vector3.forward * (capsule.height / 2 - capsule.radius);
                    break;
            }

            Vector3 center = capsule.center;
            Gizmos.DrawWireSphere(center + up, capsule.radius);
            Gizmos.DrawWireSphere(center - up, capsule.radius);
        }
    }
}
