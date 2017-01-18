using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    [CustomEditor(typeof(KouXiaGu.Terrain3D.Renderer))]
    class RendererEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        void OnSceneGUI()
        {
            var par = new BakingParameter(1, 0, 0);
            Rect rect = par.DiffuseReadPixel;
            Rect rect2 = Rect.MinMaxRect(0, 0, TerrainMesh.CHUNK_WIDTH, TerrainMesh.CHUNK_HEIGHT);

            Handles.color = Color.red;
            DrawRect(par.DiffuseReadPixel);

            Handles.color = Color.blue;
            DrawRect(rect2);
        }

        void DrawRect(Rect rect)
        {
            rect.center = Vector2.zero;
            Handles.DrawLine(rect.Northwest(), rect.Northeast());
            Handles.DrawLine(rect.Northeast(), rect.Southeast());
            Handles.DrawLine(rect.Southeast(), rect.Southwest());
            Handles.DrawLine(rect.Southwest(), rect.Northwest());
        }



    }

}
