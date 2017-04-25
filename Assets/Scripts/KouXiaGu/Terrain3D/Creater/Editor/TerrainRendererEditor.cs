//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEditor;
//using UnityEngine;
//using System.IO;

//namespace KouXiaGu.Terrain3D
//{


//    [CustomEditor(typeof(OLandformRenderer), true)]
//    class TerrainRendererEditor : Editor
//    {

//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();

//            var target = (OLandformRenderer)this.target;

//            if (Application.isPlaying)
//            {
//                if (GUILayout.Button("存储所有贴图"))
//                {
//                    string path = Application.dataPath + "\\TestTex";

//                    var name = DateTime.Now.Ticks;

//                    if (target.DiffuseMap != null)
//                        target.DiffuseMap.SavePNG(path, name + "_d", FileMode.CreateNew);

//                    if (target.HeightMap != null)
//                        target.HeightMap.SavePNG(path, name + "_h", FileMode.CreateNew);

//                    if (target.NormalMap != null)
//                        target.NormalMap.SavePNG(path, name + "_n", FileMode.CreateNew);
//                }
//            }
//        }

//    }

//}
