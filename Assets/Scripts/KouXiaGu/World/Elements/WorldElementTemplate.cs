using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.World
{

    class WorldElementTemplate : WorldElement
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

        public static void WriteLandformTemplate(string dirPath, bool overlay)
        {
            if (!overlay && LandformReader.File.Exists(dirPath))
                return;

            LandformReader.Write(LandformTemplates, dirPath);
        }

        #endregion

    }


}
