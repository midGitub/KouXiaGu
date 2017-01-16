using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using ProtoBuf;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 道路节点信息;
    /// </summary>
    [ProtoContract]
    public struct RoadInfo
    {

        /// <summary>
        /// 道路表示的编号;
        /// </summary>
        [ProtoMember(1)]
        public int ID_0;

        /// <summary>
        /// 下一个节点方向;
        /// </summary>
        [ProtoMember(2)]
        public HexDirections Next_0;

        /// <summary>
        /// 上一个节点方向;
        /// </summary>
        [ProtoMember(3)]
        public HexDirections Previous_0;

        /// <summary>
        /// 是否存在道路0?
        /// </summary>
        public bool ExistRoad_0
        {
            get { return ID_0 != 0; }
        }


        /// <summary>
        /// 道路表示的编号;
        /// </summary>
        [ProtoMember(11)]
        public int ID_1;

        /// <summary>
        /// 下一个节点方向;
        /// </summary>
        [ProtoMember(12)]
        public HexDirections Next_1;

        /// <summary>
        /// 上一个节点方向;
        /// </summary>
        [ProtoMember(13)]
        public HexDirections Previous_1;

        /// <summary>
        /// 是否存在道路0?
        /// </summary>
        public bool ExistRoad_1
        {
            get { return ID_1 != 0; }
        }



        /// <summary>
        /// 是否存在道路?
        /// </summary>
        public bool ExistRoad
        {
            get { return ExistRoad_0 && ExistRoad_1; }
        }

        /// <summary>
        /// 检查两个道路信息是否发生重复?
        /// </summary>
        public bool IsDuplicates()
        {
            return
                Next_0 == Next_1 &&
                Previous_0 == Previous_1;
        }

    }


    /// <summary>
    /// 道路编辑器;
    /// </summary>
    public class RoadEdit
    {

        public RoadEdit()
        {
        }

        public RoadEdit(IDictionary<CubicHexCoord, RoadInfo> map)
        {
            this.Map = map;
        }

        /// <summary>
        /// 最多两种道路重叠，组成 丁字路口，Y字路口，十字路口，道路;
        /// </summary>
        public const int MAX_ROAD_COUNT = 2;

        public IDictionary<CubicHexCoord, RoadInfo> Map { get; set; }


        /// <summary>
        /// 向位置加入道路信息,并且自动链接到附近断头路,若不存在断头路,则作为一个孤立的点;
        /// </summary>
        public void AddRoadInfo(CubicHexCoord coord)
        {

        }


        /// <summary>
        /// 寻找点周围的断头路;
        /// </summary>
        public IEnumerable<CoordPack<CubicHexCoord, HexDirections>> FindBlindAlley(CubicHexCoord coord)
        {
            foreach (var neighbour in coord.GetNeighbours())
            {
                yield return neighbour;
            }
        }

        /// <summary>
        /// 是否存在断头路;
        /// </summary>
        bool ExistBlindAlley(RoadInfo roadInfo)
        {
            return 
                ExistBlindAlley(roadInfo.Previous_0, roadInfo.Next_0) ||
                ExistBlindAlley(roadInfo.Previous_1, roadInfo.Next_1);
        }

        /// <summary>
        /// 是否任意一个为 Unknown ?
        /// </summary>
        bool ExistBlindAlley(HexDirections previous, HexDirections next)
        {
            if (previous == HexDirections.Unknown && next != HexDirections.Unknown)
                return true;
            else if (previous != HexDirections.Unknown && next == HexDirections.Unknown)
                return true;
            else
                return false;
        }

    }

}
