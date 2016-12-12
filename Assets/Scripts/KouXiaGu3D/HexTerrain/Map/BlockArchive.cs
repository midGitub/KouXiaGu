using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using ProtoBuf;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 用于存储地图的结构;
    /// </summary>
    [ProtoContract]
    public struct BlockArchive<TP, T>
    {

        public BlockArchive(RectCoord coord, int size, Dictionary<TP, T> map)
        {
            this.Coord = coord;
            this.Width = size;
            this.Map = map;
        }

        /// <summary>
        /// 块坐标;
        /// </summary>
        [ProtoMember(1)]
        public RectCoord Coord { get; private set; }

        /// <summary>
        /// 地图块大小;
        /// </summary>
        [ProtoMember(2)]
        public int Width { get; private set; }

        /// <summary>
        /// 块内容;
        /// </summary>
        [ProtoMember(3)]
        public Dictionary<TP, T> Map { get; private set; }

        public override string ToString()
        {
            string str = "坐标:" + Coord
                + "块尺寸:" + Width
                + "元素数:" + Map.Count;
            return str;
        }

    }

}
