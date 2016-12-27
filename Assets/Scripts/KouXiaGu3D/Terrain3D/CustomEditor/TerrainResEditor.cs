
#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.IO;

namespace KouXiaGu.Terrain3D
{

    [CustomEditor(typeof(TerrainRes), true)]
    public class TerrainResEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("导出模板"))
            {
                OutputTemplet();
            }

        }

        public static void OutputTemplet()
        {
            TerrainResPath.Create();
            RoadDescr.ArraySerializer.SerializeFile(TerrainRes.RoadDescrFile, RoadTemplets);
            LandformDescr.ArraySerializer.SerializeFile(TerrainRes.LandformDescrFile, LandformTemplets);
        }


        static RoadDescr[] RoadTemplets = new RoadDescr[]
            {
                RoadTemplet(),
                RoadTemplet(),
                RoadTemplet(),
            };

        static RoadDescr RoadTemplet()
        {
            var descr = new RoadDescr()
            {
                ID = 0,
                Name = "defalut",
                HeightAdjustTex = "texture",
                HeightAdjustBlendTex = "texture",
                DiffuseTex = "texture",
                DiffuseBlendTex = "texture",
            };
            return descr;
        }


        static LandformDescr[] LandformTemplets = new LandformDescr[]
            {
                LandformTemplet(),
                LandformTemplet(),
                LandformTemplet(),
            };

        static LandformDescr LandformTemplet()
        {
            var descr = new LandformDescr()
            {
                ID = 0,
                Name = "defalut",
                HeightTex = "texture",
                HeightBlendTex = "texture",
                DiffuseTex = "texture",
                DiffuseBlendTex = "texture",
            };
            return descr;
        }

    }

}

#endif