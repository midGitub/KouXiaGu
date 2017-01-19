using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace KouXiaGu.Terrain3D
{


    [CustomEditor(typeof(TerrainRenderer), true)]
    class TerrainChunkEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var target = (TerrainRenderer)this.target;

            EditorGUILayout.ObjectField("Diffuse", target.DiffuseTexture, typeof(Texture2D), true);
            EditorGUILayout.ObjectField("Height", target.HeightTexture, typeof(Texture2D), true);
            EditorGUILayout.ObjectField("Normal", target.NormalMap, typeof(Texture2D), true);

            if (GUILayout.Button("存储所有贴图"))
            {
                string path = Application.dataPath + "\\TestTex";

                target.DiffuseTexture.SavePNG(path, target.transform.position.ToString() + "_d", FileMode.CreateNew);
                target.HeightTexture.SavePNG(path, target.transform.position.ToString() + "_h", FileMode.CreateNew);
                target.NormalMap.SavePNG(path, target.transform.position.ToString() + "_n", FileMode.CreateNew);
            }
        }

    }

}
