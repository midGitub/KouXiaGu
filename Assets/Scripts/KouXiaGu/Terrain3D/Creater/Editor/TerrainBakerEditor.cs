using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    [CustomEditor(typeof(Baker))]
    class TerrainBakerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        void OnSceneGUI()
        {
            //var par = new BakingParameter(1, 1, 1);
            //Rect rect = par.DiffuseReadPixel;

            //Handles.color = Color.red;
            //DrawRect(par.DiffuseReadPixel);

            ////Handles.color = Color.blue;
            ////DrawRect(Rect.MinMaxRect(0, 0, TerrainChunk.CHUNK_WIDTH, TerrainChunk.CHUNK_HEIGHT));

            //Handles.color = Color.green;
            //DrawRect(Rect.MinMaxRect(0, 0, par.rDiffuseTexWidth, par.rDiffuseTexHeight));
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
