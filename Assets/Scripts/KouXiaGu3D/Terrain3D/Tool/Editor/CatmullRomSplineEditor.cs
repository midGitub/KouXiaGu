using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 用于演示曲线;
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CatmullRomSpline))]
    public class CatmullRomSplineEditor : Editor
    {

        CatmullRomSpline catmullRomSpline;
        Transform handleTransform;
        Quaternion handleRotation;

        Vector3[] points;
        List<Vector3> newPoints;

        int segmentPoints
        {
            get { return catmullRomSpline.SegmentPoints; }
        }

        private void OnEnable()
        {
            newPoints = new List<Vector3>();
        }

        void OnSceneGUI()
        {
            catmullRomSpline = (CatmullRomSpline)this.target;
            points = catmullRomSpline.Points;
            handleTransform = catmullRomSpline.transform;
            handleRotation = Tools.pivotRotation == PivotRotation.Local ?
                handleTransform.rotation : Quaternion.identity;

            for (int i = 0; i < points.Length; i++)
            {
                ShowPoint(i);
            }

            newPoints.Clear();
            newPoints.AddRange(CatmullRom.GetPath(points, segmentPoints));

            for (int i = 1; i < newPoints.Count; i++)
            {
                Handles.color = Color.red;
                Handles.DrawLine(newPoints[i - 1], newPoints[i]);
                Handles.color = Color.white;
                Handles.SphereCap(1, newPoints[i], Quaternion.identity, 0.1f);
            }
            catmullRomSpline.NewPoints = newPoints;
        }

        Vector3 ShowPoint(int index)
        {
            Vector3 point = handleTransform.TransformPoint(points[index]);
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(catmullRomSpline, "Move Point");
                EditorUtility.SetDirty(catmullRomSpline);
                points[index] = handleTransform.InverseTransformPoint(point);
            }
            return point;
        }

    }

}
