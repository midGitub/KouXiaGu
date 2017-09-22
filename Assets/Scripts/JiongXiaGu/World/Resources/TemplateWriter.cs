
#if UNITY_EDITOR

using JiongXiaGu.Resources;
using JiongXiaGu.Terrain3D;
using JiongXiaGu.World.Commerce;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace JiongXiaGu.World.Resources
{


    class TemplateWriter
    {

        static readonly Dictionary<int, OLandformInfo> templateLandformInfos = new Dictionary<int, OLandformInfo>()
        {
            { 1, new OLandformInfo()
            {
                ID = 1,
                Name = "none",
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
                Commerce = new BuildingCommerceInfo()
                {
                    Worth = 1000,
                    ProductionContents = new List<ProductionContent>()
                    {
                        new ProductionContent("春", new ProductProduction[]{ new ProductProduction(0, 10), new ProductProduction(1, 30)}),
                        new ProductionContent("夏", new ProductProduction[]{ new ProductProduction(0, 30), new ProductProduction(1, 90)}),
                    }
                }
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
                Name = "香蕉",
                Commerce = new ProductCommerceInfo()
                {
                    Worth = 10,
                }
            }
            },
            { 2, new ProductInfo()
            {
                ID = 2,
                Name = "苹果",
                Commerce = new ProductCommerceInfo()
                {
                    Worth = 15,
                },
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

        static IWriter<Dictionary<int, OLandformInfo>> LandformSerializer = new LandformInfoXmlSerializer().ToTemplateWriter();
        static IWriter<Dictionary<int, BuildingInfo>> BuildingSerializer = new BuildingInfoXmlSerializer().ToTemplateWriter();
        static IWriter<Dictionary<int, RoadInfo>> RoadSerializer = new RoadInfoXmlSerializer().ToTemplateWriter();
        static IWriter<Dictionary<int, ProductInfo>> ProductSerializer = new ProductInfoXmlSerializer().ToTemplateWriter();
        static IWriter<Dictionary<int, TownInfo>> TownSerializer = new TownInfoXmlSerializer().ToTemplateWriter();
        static IWriter<LandformTag[]> TagWriter = new LandformTagXmlSerializer().ToTemplateWriter();

        [MenuItem("Templates/WriteTerrainResourceTemplate")]
        public static void Write()
        {
            LandformSerializer.Write(templateLandformInfos);
            BuildingSerializer.Write(templateBuildingInfos);
            RoadSerializer.Write(templateRoadInfos);
            ProductSerializer.Write(templateProduct);
            TownSerializer.Write(templateTowns);
            TagWriter.Write(templateTags);
        }
    }
}

#endif