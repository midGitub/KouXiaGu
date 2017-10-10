using JiongXiaGu.Unity.Archives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using JiongXiaGu.Unity.Resources;
using System.IO;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图场景数据;
    /// </summary>
    public class MapSceneArchivalData : IDataArchival
    {
        /// <summary>
        /// 游戏地图存放目录;
        /// </summary>
        [PathDefinition(ResourceTypes.Archive, "存档地图存放路径;")]
        internal const string ArchiveMapFileName = "Map/ArchiveMap";

        /// <summary>
        /// 地图读写器;
        /// </summary>
        readonly MapReader mapXmlReader = new MapReader();

        /// <summary>
        /// 主地图文件信息;
        /// </summary>
        public MapFileInfo MainMapFileInfo { get; private set; }

        /// <summary>
        /// 用于存档的地图,若不存在则为Null;
        /// </summary>
        public Map ArchiveMap { get; private set; }

        public MapSceneArchivalData(MapFileInfo mainMapFileInfo) 
        {
            if (mainMapFileInfo == null)
                throw new ArgumentNullException(nameof(mainMapFileInfo));

            MainMapFileInfo = mainMapFileInfo;
        }

        public MapSceneArchivalData(MapFileInfo mainMapFileInfo, Map archiveMap)
        {
            if (mainMapFileInfo == null)
                throw new ArgumentNullException(nameof(mainMapFileInfo));
            if (archiveMap == null)
                throw new ArgumentNullException(nameof(archiveMap));

            MainMapFileInfo = mainMapFileInfo;
            ArchiveMap = archiveMap;
        }

        /// <summary>
        /// 输出存档内容;
        /// </summary>
        public Task Write(IArchiveFileInfo archive, CancellationToken cancellationToken)
        {
            return Task.Run(delegate ()
            {
                if (ArchiveMap != null)
                {
                    string filePath = Path.Combine(archive.ArchiveDirectory, ArchiveMapFileName);
                    mapXmlReader.WriteToFile(filePath, ArchiveMap);
                }
            });
        }
    }
}
