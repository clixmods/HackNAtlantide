using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class BezierCurveGenerate : MonoBehaviour
{
    [SerializeField]  Transform[] controlPoint;
    public Vector3  GetPosPoint(int i) { return controlPoint[i].position; }
    [Range(0, 1)]
    [SerializeField] float t;

    public Vector3 GetPointBezierCurve(float t)
    {
        Vector3 p0 = GetPosPoint(0);
        Vector3 p1 = GetPosPoint(1);
        Vector3 p2 = GetPosPoint(2);
        Vector3 p3 = GetPosPoint(3);

        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);
        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(d, e, t);
    }

    public Quaternion GetBezierOrientation(float t)
    {
        Vector3 p0 = GetPosPoint(0);
        Vector3 p1 = GetPosPoint(1);
        Vector3 p2 = GetPosPoint(2);
        Vector3 p3 = GetPosPoint(3);

        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);
        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);
        Vector3 tangent = (e - d).normalized;
        

        return Quaternion.LookRotation(tangent); ;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawSphere(GetPosPoint(i), 0.02f);
        }
        Handles.DrawBezier(GetPosPoint(0), GetPosPoint(3), GetPosPoint(1), GetPosPoint(2), Color.blue, EditorGUIUtility.whiteTexture, 1);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetPointBezierCurve(t), 0.1f);
        Gizmos.color = Color.white;
    }
#endif
}
