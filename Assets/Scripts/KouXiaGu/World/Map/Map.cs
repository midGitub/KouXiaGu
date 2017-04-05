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

        [ProtoMember(1)]
        public ObservableDictionary<CubicHexCoord, MapNode> Data { get; private set; }

        [ProtoMember(2)]
        public RoadInfo Road { get; private set; }

        public Map()
        {
            Data = new ObservableDictionary<CubicHexCoord, MapNode>();
            Road = new RoadInfo();
        }

    }

    /// <summary>
    /// 地图文件 管理\记录;
    /// </summary>
    public class MapFile
    {
        static MapFile()
        {
            defaultInfoReader = new MapInfoReader();
            defaultMapReader = new ProtoMapReader();
        }

        static readonly MapInfoReader defaultInfoReader;
        static readonly MapReader defaultMapReader;

        public MapInfo Info { get; private set; }
        public string InfoPath { get; private set; }

        public string InfoFileSearchPattern
        {
            get { return "*" + defaultInfoReader.FileExtension; }
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
            Info = defaultInfoReader.Read(filePath);
        }

        public void WriteInfo(MapInfo info)
        {
            string filePath = InfoPath;
            defaultInfoReader.Write(filePath, info);
            this.Info = info;
        }

        public Map ReadMap()
        {
            return Read(defaultMapReader);
        }

        Map Read(MapReader reader)
        {
            string filePath = GetMapDataFilePath(reader);
            Map map = reader.Read(filePath);
            return map;
        }

        public void WriteMap(Map map)
        {
            Write(defaultMapReader, map);
        }

        void Write(MapReader reader, Map map)
        {
            string filePath = GetMapDataFilePath(reader);
            reader.Write(filePath, map);
        }

        string GetMapDataFilePath(MapReader reader)
        {
            return Path.ChangeExtension(InfoPath, reader.FileExtension);
        }

    }

}
