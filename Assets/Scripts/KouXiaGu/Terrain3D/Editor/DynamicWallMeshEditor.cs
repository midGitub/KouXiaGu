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

        public float disPlayPointSize = 0.01f;


        DynamicWallMesh Target
        {
            get { return (DynamicWallMesh)this.target; }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            disPlayPointSize = EditorGUILayout.FloatField("DisPlayPointSize", disPlayPointSize);
            EditorGUILayout.LabelField("VerticeCount:" + Target.DynamicWall.VerticeCount);
        }

        void OnSceneGUI()
        {
            DisPlayDynamicWall(Target.DynamicWall);
        }

        void DisPlayDynamicWall(DynamicWallSectionInfo item)
        {
            Handles.color = Color.red;
            foreach (var vertice in item.GetOriginalVertices())
            {
                Vector3 newVertice = Target.transform.TransformPoint(vertice);
                Handles.SphereHandleCap(1, newVertice, Quaternion.identity, disPlayPointSize, EventType.Repaint);
            }

            Handles.color = Color.blue;
            foreach (var section in item.SectionList)
            {
                Vector3 nodePos = Target.transform.TransformPoint(section.Position);
                Handles.SphereHandleCap(1, nodePos, Quaternion.identity, disPlayPointSize, EventType.Repaint);
            }
        }

    }

}
