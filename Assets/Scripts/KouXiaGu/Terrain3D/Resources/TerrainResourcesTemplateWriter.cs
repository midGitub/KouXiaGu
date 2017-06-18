
#if UNITY_EDITOR

using System.Collections.Generic;
using KouXiaGu.Resources;
using System.IO;
using UnityEditor;
using KouXiaGu.World.Resources;

namespace KouXiaGu.Terrain3D
{

    public class TerrainResourcesTemplateWriter
    {

        static readonly Dictionary<int, LandformInfo> templateLandformInfos = new Dictionary<int, LandformInfo>()
        {
            { 1, new LandformInfo()
            {
                ID = 1,
                Tags = "none",
                TerrainInfo = new TerrainLandformInfo()
                {
                    DiffuseBlendTex = "none",
                    DiffuseTex = "none",
                    HeightBlendTex = "none",
                    HeightTex = "none",
                },
            } },
        };

        static readonly Dictionary<int, BuildingInfo> templateBuildingInfos = new Dictionary<int, BuildingInfo>()
        {
            { 1, new BuildingInfo()
            {
                ID = 1,
                Tags = "none",
                TerrainInfo = new TerrainBuildingInfo()
                {
                    PrefabName = "none",
                },
            } },
        };

        static readonly Dictionary<int, RoadInfo> templateRoadInfos = new Dictionary<int, RoadInfo>()
        {
            { 1, new RoadInfo()
            {
                ID = 1,
                TerrainInfo = new TerrainRoadInfo()
                {
                    DiffuseBlendTex = "none",
                    DiffuseTex = "none",
                },
            } },
        };

        static readonly string[] templateTags = new string[]
            {
                "Unknown1",
                "Unknown2",
            };

        static IWriter<Dictionary<int, LandformInfo>> LandformSerializer = new LandformInfoXmlSerializer().ToTemplateWriter();
        static IWriter<Dictionary<int, BuildingInfo>> BuildingSerializer = new BuildingInfoXmlSerializer().ToTemplateWriter();
        static IWriter<Dictionary<int, RoadInfo>> RoadSerializer = new RoadInfoXmlSerializer().ToTemplateWriter();
        static IWriter<string[]> TagWriter = new LandformTagXmlSerializer().ToTemplateWriter();

        [MenuItem("Templates/WriteTerrainResourceTemplate")]
        public static void Write()
        {
            Write(FileMode.Create);
        }

        public static void Write(FileMode fileMode)
        {
            LandformSerializer.Write(templateLandformInfos, fileMode);
            BuildingSerializer.Write(templateBuildingInfos, fileMode);
            RoadSerializer.Write(templateRoadInfos, fileMode);
            TagWriter.Write(templateTags, fileMode);
        }
    }
}

#endif