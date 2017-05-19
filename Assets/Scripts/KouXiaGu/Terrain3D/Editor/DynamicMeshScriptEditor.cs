using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace KouXiaGu.Terrain3D.Wall
{

    [CustomEditor(typeof(DynamicMeshScript))]
    [CanEditMultipleObjects]
    class DynamicMeshScriptEditor : Editor
    {

        bool isEditMode;
        float displayPointSize = 0.01f;
        float spacing = 0.1f;
        bool isEditJointPoint;
        MeshFilter meshFilter;
        DynamicMeshScript instance;

        bool isInitialized
        {
            get { return instance.MeshData != null; }
        }

        void Awake()
        {
            instance = (DynamicMeshScript)this.target;
            meshFilter = instance.GetComponent<MeshFilter>();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (isEditMode = EditorGUILayout.BeginToggleGroup("IsEditMode", isEditMode))
            {
                spacing = EditorGUILayout.FloatField("Spacing", spacing);
                if (GUILayout.Button("InitializeOrUpdate"))
                {
                    instance.Build(spacing);
                }

                if (isInitialized)
                {
                    displayPointSize = EditorGUILayout.FloatField("DisPlayPointSize", displayPointSize);
                    EditorGUILayout.LabelField("VerticeCount:" + instance.MeshData.Points.Count);
                }
            }
            EditorGUILayout.EndToggleGroup();
        }

        void OnSceneGUI()
        {
            if (isEditMode && isInitialized)
            {
                Transform handleTransform = instance.transform;
                Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
                    handleTransform.rotation : Quaternion.identity;

                DisplayVertices(handleTransform, handleRotation);
            }
        }

        void SceneGUISphere(Vector3 localPosition)
        {
            Handles.SphereHandleCap(1, localPosition, Quaternion.identity, displayPointSize, EventType.Repaint);
        }

        void DisplayVertices(Transform handleTransform, Quaternion handleRotation)
        {
            IList<JointPoint> sections = instance.MeshData.JointInfo.JointPoints;
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
