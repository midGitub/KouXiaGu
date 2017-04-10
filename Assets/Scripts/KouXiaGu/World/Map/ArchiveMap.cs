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
        public ArchiveDictionary<CubicHexCoord, MapNode> Data { get; set; }

        [ProtoMember(2)]
        public MapRoad Road { get; set; }

        [ProtoMember(3)]
        public MapTown Town { get; set; }

        public ArchiveMap()
        {
        }

        public ArchiveMap(Map map)
        {
            Data.Subscribe(map.Data);
            Road = map.Road;
            Town = map.Town;
        }

        public void Subscribe(Map map)
        {
            Data.Subscribe(map.Data);
        }
    }

}
