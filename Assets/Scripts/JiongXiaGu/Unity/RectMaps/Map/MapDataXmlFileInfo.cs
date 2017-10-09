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
        /// 地图名;
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// 地图版本;
        /// </summary>
        public int Version { get; internal set; }

        /// <summary>
        /// 地图文件路径;
        /// </summary>
        public FileInfo File { get; internal set; }

        public MapDataXmlFileInfo(FileInfo file, string name, int version)
        {
            File = file;
            Name = name;
            Version = version;
        }
    }
}
