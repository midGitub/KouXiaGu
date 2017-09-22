using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace JiongXiaGu.Terrain3D
{

    [CustomEditor(typeof(LandformChunk))]
    [CanEditMultipleObjects]
    class TerrainChunkEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var target = (LandformChunk)this.target;

            var texture = target.Renderer;

            if (texture != null)
            {
                var diffuse = EditorGUILayout.ObjectField("DiffuseMap", texture.DiffuseMap, typeof(Texture2D), false) as Texture2D;
                texture.SetDiffuseMap(diffuse);

                var height = EditorGUILayout.ObjectField("HeightMap", texture.HeightMap, typeof(Texture2D), false) as Texture2D;
                texture.SetHeightMap(height);

                var roadDiffuse = EditorGUILayout.ObjectField("RoadDiffuseMap", texture.RoadDiffuseMap, typeof(Texture2D), false) as Texture2D;
                texture.SetRoadDiffuseMap(roadDiffuse);

                var normalMap = EditorGUILayout.ObjectField("HeightMap", texture.NormalMap, typeof(Texture2D), false) as Texture2D;
                texture.SetNormalMap(normalMap);
            }

        }

    }

}
