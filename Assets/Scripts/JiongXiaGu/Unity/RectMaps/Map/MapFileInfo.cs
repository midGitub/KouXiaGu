using System.IO;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 游戏地图文件信息;
    /// </summary>
    public class MapFileInfo
    {
        /// <summary>
        /// 地图文件描述信息;
        /// </summary>
        public MapDescription Description { get; internal set; }

        /// <summary>
        /// 地图文件路径;
        /// </summary>
        public FileInfo FileInfo { get; private set; }

        public MapFileInfo(FileInfo file, MapDescription description)
        {
            FileInfo = file;
            Description = description;
        }
    }
}
