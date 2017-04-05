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
            Data.Subscribe(map.Data);
            Road = map.Road;
        }

        public void Subscribe(Map map)
        {
            Data.Subscribe(map.Data);
        }
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
