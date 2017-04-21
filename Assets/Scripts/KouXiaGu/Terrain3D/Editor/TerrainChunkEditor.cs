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

            var texture = target.Texture;

            if (texture != null)
            {
                var diffuse = EditorGUILayout.ObjectField("DiffuseMap", texture.DiffuseMap, typeof(Texture2D), false) as Texture2D;
                texture.SetDiffuseMap(diffuse);

                var height = EditorGUILayout.ObjectField("HeightMap", texture.HeightMap, typeof(Texture2D), false) as Texture2D;
                texture.SetHeightMap(height);

                var normalMap = EditorGUILayout.ObjectField("HeightMap", texture.NormalMap, typeof(Texture2D), false) as Texture2D;
                texture.SetNormalMap(normalMap);
            }

        }

    }

}
