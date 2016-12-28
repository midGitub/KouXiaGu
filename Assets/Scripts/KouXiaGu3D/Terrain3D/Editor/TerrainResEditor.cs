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

            GUILayout.BeginVertical();
            GUILayout.Label("拓展:");

            if (GUILayout.Button("导出缺失模板"))
            {
                OutputTemplet();
            }

            GUILayout.EndVertical();
        }

        public static void OutputTemplet()
        {
            TerrainResPath.Create();

            if(!File.Exists(TerrainRes.RoadDescrFile))
                RoadDescr.ArraySerializer.SerializeXiaGu(TerrainRes.RoadDescrFile, RoadTemplets);

            if (!File.Exists(TerrainRes.LandformDescrFile))
                LandformDescr.ArraySerializer.SerializeXiaGu(TerrainRes.LandformDescrFile, LandformTemplets);
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
