using JiongXiaGu.Grids;
using ProtoBuf;
using System;
using System.Collections.Generic;

namespace JiongXiaGu.World.Map
{

    /// <summary>
    /// 游戏地图数据;
    /// </summary>
    [ProtoContract]
    public class MapData
    {
        [ProtoMember(1)]
        public Dictionary<CubicHexCoord, MapNode> Data { get; set; }

        [ProtoMember(2)]
        public IdentifierGenerator Landform { get; set; }

        [ProtoMember(3)]
        public IdentifierGenerator Road { get; set; }

        [ProtoMember(4)]
        public IdentifierGenerator Building { get; set; }

        [ProtoMember(5)]
        public TownCorePositions TownCorePositions { get; set; }
    }
}
