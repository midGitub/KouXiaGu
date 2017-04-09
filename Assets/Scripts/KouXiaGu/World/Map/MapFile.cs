using System;
using System.Collections.Generic;
using System.IO;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 地图文件;
    /// </summary>
    public class MapFile : IMapReader
    {

        public string InfoPath { get; private set; }
        public MapInfo Info { get; set; }

        MapInfoReader defaultInfoReader
        {
            get { return MapInfoReader.DefaultReader; }
        }

        MapFileReader defaultMapReader
        {
            get { return MapFileReader.DefaultReader; }
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

        Map IMapReader.Read()
        {
            return ReadMap();
        }

    }

}
