using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 用于存储地图的结构;
    /// </summary>
    [ProtoContract]
    public struct BlockArchive<T>
    {

        public BlockArchive(ShortVector2 Coord, Dictionary<CubicHexCoord, T> Map)
        {
            this.Coord = Coord;
            this.Map = Map;
        }

        /// <summary>
        /// 块坐标;
        /// </summary>
        [ProtoMember(1)]
        public ShortVector2 Coord { get; private set; }

        /// <summary>
        /// 块内容;
        /// </summary>
        [ProtoMember(2)]
        public Dictionary<CubicHexCoord, T> Map { get; private set; }

    }

}
