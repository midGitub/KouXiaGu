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
            newPoints.AddRange(CatmullRom.GetFullPath(points, segmentPoints));

            for (int i = 0; i < newPoints.Count - 1; i++)
            {
                Handles.color = Color.red;
                Handles.DrawLine(newPoints[i], newPoints[i + 1]);
                Handles.color = Color.white;
                Handles.SphereCap(1, newPoints[i], Quaternion.identity, 0.08f);

                //Vector3 left, right;
                //GetPoint(newPoints[i], newPoints[i + 1], out left, out right);

                //Handles.color = Color.yellow;
                //Handles.SphereCap(1, left, Quaternion.identity, 0.1f);
                //Handles.color = Color.blue;
                //Handles.SphereCap(1, right, Quaternion.identity, 0.1f);
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

        void GetPoint(Vector3 p1, Vector3 p2, out Vector3 left, out Vector3 right)
        {
            double angle = AngleY(p1, p2);
            left = Circle(0.1f, -90 * (Math.PI / 180) + angle) + p1;
            right = Circle(0.1f, 90 * (Math.PI / 180) + angle) + p1;
        }



        Vector3 GetPoint(Vector3 p1, Vector3 p2, float width)
        {
            double angle = AngleY(p1, p2);
            angle = -90 * (Math.PI / 180) + angle;
            return Circle(width, angle);
        }

        public static Vector3 Circle(float radius, double angle)
        {
            double x = Math.Sin(angle) * radius;
            double y = Math.Cos(angle) * radius;

            return new Vector3((float)x, 0, (float)y);
        }

        /// <summary>
        /// 获取到两个点的角度(忽略Y),单位弧度;原型:Math.Atan2()
        /// </summary>
        public static double AngleY(Vector3 from, Vector3 to)
        {
            double angle = (Math.Atan2((to.x - from.x), (to.z - from.z)));
            return angle;
        }

    }

}
