﻿using System;
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

        bool isEditMode;
        float displayPointSize = 0.01f;
        float spacing = 0.1f;
        bool isEditJointPoint;
        MeshFilter meshFilter;
        DynamicWallMesh instance;

        void Awake()
        {
            instance = (DynamicWallMesh)this.target;
            meshFilter = instance.GetComponent<MeshFilter>();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (isEditMode = EditorGUILayout.BeginToggleGroup("IsEditMode", isEditMode))
            {
                spacing = EditorGUILayout.FloatField("Spacing", spacing);
                if (GUILayout.Button("Initialize"))
                {
                    instance.Build(spacing);
                }

                displayPointSize = EditorGUILayout.FloatField("DisPlayPointSize", displayPointSize);
                EditorGUILayout.LabelField("VerticeCount:" + instance.WallInfo.Points.Count);
            }
            EditorGUILayout.EndToggleGroup();
        }

        void OnSceneGUI()
        {
            Transform handleTransform = instance.transform;
            Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
                handleTransform.rotation : Quaternion.identity;

            if (isEditMode)
            {
                DisplayVertices(handleTransform, handleRotation);
            }
        }

        void SceneGUISphere(Vector3 localPosition)
        {
            Handles.SphereHandleCap(1, localPosition, Quaternion.identity, displayPointSize, EventType.Repaint);
        }

        void DisplayVertices(Transform handleTransform, Quaternion handleRotation)
        {
            IList<JointPoint> sections = instance.WallInfo.JointInfo.JointPoints;
            Vector3[] vertices = meshFilter.sharedMesh.vertices;

            foreach (var section in sections)
            {
                Handles.color = RandomColor.Get((int)(section.InterpolatedValue * 100), 1);
                foreach (var childIndex in section.Children)
                {
                    Vector3 point = handleTransform.TransformPoint(vertices[childIndex]);
                    SceneGUISphere(point);
                }
            }
        }

        //void DisplaySection(Transform handleTransform, Quaternion handleRotation)
        //{
        //    IList<JointPoint> sections = Target.WallInfo.JointInfo.JointPoints;
        //    for (int i = 0; i < sections.Count; i++)
        //    {
        //        Vector3 point = handleTransform.TransformPoint(sections[i].Position);
        //        EditorGUI.BeginChangeCheck();
        //        point = Handles.DoPositionHandle(point, handleRotation);
        //        if (EditorGUI.EndChangeCheck())
        //        {
        //            Undo.RecordObject(Target, "Move Point");
        //            EditorUtility.SetDirty(Target);
        //            var result = handleTransform.InverseTransformPoint(point);
        //        }
        //    }
        //}
    }

}
