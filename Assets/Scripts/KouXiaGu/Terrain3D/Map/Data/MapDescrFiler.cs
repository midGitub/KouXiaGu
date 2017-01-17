using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{


    public class MapDescrFiler
    {

        const string DESCR_FILE_NAME = "Map";


        /// <summary>
        /// 读取地图描述信息;
        /// </summary>
        public static MapDescr Read()
        {
            string filePath = TerrainFiler.Combine(DESCR_FILE_NAME);
            return MapDescr.XmlRead(filePath);
        }

        /// <summary>
        /// 保存地图描述信息;
        /// </summary>
        public static void Write(MapDescr data)
        {
            string filePath = TerrainFiler.Combine(DESCR_FILE_NAME);
            MapDescr.XmlWrite(filePath, data);
        }


    }

}
