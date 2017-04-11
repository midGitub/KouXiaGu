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
    class WorldElementTemplate : WorldElement
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


        public WorldElementTemplate() : base()
        {
            AddDictionary(RoadInfos, RoadTemplates);
            AddDictionary(LandformInfos, LandformTemplates);
        }

        void AddDictionary<T>(Dictionary<int, T> dictionary, IEnumerable<T> items)
            where T : IMarked
        {
            dictionary.AddOrUpdate(items, item => item.ID);
        }

    }


}
