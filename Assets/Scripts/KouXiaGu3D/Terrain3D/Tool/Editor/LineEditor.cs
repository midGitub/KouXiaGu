using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [CustomEditor(typeof(Line))]
    public class LineEditor : Editor
    {

        Line line;
        Transform handleTransform;
        Quaternion handleRotation;

        Vector3[] points
        {
            get { return line.points; }
        }

        void OnSceneGUI()
        {
            line = target as Line;
            handleTransform = line.transform;
            handleRotation = Tools.pivotRotation == PivotRotation.Local ?
                handleTransform.rotation : Quaternion.identity;

            Vector3 p0 = default(Vector3);
            Handles.color = Color.white;

            for (int i = 0; i < points.Length; i++)
            {
                Vector3 p1 = ShowPoint(i);
                if (i != 0)
                {
                    Handles.DrawLine(p0, p1);
                }
                p0 = p1;
            }
        }

        Vector3 ShowPoint(int index)
        {
            Vector3 point = handleTransform.TransformPoint(points[index]);
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(line, "Move Point");
                EditorUtility.SetDirty(line);
                points[index] = handleTransform.InverseTransformPoint(point);
            }
            return point;
        }

    }

}
