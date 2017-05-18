using KouXiaGu.Grids;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 游戏地图数据;
    /// </summary>
    [ProtoContract]
    public class MapData
    {
        public MapData()
        {
            Map = new Dictionary<CubicHexCoord, MapNode>();
        }

        /// <summary>
        /// 地图数据;
        /// </summary>
        [ProtoMember(1)]
        public Dictionary<CubicHexCoord, MapNode> Map { get; set; }

    }

}
