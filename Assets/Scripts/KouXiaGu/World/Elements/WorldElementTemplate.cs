using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using KouXiaGu.Collections;

namespace KouXiaGu.World
{

    /// <summary>
    /// 预定义模版;
    /// </summary>
    class WorldElementTemplate : WorldElementManager
    {

        const string StrNone = "None";

        #region Road;

        static readonly RoadInfo RoadTemplate = new RoadInfo()
        {
            ID = 1,
            Name = StrNone,
            Terrain = new TerrainRoadInfo()
            {
                DiffuseBlendTex = StrNone,
                DiffuseTex = StrNone,
                HeightAdjustBlendTex = StrNone,
                HeightAdjustTex = StrNone,
            },
        };

        static readonly RoadInfo[] RoadTemplates = new RoadInfo[]
            {
                RoadTemplate,
            };

        #endregion

        #region Landform;

        static readonly LandformInfo LandformTemplate = new LandformInfo()
        {
            ID = 1,
            Name = StrNone,
            Terrain = new TerrainLandformInfo()
            {
                DiffuseBlendTex = StrNone,
                DiffuseTex = StrNone,
                HeightBlendTex = StrNone,
                HeightTex = StrNone,
            },
        };

        static readonly LandformInfo[] LandformTemplates = new LandformInfo[]
           {
                LandformTemplate,
           };

        #endregion

        #region Building

        static readonly BuildingInfo BuildingTemplate = new BuildingInfo()
        {
            ID = 1,
            Name = StrNone,
            Terrain = new TerrainBuildingInfo()
            {
                PrefabName = StrNone,
            },
        };

        static readonly BuildingInfo[] BuildingTemplates = new BuildingInfo[]
          {
                BuildingTemplate,
          };

        #endregion

        #region Product

        static readonly ProductElementInfo ProductTemplate = new ProductElementInfo()
        {
            ID = 1,
            Name = StrNone,
        };

        static readonly ProductElementInfo[] ProductTemplates = new ProductElementInfo[]
          {
                ProductTemplate,
          };

        #endregion

        public WorldElementTemplate() : base()
        {
            AddDictionary(RoadInfos, RoadTemplates);
            AddDictionary(LandformInfos, LandformTemplates);
            AddDictionary(BuildingInfos, BuildingTemplates);
            AddDictionary(ProductInfos, ProductTemplates);
        }

        void AddDictionary<T>(Dictionary<int, T> dictionary, IEnumerable<T> items)
            where T : ElementInfo
        {
            dictionary.AddOrUpdate(items, item => item.ID);
        }

    }


}
