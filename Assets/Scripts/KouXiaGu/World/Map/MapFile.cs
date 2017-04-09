using System;
using System.Collections.Generic;
using System.IO;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 地图文件;
    /// </summary>
    public class MapFile
    {

        public string InfoPath { get; private set; }

        MapInfoReader defaultInfoReader
        {
            get { return MapInfoReader.DefaultReader; }
        }

        MapReader defaultMapReader
        {
            get { return MapReader.DefaultReader; }
        }


        public MapFile(string infoPath)
        {
            InfoPath = infoPath;
        }


        public MapInfo RereadInfo()
        {
            var info = defaultInfoReader.Read(InfoPath);
            return info;
        }

        public void WriteInfo(MapInfo info)
        {
            defaultInfoReader.Write(InfoPath, info);
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
