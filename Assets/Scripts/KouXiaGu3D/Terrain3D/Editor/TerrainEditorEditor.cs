using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using KouXiaGu.Initialization;
using UnityEditor;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [CustomEditor(typeof(TerrainEditor), true)]
    public class TerrainEditorEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var edit = (TerrainEditor)target;

            if (GUILayout.Button("创建新地图"))
            {
                TerrainEditor.CreateMap(edit.description);
            }
        }

    }

}
