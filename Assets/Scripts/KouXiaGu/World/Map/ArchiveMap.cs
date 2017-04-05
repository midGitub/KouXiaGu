using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;
using KouXiaGu.Grids;
using ProtoBuf;

namespace KouXiaGu.World.Map
{

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
        public DictionaryArchiver<CubicHexCoord, MapNode> Data { get; set; }

        [ProtoMember(2)]
        public RoadInfo Road { get; set; }

        public ArchiveMap()
        {
        }

    }

}
