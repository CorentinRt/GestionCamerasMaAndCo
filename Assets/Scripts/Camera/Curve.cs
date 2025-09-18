using Unity.VisualScripting;
using UnityEngine;

public class Curve : MonoBehaviour
{
    // ----- FIELDS ----- //
    public Vector3 A, B, C, D;
    // ----- FIELDS ----- //

    public Vector3 GetPosition(float t)
    {
        return MathUtils.CubicBezier(A, B, C, D, t);
    }

    public Vector3 GetPosition(float t, Matrix4x4 localToWorldMatrix)
    {
        return localToWorldMatrix.MultiplyPoint(GetPosition(t));
    }

    private void OnDrawGizmos()
    {
        DrawGizmos(Color.magenta, transform.localToWorldMatrix);
    }

    private void DrawGizmos(Color color, Matrix4x4 localToWorldMatrix)
    {
        // Points :
        Vector3 aW = localToWorldMatrix.MultiplyPoint(A);
        Vector3 bW = localToWorldMatrix.MultiplyPoint(B);
        Vector3 cW = localToWorldMatrix.MultiplyPoint(C);
        Vector3 dW = localToWorldMatrix.MultiplyPoint(D);

        Gizmos.color = color;
        Gizmos.DrawSphere(aW, 0.05f);
        Gizmos.DrawSphere(bW, 0.05f);
        Gizmos.DrawSphere(cW, 0.05f);
        Gizmos.DrawSphere(dW, 0.05f);

        // Courbe :
        const int nbrLines = 32;
        Vector3 previousPoint = localToWorldMatrix.MultiplyPoint(A);
        for (int i = 1; i <= nbrLines; i++)
        {
            float t = i / (float)nbrLines; // Percent
            Vector3 currentPoint = localToWorldMatrix.MultiplyPoint(GetPosition(t));
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }
}
