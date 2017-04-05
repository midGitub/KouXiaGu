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

        public void Update(ArchiveMap archive)
        {
            Data.AddOrUpdate(archive.Data);
            Road = archive.Road;
        }

    }

    /// <summary>
    /// 地图文件 管理\记录;
    /// </summary>
    public class MapFile
    {

        static readonly MapInfoReader defaultInfoReader;
        static readonly MapReader defaultMapReader;

        public static string DefaultMapsDirectory
        {
            get { return Path.Combine(ResourcePath.ConfigDirectoryPath, "Maps"); }
        }

        public static string InfoFileSearchPattern
        {
            get { return "*" + defaultInfoReader.FileExtension; }
        }

        static MapFile()
        {
            defaultInfoReader = new MapInfoReader();
            defaultMapReader = new ProtoMapReader();
        }

        public static IEnumerable<MapFile> SearchAll(string dirPath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var filePaths = Directory.GetFiles(dirPath, InfoFileSearchPattern, searchOption);

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

        static bool TryReadInfo(string filePath, out MapInfo info)
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

        public static MapFile Create(MapInfo Info)
        {
            string dirPath = Path.Combine(DefaultMapsDirectory, Info.ID.ToString());

            if (Directory.Exists(dirPath) && Directory.GetFiles(dirPath).Length != 0)
                throw new ArgumentException("已经存在相同ID的地图;ID:" + Info.ID);

            return Create(dirPath, Info);
        }

        public static MapFile Create(string dirPath, MapInfo Info)
        {
            Directory.CreateDirectory(dirPath);
            string infoPath = Path.Combine(dirPath, Info.Name.ToString());
            Path.ChangeExtension(infoPath, defaultInfoReader.FileExtension);

            var file = new MapFile(infoPath, Info);
            file.WriteInfo();
            return file;
        }


        public string InfoPath { get; private set; }
        public MapInfo Info { get; set; }

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
