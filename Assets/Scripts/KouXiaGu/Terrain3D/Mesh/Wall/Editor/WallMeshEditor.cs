using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [CanEditMultipleObjects, CustomEditor(typeof(WallMesh))]
    public class WallMeshEditor : Editor
    {

        MeshFilter meshFilter;

        List<PointObject> pointObjects;


        WallMesh instance
        {
            get { return (WallMesh)target; }
        }

        Transform transform
        {
            get { return instance.transform; }
        }

        Transform pointsParent
        {
            get { return transform; }
        }


        void OnEnable()
        {
            meshFilter = instance.GetComponent<MeshFilter>();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("开始编辑"))
            {
                StartEdit();
            }
            if (GUILayout.Button("完成编辑"))
            {

            }

            EditorGUILayout.EndHorizontal();
        }


        void StartEdit()
        {
            IList<Vector3> points = meshFilter.sharedMesh.vertices;
            CreatePointObject(points);
        }

        void CreatePointObject(IList<Vector3> points)
        {
            if(pointObjects.Count == 0 && points.Count > 0)
            {
                for (int i = 0; i < points.Count; i++)
                {
                   PointObject pointObject = CreatePointObject(i, points[i]);
                   pointObjects.Add(pointObject);
                }
            }
        }

        PointObject CreatePointObject(int id, Vector3 pos)
        {
            GameObject point = new GameObject(id.ToString(), typeof(PointObject));
            point.transform.SetParent(pointsParent);
            point.transform.position = pos;

            if (pointObjects == null)
                pointObjects = new List<PointObject>();

            PointObject pointObject = point.GetComponent<PointObject>();
            return pointObject;
        }


        void OnSceneGUI()
        {
            Mesh mesh = meshFilter.sharedMesh;

            foreach (var pos in mesh.vertices)
            {
                Vector3 newPos = instance.transform.TransformPoint(pos);
                //Handles.SphereCap(1, newPos, Quaternion.identity, UnityEngine.Random.Range(0.02f, 0.04f));
                Handles.SphereCap(1, newPos, Quaternion.identity, 0.02f);
            }


            Event e = Event.current;

            if (e.isKey || e.isMouse)
            {
                if (e.keyCode == KeyCode.Mouse0)
                {
                    Debug.Log(e.mousePosition);
                }
                if (e.keyCode == KeyCode.A)
                {
                    Debug.Log(e.mousePosition);
                }
            }

            //Selection.gameObjects

        }

    }

}
