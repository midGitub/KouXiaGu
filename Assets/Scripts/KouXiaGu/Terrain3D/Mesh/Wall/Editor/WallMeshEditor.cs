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

        WallMesh instance
        {
            get { return (WallMesh)target; }
        }

        void OnEnable()
        {
            meshFilter = instance.GetComponent<MeshFilter>();
        }

        void OnSceneGUI()
        {
            Mesh mesh = meshFilter.sharedMesh;

            foreach (var pos in mesh.vertices)
            {
                Vector3 newPos = instance.transform.TransformPoint(pos);
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

        }

    }

}
