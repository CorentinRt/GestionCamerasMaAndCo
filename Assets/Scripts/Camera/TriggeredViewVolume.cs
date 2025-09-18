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
}
