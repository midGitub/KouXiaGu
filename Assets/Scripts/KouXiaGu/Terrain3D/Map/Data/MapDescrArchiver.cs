using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Initialization;
using System.IO;

namespace KouXiaGu.Terrain3D
{


    /// <summary>
    /// 在存档时,将新的地图描述信息保存到存档;
    /// </summary>
    public class MapDescrArchiver
    {

        const string DESCR_FILE_NAME = "Map";

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
                return MapDescrFiler.Read();
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
