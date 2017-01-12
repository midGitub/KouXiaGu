using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [CustomEditor(typeof(BezierCurve))]
    [CanEditMultipleObjects]
    public class BezierCurveEditor : Editor
    {
        const int lineSteps = 10;

        BezierCurve curve;
        Transform handleTransform;
        Quaternion handleRotation;

        Vector3[] points
        {
            get { return curve.points; }
        }

        private void OnSceneGUI()
        {
            curve = target as BezierCurve;
            handleTransform = curve.transform;
            handleRotation = Tools.pivotRotation == PivotRotation.Local ?
                handleTransform.rotation : Quaternion.identity;

            Vector3 p0 = ShowPoint(0);
            Vector3 p1 = ShowPoint(1);
            Vector3 p2 = ShowPoint(2);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p1, p2);

            Vector3 lineStart = GetPoint(0f);
            Handles.color = Color.green;
            Handles.DrawLine(lineStart, lineStart + GetDirection(0f));
            for (int i = 1; i <= lineSteps; i++)
            {
                Vector3 lineEnd = GetPoint(i / (float)lineSteps);
                Handles.color = Color.white;
                Handles.DrawLine(lineStart, lineEnd);
                Handles.color = Color.green;
                Handles.DrawLine(lineEnd, lineEnd + GetDirection(i / (float)lineSteps));
                lineStart = lineEnd;
            }
        }

        private Vector3 ShowPoint(int index)
        {
            Vector3 point = handleTransform.TransformPoint(points[index]);
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(curve, "Move Point");
                EditorUtility.SetDirty(curve);
                points[index] = handleTransform.InverseTransformPoint(point);
            }
            return point;
        }


        public Vector3 GetPoint(float t)
        {
            return handleTransform.TransformPoint(GetPoint(points[0], points[1], points[2], t));
        }

        public Vector3 GetVelocity(float t)
        {
            return handleTransform.TransformPoint(GetFirstDerivative(points[0], points[1], points[2], t)) -
                handleTransform.position;
        }

        public Vector3 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }


        public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                oneMinusT * oneMinusT * p0 +
                2f * oneMinusT * t * p1 +
                t * t * p2;
        }

        public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            return
                2f * (1f - t) * (p1 - p0) +
                2f * t * (p2 - p1);
        }


        //public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        //{
        //    t = Mathf.Clamp01(t);
        //    float oneMinusT = 1f - t;
        //    return
        //        oneMinusT * oneMinusT * oneMinusT * p0 +
        //        3f * oneMinusT * oneMinusT * t * p1 +
        //        3f * oneMinusT * t * t * p2 +
        //        t * t * t * p3;
        //}

        //public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        //{
        //    t = Mathf.Clamp01(t);
        //    float oneMinusT = 1f - t;
        //    return
        //        3f * oneMinusT * oneMinusT * (p1 - p0) +
        //        6f * oneMinusT * t * (p2 - p1) +
        //        3f * t * t * (p3 - p2);
        //}

    }

}
