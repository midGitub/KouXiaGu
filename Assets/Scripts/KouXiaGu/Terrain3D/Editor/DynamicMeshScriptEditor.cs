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
                if (isInitialized)
                {
                    displayPointSize = EditorGUILayout.FloatField("DisPlayPointSize", displayPointSize);
                    EditorGUILayout.LabelField("VerticeCount:" + instance.MeshData.Points.Length);
                    isEditJointPoint = EditorGUILayout.Toggle("IsEditJointPoint", isEditJointPoint);
                }

                spacing = EditorGUILayout.FloatField("Spacing", spacing);

                if (GUILayout.Button("AutoBuild"))
                {
                    instance.AutoBuild(spacing);
                }
                if (GUILayout.Button("Save..."))
                {
                    instance.Save();
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
                    //Handles.Label(point, Math.Round(section.InterpolatedValue, 3).ToString());
                    //Handles.Label(point, (point).ToString());
                }
            }
        }

        void DisplaySection(Transform handleTransform, Quaternion handleRotation)
        {
            IList<JointPoint> sections = instance.MeshData.JointInfo.JointPoints;
            Vector3[] vertices = meshFilter.sharedMesh.vertices;

            for (int i = 0; i < sections.Count; i++)
            {
                JointPoint joint = sections[i];
                Vector3 point = handleTransform.TransformPoint(joint.Position);
                EditorGUI.BeginChangeCheck();
                point = Handles.DoPositionHandle(point, handleRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(instance, "Move Point");
                    EditorUtility.SetDirty(instance);
                    Vector3 result = handleTransform.InverseTransformPoint(point);
                }
            }
        }
    }

}
