using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.World.Map
{


    public class MapDataEditor
    {

        static DataReader<Dictionary<int, RoadInfo>, RoadInfo[]> RoadReader
        {
            get { return MapManager.RoadReader; }
        }

        static DataReader<Dictionary<int, LandformInfo>, LandformInfo[]> LandformReader
        {
            get { return MapManager.LandformReader; }
        }


        public MapDataEditor()
        {
            Manager = new MapManager();
        }

        public MapDataEditor(MapManager manager)
        {
            Manager = manager;
        }

        public MapManager Manager { get; private set; }
    }


    static class DataTemplate
    {

        const string StrNone = "None";

        /// <summary>
        /// 输出空的模版文件;
        /// </summary>
        /// <param name="overlay">输出到的文件夹;</param>
        /// <param name="overlay">是否覆盖已经存在的文件?</param>
        public static void WriteTemplateAll(string dirPath, bool overlay)
        {
            WriteRoadTemplate(dirPath, overlay);
            WriteLandformTemplate(dirPath, overlay);
        }

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
                RoadTemplate,
            };

        static DataReader<Dictionary<int, RoadInfo>, RoadInfo[]> RoadReader
        {
            get { return MapManager.RoadReader; }
        }

        public static void WriteRoadTemplate(string dirPath, bool overlay)
        {
            if (!overlay && RoadReader.File.Exists(dirPath))
                return;

            RoadReader.Write(RoadTemplates, dirPath);
        }

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
                LandformTemplate,
           };

        static DataReader<Dictionary<int, LandformInfo>, LandformInfo[]> LandformReader
        {
            get { return MapManager.LandformReader; }
        }

        public static void WriteLandformTemplate(string dirPath, bool overlay)
        {
            if (!overlay && LandformReader.File.Exists(dirPath))
                return;

            LandformReader.Write(LandformTemplates, dirPath);
        }

        #endregion

    }


}
