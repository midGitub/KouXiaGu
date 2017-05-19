using KouXiaGu.Grids;
using ProtoBuf;
using System;
using System.Collections.Generic;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 游戏地图数据;
    /// </summary>
    [ProtoContract]
    public class MapData
    {
        [ProtoMember(0)]
        public Dictionary<CubicHexCoord, MapNode> Map { get; set; }

        [ProtoMember(1)]
        public IdentifierGenerator Landform { get; set; }

        [ProtoMember(2)]
        public IdentifierGenerator Road { get; set; }

        [ProtoMember(3)]
        public IdentifierGenerator Building { get; set; }
    }

}
