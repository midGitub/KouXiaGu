using System;
using System.Collections.Generic;
using System.IO;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 获取地图文件;
    /// </summary>
    public class MapFileManager
    {

        MapInfoReader defaultInfoReader
        {
            get { return MapInfoReader.DefaultReader; }
        }

        public virtual string DefaultMapsDirectory
        {
            get { return Path.Combine(ResourcePath.ConfigDirectoryPath, "Maps"); }
        }

        public MapFileManager()
        {
        }

        public virtual IEnumerable<MapFile> SearchAll(string dirPath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var filePaths = Directory.GetFiles(dirPath, defaultInfoReader.FileSearchPattern, searchOption);

            foreach (var filePath in filePaths)
            {
                MapInfo info;
                if (TryReadInfo(filePath, out info))
                {
                    MapFile file = new MapFile(filePath, info);
                    yield return file;
                }
            }
        }

        bool TryReadInfo(string filePath, out MapInfo info)
        {
            try
            {
                info = defaultInfoReader.Read(filePath);
                return true;
            }
            catch
            {
                info = default(MapInfo);
                return false;
            }
        }

        public MapFile Create(MapInfo Info)
        {
            string dirPath = Path.Combine(DefaultMapsDirectory, Info.ID.ToString());

            if (Directory.Exists(dirPath) && Directory.GetFiles(dirPath).Length != 0)
                throw new ArgumentException("已经存在相同ID的地图;ID:" + Info.ID);

            return Create(dirPath, Info);
        }

        public virtual MapFile Create(string dirPath, MapInfo Info)
        {
            Directory.CreateDirectory(dirPath);
            string infoPath = Path.Combine(dirPath, Info.Name.ToString());
            Path.ChangeExtension(infoPath, defaultInfoReader.FileExtension);

            var file = new MapFile(infoPath, Info);
            file.WriteInfo();
            return file;
        }

    }

    /// <summary>
    /// 地图文件;
    /// </summary>
    public class MapFile
    {

        public string InfoPath { get; private set; }
        public MapInfo Info { get; set; }

        MapInfoReader defaultInfoReader
        {
            get { return MapInfoReader.DefaultReader; }
        }

        MapReader defaultMapReader
        {
            get { return MapReader.DefaultReader; }
        }

        public MapFile(string infoPath, MapInfo info)
        {
            InfoPath = infoPath;
            Info = info;
        }

        public MapInfo RereadInfo()
        {
            Info = defaultInfoReader.Read(InfoPath);
            return Info;
        }

        public void WriteInfo()
        {
            defaultInfoReader.Write(InfoPath, Info);
        }

        public Map ReadMap()
        {
            string filePath = GetMapDataFilePath();
            Map map = defaultMapReader.Read(filePath);
            return map;
        }

        public void WriteMap(Map map)
        {
            string filePath = GetMapDataFilePath();
            defaultMapReader.Write(filePath, map);
        }

        public string GetMapDataFilePath()
        {
            return Path.ChangeExtension(InfoPath, defaultMapReader.FileExtension);
        }

    }

}
