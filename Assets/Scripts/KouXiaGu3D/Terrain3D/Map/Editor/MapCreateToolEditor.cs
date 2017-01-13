using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [CustomEditor(typeof(MapCreateTool), true)]
    class MapCreateToolEditor : Editor
    {
        protected MapCreateToolEditor() { }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (TerrainInitializer.Map != null)
            {
                EditorGUILayout.LabelField("地图容量:", TerrainInitializer.Map.Count.ToString());
                EditorGUILayout.LabelField("归档容量:", MapArchiver.Map.Count.ToString());


                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("保存预制"))
                {
                    MapFiler.Write(TerrainInitializer.Map);
                    MapArchiver.Map.Clear();
                }
                if (GUILayout.Button("覆盖随机地图"))
                {
                    MapCreateTool.Fill(TerrainInitializer.Map);
                }
                EditorGUILayout.EndHorizontal();
            }




        }

    }

}
