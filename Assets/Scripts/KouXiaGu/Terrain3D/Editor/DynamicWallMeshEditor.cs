using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using KouXiaGu.Terrain3D.Wall;

namespace KouXiaGu.Terrain3D
{

    [CustomEditor(typeof(DynamicWallMesh))]
    [CanEditMultipleObjects]
    class DynamicWallMeshEditor : Editor
    {

        public float disPlayPointSize = 0.01f;
        public bool isDisplayCurrentVertices = true;
        MeshFilter meshFilter;

        DynamicWallMesh Target
        {
            get { return (DynamicWallMesh)this.target; }
        }

        void OnEnable()
        {
            meshFilter = Target.GetComponent<MeshFilter>();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            isDisplayCurrentVertices = EditorGUILayout.Toggle("IsDisplayCurrentVertices", isDisplayCurrentVertices);
            disPlayPointSize = EditorGUILayout.FloatField("DisPlayPointSize", disPlayPointSize);
            EditorGUILayout.LabelField("VerticeCount:" + Target.WallInfo.Points.Count);
        }

        void OnSceneGUI()
        {
            Transform handleTransform = Target.transform;
            Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
                handleTransform.rotation : Quaternion.identity;

            if (isDisplayCurrentVertices)
            {
                DisplayCurrentVertices();
            }
            else
            {
                DisPlayDynamicWall();
            }

            DisPlaySection(handleTransform, handleRotation);
        }

        void DisPlayDynamicWall()
        {
            IList<JointPoint> sections = Target.WallInfo.JointInfo.JointPoints;
            IList<LocalVertice> points = Target.WallInfo.Points;

            foreach (var section in sections)
            {
                Handles.color = RandomColor.Get((int)(section.InterpolatedValue * 100), 1);
                SceneGUISphere(section.Position);
                foreach (var childIndex in section.Children)
                {
                    Vector3 locaPosition = points[childIndex].LocalPosition;
                    Vector3 position = locaPosition + section.Position;
                    SceneGUISphere(position);
                }
            }
        }

        Vector3 SceneGUISphere(Vector3 localPosition)
        {
            Vector3 position = Target.transform.TransformPoint(localPosition);
            Handles.SphereHandleCap(1, position, Quaternion.identity, disPlayPointSize, EventType.Repaint);
            return position;
        }

        void DisplayCurrentVertices()
        {
            IList<JointPoint> sections = Target.WallInfo.JointInfo.JointPoints;
            IList<Vector3> points = meshFilter.sharedMesh.vertices;

            foreach (var section in sections)
            {
                Handles.color = RandomColor.Get((int)(section.InterpolatedValue * 100), 1);
                SceneGUISphere(section.Position);
                foreach (var childIndex in section.Children)
                {
                    Vector3 position = points[childIndex];
                    SceneGUISphere(position);
                }
            }
        }

        void DisPlaySection(Transform handleTransform, Quaternion handleRotation)
        {
            IList<JointPoint> sections = Target.WallInfo.JointInfo.JointPoints;
            for (int i = 0; i < sections.Count; i++)
            {
                Vector3 point = handleTransform.TransformPoint(sections[i].Position);
                EditorGUI.BeginChangeCheck();
                point = Handles.DoPositionHandle(point, handleRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(Target, "Move Point");
                    EditorUtility.SetDirty(Target);
                    var result = handleTransform.InverseTransformPoint(point);
                    Target.ChangeSection(i, result);
                }
            }
        }
    }

}
