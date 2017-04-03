using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Collections;
using KouXiaGu.Grids;
using KouXiaGu.Terrain3D;
using ProtoBuf;
using System.IO;
using System.Xml.Serialization;

namespace KouXiaGu.World.Map
{


    public class MapFile
    {
        static readonly MapInfoReader infoReader = new MapInfoReader();
        static readonly MapReader mapReader = new ProtoMapReader();

        public MapInfo Info { get; private set; }
        public string InfoPath { get; private set; }

        public MapInfoReader InfoReader
        {
            get { return infoReader; }
        }

        public MapReader MapReader
        {
            get { return mapReader; }
        }

        public string InfoFileSearchPattern
        {
            get { return "*" + InfoReader.FileExtension; }
        }

        public MapFile(string filePath)
        {
            ReadInfoFile(filePath);
        }

        public MapFile(string dirPath, SearchOption searchOption)
        {
            var filePaths = Directory.GetFiles(dirPath, InfoFileSearchPattern, searchOption);

            if (filePaths.Length == 0)
                throw new FileNotFoundException();

            string filePath = filePaths[0];
            ReadInfoFile(filePath);
        }

        void ReadInfoFile(string filePath)
        {
            InfoPath = filePath;
            Info = InfoReader.Read(filePath);
        }

        public string GetMapDataFilePath()
        {
            return Path.ChangeExtension(InfoPath, MapReader.FileExtension);
        }

        public Map ReadMap()
        {
            string filePath = GetMapDataFilePath();
            return MapReader.Read(filePath);
        }

        public void WriteMap(Map data)
        {
            string filePath = GetMapDataFilePath();
            MapReader.Write(filePath, data);
        }

        public void WriteInfo(MapInfo data)
        {
            string filePath = InfoPath;
            InfoReader.Write(filePath, data);
        }

    }



    public class MapInfoReader
    {
        public MapInfoReader()
        {
            serializer = new XmlSerializer(typeof(MapInfo));
        }

        XmlSerializer serializer;

        public string FileExtension
        {
            get { return ".xml"; }
        }

        public MapInfo Read(string filePath)
        {
            return (MapInfo)serializer.DeserializeXiaGu(filePath);
        }

        public void Write(string filePath, MapInfo data)
        {
            serializer.SerializeXiaGu(filePath, data);
        }
    }

    [XmlType("MapInfo")]
    public struct MapInfo
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }
    }


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
        public ObservableDictionary<CubicHexCoord, TerrainNode> Data { get; private set; }

        [ProtoMember(2)]
        public RoadInfo Road { get; set; }

        public Map()
        {
            Data = new ObservableDictionary<CubicHexCoord, TerrainNode>();
            Road = new RoadInfo();
        }
    }



    public abstract class ArchiveMapReader
    {
        public abstract string FileExtension { get; }
        public abstract ArchiveMap Read(string filePath);
        public abstract void Write(string filePath, ArchiveMap data);
    }

    public class ProtoArchiveMapReader : ArchiveMapReader
    {
        public override string FileExtension
        {
            get { return ".aMap"; }
        }

        public override ArchiveMap Read(string filePath)
        {
            ArchiveMap data = ProtoBufExtensions.Deserialize<ArchiveMap>(filePath);
            return data;
        }

        public override void Write(string filePath, ArchiveMap data)
        {
            ProtoBufExtensions.Serialize(filePath, data);
        }
    }

    [ProtoContract]
    public class ArchiveMap
    {
        [ProtoMember(1)]
        public DictionaryArchiver<CubicHexCoord, TerrainNode> Data { get; set; }

        [ProtoMember(2)]
        public RoadInfo Road { get; set; }
    }


}
