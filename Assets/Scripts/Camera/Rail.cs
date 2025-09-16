using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Rail : MonoBehaviour
{
    // ----- FIELDS ----- //
    public bool IsLoop;

    private float _length;

    private List<Transform> _railPoints = new List<Transform>();
    // ----- FIELDS ----- //

    private void Start()
    {
        GetChildrenAndLength();
    }

    private void GetChildrenAndLength()
    {
        if (_railPoints.Count > 0) _railPoints.Clear();
        _length = 0f;

        // Get rail points
        for (int i = 0; i < transform.childCount; i++)
        {
            _railPoints.Add(transform.GetChild(i));
        }

        // Init length
        for (int i = 0; i < _railPoints.Count - 1; i++)
        {
            _length += Vector3.Distance(_railPoints[i + 1].position, _railPoints[i].position);
        }

        if (IsLoop && _railPoints.Count > 0)
        {
            _length += Vector3.Distance(_railPoints[_railPoints.Count - 1].position, _railPoints[0].position);
        }

        //Debug.Log($"Rail length : {_length}");
    }

    public float GetLength()
    {
        return _length;
    }

    public Vector3 GetPosition(float distance)
    {
        if (_railPoints.Count == 0) return Vector3.zero;
        if (_railPoints.Count < 2 && _railPoints.Count == 1) return _railPoints[0].position;

        int targetIndex = 0;
        float totalLength = 0f;
        float targetLength = 0f;
        float remainingLength = 0f;

        if (IsLoop)
        {
            distance = Mathf.Repeat(distance, _length);
            
            for (int i = 0; i < _railPoints.Count; i++)
            {
                if (i == _railPoints.Count - 1)
                {
                    targetLength = Vector3.Distance(_railPoints[i].position, _railPoints[0].position);
                }
                else
                {
                    targetLength = Vector3.Distance(_railPoints[i + 1].position, _railPoints[i].position);
                }
                
                totalLength += targetLength;

                if (totalLength >= distance )
                {
                    remainingLength = distance - (totalLength - targetLength);
                    break;
                }

                if (i != _railPoints.Count - 1)
                    targetIndex++;
            }


            float percent = remainingLength / targetLength;
            //Debug.Log($"Percent : {percent}, target index : {targetIndex}");

            if (targetIndex == _railPoints.Count - 1)
            {
                return Vector3.Lerp(_railPoints[targetIndex].position, _railPoints[0].position, percent);
            }
            else
            {
                return Vector3.Lerp(_railPoints[targetIndex].position, _railPoints[targetIndex + 1].position, percent);
            }

        }
        else
        {
            distance = Mathf.Clamp(distance, 0f, _length);

            


            for (int i = 0; i < _railPoints.Count - 1; i++)
            {
                targetLength = Vector3.Distance(_railPoints[i + 1].position, _railPoints[i].position);
                totalLength += targetLength;

                if (totalLength >= distance)
                {
                    remainingLength = distance - (totalLength - targetLength);
                    break;
                }
                targetIndex++;
            }

            float percent = remainingLength / targetLength;
            //Debug.Log($"Percent : {percent}, target index : {targetIndex}");

            if (targetIndex == _railPoints.Count - 1)
            {
                return Vector3.Lerp(_railPoints[_railPoints.Count - 2].position, _railPoints[_railPoints.Count - 1].position, 1f);
            }

            return Vector3.Lerp(_railPoints[targetIndex].position, _railPoints[targetIndex + 1].position, percent);
        } 
    }

    public List<Transform> GetRailPoints()
    {
        return _railPoints;
    }


    private void OnDrawGizmos()
    {
        GetChildrenAndLength();

        DrawGizmos(Color.magenta);
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;

        for (int i = 0; i < _railPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(_railPoints[i + 1].position, _railPoints[i].position);
        }

        if (IsLoop && _railPoints.Count > 0 && _railPoints != null)
        {
            Gizmos.DrawLine(_railPoints[_railPoints.Count - 1].position, _railPoints[0].position);
        }
    }
}
