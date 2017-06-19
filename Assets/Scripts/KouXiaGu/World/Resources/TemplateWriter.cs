
#if UNITY_EDITOR

using KouXiaGu.Resources;
using KouXiaGu.Terrain3D;
using KouXiaGu.World.Commerce;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace KouXiaGu.World.Resources
{


    class TemplateWriter
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

        static readonly Dictionary<int, ProductInfo> templateProduct = new Dictionary<int, ProductInfo>()
        {
            {1, new ProductInfo()
            {
                ID = 1,
                Name = "Unknown",
                Commerce = new CommerceProduct()
                {
                    Worth = 0,
                }
            }
            }
        };

        static readonly Dictionary<int, TownInfo> templateTowns = new Dictionary<int, TownInfo>()
        {
            {1, new TownInfo()
            {
                ID = 1,
                Name = "Unknown",
                Description = "Unknown",
            }
            }
        };

        static readonly LandformTag[] templateTags = new LandformTag[]
            {
               new LandformTag()
               {
                   Name = "Unknown1"
               }
            };

        static IWriter<Dictionary<int, LandformInfo>> LandformSerializer = new LandformInfoXmlSerializer().ToTemplateWriter();
        static IWriter<Dictionary<int, BuildingInfo>> BuildingSerializer = new BuildingInfoXmlSerializer().ToTemplateWriter();
        static IWriter<Dictionary<int, RoadInfo>> RoadSerializer = new RoadInfoXmlSerializer().ToTemplateWriter();
        static IWriter<Dictionary<int, ProductInfo>> ProductSerializer = new ProductInfoXmlSerializer().ToTemplateWriter();
        static IWriter<Dictionary<int, TownInfo>> TownSerializer = new TownInfoXmlSerializer().ToTemplateWriter();
        static IWriter<LandformTag[]> TagWriter = new LandformTagXmlSerializer().ToTemplateWriter();

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
            ProductSerializer.Write(templateProduct, fileMode);
            TownSerializer.Write(templateTowns, fileMode);
            TagWriter.Write(templateTags, fileMode);
        }
    }
}

#endif