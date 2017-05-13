using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace KouXiaGu.Terrain3D
{

    [CustomEditor(typeof(DynamicWallMesh))]
    [CanEditMultipleObjects]
    class DynamicWallMeshEditor : Editor
    {

        float disPlayPointSize = 0.01f;
        bool isDisplayCurrentVertices = true;
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
            EditorGUILayout.LabelField("VerticeCount:" + Target.DynamicWall.VerticeCount);
        }

        void OnSceneGUI()
        {
            if (isDisplayCurrentVertices)
            {
                DisplayCurrentVertices();
            }
            else
            {
                DisPlayDynamicWall();
            }
        }

        void DisPlayDynamicWall()
        {
            Handles.color = Color.red;
            foreach (var vertice in Target.DynamicWall.GetVertices())
            {
                SceneGUISphere(vertice);
                //Vector3 newVertice = Target.transform.TransformPoint(vertice);
                //Handles.SphereHandleCap(1, newVertice, Quaternion.identity, disPlayPointSize, EventType.Repaint);
            }

            Handles.color = Color.blue;
            foreach (var section in Target.DynamicWall.SectionPoint)
            {
                SceneGUISphere(section);
                //Vector3 nodePos = Target.transform.TransformPoint(section);
                //Handles.SphereHandleCap(1, nodePos, Quaternion.identity, disPlayPointSize, EventType.Repaint);
            }
        }

        void SceneGUISphere(Vector3 localPosition)
        {
            Vector3 position = Target.transform.TransformPoint(localPosition);
            Handles.SphereHandleCap(1, position, Quaternion.identity, disPlayPointSize, EventType.Repaint);
        }

        void DisplayCurrentVertices()
        {
            Handles.color = Color.red;
            Mesh mesh = meshFilter.sharedMesh;

            foreach (var vertice in mesh.vertices)
            {
                SceneGUISphere(vertice);
            }
        }

    }

}
