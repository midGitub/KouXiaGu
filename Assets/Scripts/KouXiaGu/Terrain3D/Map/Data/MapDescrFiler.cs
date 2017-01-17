using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using KouXiaGu.Initialization;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 描述文件管理;
    /// </summary>
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


        /// <summary>
        /// 从存档获取到描述信息,若无法获取到,则返回默认的描述信息;
        /// </summary>
        public static MapDescr Read(ArchiveFile archive)
        {
            try
            {
                string filePath = archive.CombineToTerrain(DESCR_FILE_NAME);
                return MapDescr.Read(filePath);
            }
            catch (FileNotFoundException)
            {
                return Read();
            }
        }

        /// <summary>
        /// 输出描述信息到存档;
        /// </summary>
        public static void Write(ArchiveFile archive, MapDescr data)
        {
            string filePath = archive.CombineToTerrain(DESCR_FILE_NAME);
            MapDescr.Write(filePath, data);
        }

    }

}
