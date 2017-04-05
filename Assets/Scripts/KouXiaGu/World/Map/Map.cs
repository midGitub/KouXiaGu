using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KouXiaGu.Collections;
using KouXiaGu.Grids;
using ProtoBuf;

namespace KouXiaGu.World.Map
{

    public abstract class MapReader
    {
        public virtual string SearchPattern
        {
            get { return "*" + FileExtension; }
        }

        public abstract string FileExtension { get; }
        public abstract Map Read(string filePath);
        public abstract void Write(string filePath, Map data);
    }

    public class ProtoMapReader : MapReader
    {
        public override string FileExtension
        {
            get { return ".map"; }
        }

        public override Map Read(string filePath)
        {
            Map data = ProtoBufExtensions.Deserialize<Map>(filePath);
            return data;
        }

        public override void Write(string filePath, Map data)
        {
            ProtoBufExtensions.Serialize(filePath, data);
        }
    }

    [ProtoContract]
    public class Map
    {
        public static Map Read(MapReader reader, MapFile file)
        {
            string filePath = GetMapDataFilePath(reader, file);
            Map map = reader.Read(filePath);
            map.File = file;
            return map;
        }

        public static void Write(MapReader reader, Map map)
        {
            string filePath = GetMapDataFilePath(reader, map.File);
            reader.Write(filePath, map);
        }

        static string GetMapDataFilePath(MapReader reader, MapFile file)
        {
            return Path.ChangeExtension(file.InfoPath, reader.FileExtension);
        }


        [ProtoMember(1)]
        public ObservableDictionary<CubicHexCoord, MapNode> Data { get; private set; }

        [ProtoMember(2)]
        public RoadInfo Road { get; private set; }

        public MapFile File { get; private set; }

        Map()
        {
        }

        public Map(MapFile file)
        {
            Data = new ObservableDictionary<CubicHexCoord, MapNode>();
            Road = new RoadInfo();
            File = file;
        }

    }

    /// <summary>
    /// 地图文件;
    /// </summary>
    public class MapFile
    {
        static MapFile()
        {
            infoReader = new MapInfoReader();
        }

        static readonly MapInfoReader infoReader;

        public MapInfo Info { get; private set; }
        public string InfoPath { get; private set; }

        public MapInfoReader InfoReader
        {
            get { return infoReader; }
        }

        public string InfoFileSearchPattern
        {
            get { return "*" + InfoReader.FileExtension; }
        }

        public MapFile(string filePath)
        {
            ReadInfo(filePath);
        }

        public MapFile(string dirPath, SearchOption searchOption)
        {
            var filePaths = Directory.GetFiles(dirPath, InfoFileSearchPattern, searchOption);

            if (filePaths.Length == 0)
                throw new FileNotFoundException();

            string filePath = filePaths[0];
            ReadInfo(filePath);
        }

        void ReadInfo(string filePath)
        {
            InfoPath = filePath;
            Info = InfoReader.Read(filePath);
        }

        public void WriteInfo(MapInfo info)
        {
            string filePath = InfoPath;
            InfoReader.Write(filePath, info);
            this.Info = info;
        }

    }

}
