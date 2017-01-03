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

        int landform;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var edit = (TerrainEditor)target;

            if (TerrainController.CurrentMap != null)
            {
                EditorGUILayout.IntField("地图容量:", TerrainController.CurrentMap.Map.Count);
                EditorGUILayout.IntField("归档容量:", MapArchiver.ArchiveMap.Count);
            }
            else
                EditorGUILayout.IntField("地图容量:", 0);

            if (GUILayout.Button("创建新地图"))
            {
                TerrainEditor.CreateMap(edit.description);
            }

            if (GUILayout.Button("保存预制"))
            {
                TerrainController.CurrentMap.Save();
                MapArchiver.ArchiveMap.Clear();
            }

            if (GUILayout.Button("随机地图"))
            {
                TerrainEditor.RandomMap(edit.randomMapSize);
            }

            landform = EditorGUILayout.IntField("平铺地形:", landform);

            if (GUILayout.Button("随机地形地图"))
            {
                TerrainEditor.RandomMap(edit.randomMapSize, ((TerrainEditor)target).landforms);
            }

        }

    }

}
