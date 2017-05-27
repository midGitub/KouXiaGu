using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using KouXiaGu.Collections;
using KouXiaGu.Navigation;

namespace KouXiaGu.Resources
{

    /// <summary>
    /// 预定义模版;
    /// </summary>
    class WorldElementTemplate : BasicTerrainResource
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
            Navigation = new NavLandformInfo(),
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
            Terrain = new TerrainBuildingInfo()
            {
                PrefabName = StrNone,
            },
            Navigation = new NavBuildingInfo(),
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

        public WorldElementTemplate() : this(true)
        {
            AddDictionary(Road, RoadTemplates);
            AddDictionary(Landform, LandformTemplates);
            //AddDictionary(Building, BuildingTemplates);
        }

        public WorldElementTemplate(bool changeFileName) : base()
        {
            this.ChangeFileName = changeFileName;
        }

        /// <summary>
        /// 当存在相同文件时,是否改名后保存?
        /// </summary>
        public bool ChangeFileName { get; set; }

        void AddDictionary<T>(Dictionary<int, T> dictionary, IEnumerable<T> items)
            where T : ElementInfo
        {
            dictionary.AddOrUpdate(items, item => item.ID);
        }

        //protected override void WriteToDirectory<T>(
        //    DataReader<Dictionary<int, T>, IEnumerable<T>> reader,
        //    Dictionary<int, T> dictionary,
        //    string dirPath,
        //    bool overlay)
        //{
        //    IEnumerable<T> infos = dictionary.Values;

        //    if (!overlay && reader.File.Exists(dirPath) && ChangeFileName)
        //        reader.WriteToDirectory(infos, dirPath, "_Template_");
        //    else
        //        base.WriteToDirectory(reader, dictionary, dirPath, overlay);
        //}

    }


}
