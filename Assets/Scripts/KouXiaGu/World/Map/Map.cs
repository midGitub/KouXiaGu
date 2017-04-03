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
