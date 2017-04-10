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


        public MapInfo ReadInfo()
        {
            var info = defaultInfoReader.Read(InfoPath);
            return info;
        }

        public void WriteInfo(MapInfo info)
        {
            defaultInfoReader.Write(InfoPath, info);
        }


        public MapData ReadMap()
        {
            string filePath = GetMapDataFilePath();
            MapData map = defaultMapReader.Read(filePath);
            return map;
        }

        public void WriteMap(MapData map)
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
