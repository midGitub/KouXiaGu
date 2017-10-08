using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using ProtoBuf;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 游戏地图文件信息;
    /// </summary>
    public class MapDataXmlFileInfo
    {
        /// <summary>
        /// 地图文件路径;
        /// </summary>
        public FileInfo File { get; private set; }

        /// <summary>
        /// 地图信息;
        /// </summary>
        public MapDescription Description { get; private set; }

        public MapDataXmlFileInfo(FileInfo file, MapDescription description)
        {
            File = file;
            Description = description;
        }
    }
}
