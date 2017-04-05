using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
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

        public ArchiveMap(Map map)
        {
            Subscribe(map);
        }

        public void Subscribe(Map map)
        {
            Data.Subscribe(map.Data);
            Road = map.Road;
        }
    }

    [ProtoContract]
    public struct ArchiveMapInfo
    {
        /// <summary>
        /// 地图ID;
        /// </summary>
        [XmlAttribute("id")]
        public int ID { get; set; }
    }

    /// <summary>
    /// 存档地图文件 管理\记录;
    /// </summary>
    public class ArchiveMapFile
    {

        public ArchiveMap Read()
        {
            throw new NotImplementedException();
        }

        public void Write(ArchiveMap map)
        {
            throw new NotImplementedException();
        }

    }

}
