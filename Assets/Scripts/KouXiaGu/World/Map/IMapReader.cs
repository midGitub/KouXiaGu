using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 读取地图接口;
    /// </summary>
    public interface IMapReader
    {
        /// <summary>
        /// 地图信息;
        /// </summary>
        MapFile Predefined { get; }

        /// <summary>
        /// 获取到地图,包括存档内容;
        /// </summary>
        Map GetMap();

        /// <summary>
        /// 获取到继续存档内容;
        /// </summary>
        ArchiveMap GetArchiveMap();
    }


    /// <summary>
    /// 从存档读取到地图;
    /// </summary>
    public class MapFromArchived : IMapReader
    {
        Map map;
        ArchiveMapFile archiveMapFile;
        ArchiveMap archiveMap;
        public string ArchiveDir { get; private set; }
        public MapFile Predefined { get; private set; }

        public MapFromArchived(string archiveDir)
        {
            ArchiveDir = archiveDir;
            archiveMapFile = new ArchiveMapFile(archiveDir);
            Predefined = FindMapFile(archiveMapFile);
        }

        MapFile FindMapFile(ArchiveMapFile archiveMapFile)
        {
            ArchiveMapInfo archiveInfo = archiveMapFile.ReadInfo();
            try
            {
                var maps = MapFileManager.Default.SearchAll();
                var file = maps.First(item => item.Value.ID == archiveInfo.ID);
                return file.Key;
            }
            catch (InvalidOperationException)
            {
                throw new FileNotFoundException("未找到对应的地图文件;MapID:" + archiveInfo.ID);
            }
        }

        /// <summary>
        /// 读取到地图,包括存档内容;
        /// </summary>
        public Map GetMap()
        {
            var map = ReadMap();
            var archiveMap = ReadArchiveMap();
            map.Update(archiveMap);
            return map;
        }

        Map ReadMap()
        {
            if (map == null)
            {
                map = Predefined.ReadMap();
            }
            return map;
        }

        public ArchiveMap GetArchiveMap()
        {
            return ReadArchiveMap();
        }

        ArchiveMap ReadArchiveMap()
        {
            if (archiveMap == null)
            {
                archiveMap = archiveMapFile.ReadMap();
            }
            return archiveMap;
        }

    }

    /// <summary>
    /// 直接读取预定义的地图文件;
    /// </summary>
    public class MapFromPredefined : IMapReader
    {
        public MapFile Predefined { get; private set; }

        public MapFromPredefined(MapFile mapFile)
        {
            Predefined = mapFile;
        }

        public Map GetMap()
        {
            return Predefined.ReadMap();
        }

        public ArchiveMap GetArchiveMap()
        {
            return new ArchiveMap();
        }
    }

}
