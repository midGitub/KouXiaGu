using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [CustomEditor(typeof(Chunk))]
    [CanEditMultipleObjects]
    class TerrainChunkEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var target = (Chunk)this.target;

            var texture = target.Renderer;

            if (texture != null)
            {
                var diffuse = EditorGUILayout.ObjectField("DiffuseMap", texture.DiffuseMap, typeof(Texture2D), false) as Texture2D;
                texture.SetDiffuseMap(diffuse);

                var height = EditorGUILayout.ObjectField("HeightMap", texture.HeightMap, typeof(Texture2D), false) as Texture2D;
                texture.SetHeightMap(height);

                var roadDiffuse = EditorGUILayout.ObjectField("RoadDiffuse", texture.RoadDiffuseMap, typeof(Texture2D), false) as Texture2D;
                texture.SetRoadDiffuseMap(roadDiffuse);

                var roadHeight = EditorGUILayout.ObjectField("RoadHeight", texture.RoadHeightMap, typeof(Texture2D), false) as Texture2D;
                texture.SetRoadHeightMap(roadHeight);

                var normalMap = EditorGUILayout.ObjectField("HeightMap", texture.NormalMap, typeof(Texture2D), false) as Texture2D;
                texture.SetNormalMap(normalMap);
            }

        }

    }

}
