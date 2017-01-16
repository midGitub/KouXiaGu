using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Collections;
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
        /// 道路的唯一编号;
        /// </summary>
        [ProtoMember(1)]
        public uint ID;

    }


    /// <summary>
    /// 道路编辑类;
    /// </summary>
    public static class RoadEdit
    {

        /// <summary>
        /// 进行编辑的地图;
        /// </summary>
        public static IDictionary<CubicHexCoord, RoadInfo> Map { get; set; }





        //public RoadEdit()
        //{
        //}

        //public RoadEdit(IDictionary<CubicHexCoord, RoadInfo> map)
        //{
        //    this.Map = map;
        //}

        ///// <summary>
        ///// 最多两种道路重叠，组成 丁字路口，Y字路口，十字路口，道路;
        ///// </summary>
        //public const int MAX_ROAD_COUNT = 2;

        //public IDictionary<CubicHexCoord, RoadInfo> Map { get; set; }



        ///// <summary>
        ///// 向位置加入道路信息,并且自动链接到附近断头路,
        ///// 若不存在,则作为一个孤立的点;
        ///// </summary>
        //public void AddRoad(CubicHexCoord coord)
        //{
        //    CoordPack<CubicHexCoord, HexDirections, RoadInfo> target;
        //    if (AssociateBlindAlley(coord))
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        AddRoad(coord, new RoadInfo.Road());
        //    }
        //}


        ///// <summary>
        ///// 关联到存在断头路的邻居节点,若关联了,则返回true,否则返回false;
        ///// </summary>
        //bool AssociateBlindAlley(CubicHexCoord target)
        //{
        //    foreach (var neighbour in Map.GetNeighbours<CubicHexCoord, HexDirections, RoadInfo>(target))
        //    {
        //        RoadInfo roadInfo = neighbour.Item;
        //        if (roadInfo.ExistRoad)
        //        {
        //            for (int i = 0; i < roadInfo.RoadInfos.Count; i++)
        //            {
        //                RoadInfo.Road road = roadInfo.RoadInfos[i];

        //                if (road.Next == HexDirections.Unknown)
        //                {
        //                    road.Next = CubicHexCoord.ToOppositeDirection(neighbour.Direction);
        //                    roadInfo.RoadInfos[i] = road;

        //                    RoadInfo.Road targetRoad = new RoadInfo.Road();
        //                    targetRoad.ID = road.ID;
        //                    targetRoad.Previous = neighbour.Direction;
        //                    AddRoad(target, targetRoad);
        //                    return true;
        //                }
        //                else if (road.Previous == HexDirections.Unknown)
        //                {
        //                    road.Previous = CubicHexCoord.ToOppositeDirection(neighbour.Direction);
        //                    roadInfo.RoadInfos[i] = road;

        //                    RoadInfo.Road targetRoad = new RoadInfo.Road();
        //                    targetRoad.ID = road.ID;
        //                    targetRoad.Next = neighbour.Direction;
        //                    AddRoad(target, targetRoad);
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// 添加道路信息到节点;
        ///// </summary>
        //void AddRoad(CubicHexCoord target, RoadInfo.Road targetRoad)
        //{
        //    RoadInfo roadInfo = Map[target];
        //    if (roadInfo.RoadInfos == null)
        //    {
        //        roadInfo.RoadInfos = new List<RoadInfo.Road>(MAX_ROAD_COUNT);
        //        roadInfo.RoadInfos.Add(targetRoad);
        //    }
        //    else if (roadInfo.RoadInfos.Count < MAX_ROAD_COUNT)
        //    {
        //        roadInfo.RoadInfos.Add(targetRoad);
        //    }
        //    else
        //    {
        //        throw new ArgumentOutOfRangeException();
        //    }
        //    Map[target] = roadInfo;
        //}


        ///// <summary>
        ///// 遍历获取到这条道路的路径;
        ///// </summary>
        //LinkedList<CubicHexCoord> RoadPath(CubicHexCoord startPoint, Road targetRoad)
        //{
        //    if (targetRoad.Next == HexDirections.Unknown && targetRoad.Previous == HexDirections.Unknown)
        //        throw new ArgumentException("空的道路节点;");

        //    LinkedList<CubicHexCoord> roadPath = new LinkedList<CubicHexCoord>();
        //    roadPath.AddFirst(startPoint);




        //    return roadPath;
        //}



        ///// <summary>
        ///// 获取到周围的单道;
        ///// </summary>
        //IEnumerable<CoordPack<CubicHexCoord, HexDirections, RoadInfo>> FindStraight(CubicHexCoord coord)
        //{
        //    foreach (var neighbour in Map.GetNeighbours<CubicHexCoord, HexDirections, RoadInfo>(coord))
        //    {
        //        if (ExistStraight(neighbour.Item))
        //            yield return neighbour;
        //    }
        //}

        ///// <summary>
        ///// 判断是否为单道;
        ///// </summary>
        //bool ExistStraight(RoadInfo roadInfo)
        //{
        //    if (roadInfo.RoadInfos[0].ExistRoad && !roadInfo.RoadInfos[1].ExistRoad)
        //        return true;
        //    else if (!roadInfo.ExistRoad_0 && roadInfo.ExistRoad_1)
        //        return true;
        //    else
        //        return false;
        //}


        //bool ExistBlindAlley(RoadInfo.Road road)
        //{
        //    if (road.Previous == HexDirections.Unknown && road.Next != HexDirections.Unknown)
        //        return true;
        //    else if (road.Previous != HexDirections.Unknown && road.Next == HexDirections.Unknown)
        //        return true;
        //    else
        //        return false;
        //}



        ///// <summary>
        ///// 寻找点周围的断头路;
        ///// </summary>
        //IEnumerable<CoordPack<CubicHexCoord, HexDirections, RoadInfo>> FindBlindAlley(CubicHexCoord coord)
        //{
        //    foreach (var neighbour in Map.GetNeighbours<CubicHexCoord, HexDirections, RoadInfo>(coord))
        //    {
        //        if (ExistBlindAlley(neighbour.Item))
        //            yield return neighbour;
        //    }
        //}

        ///// <summary>
        ///// 是否存在断头路;
        ///// </summary>
        //bool ExistBlindAlley(RoadInfo roadInfo)
        //{
        //    return 
        //        ExistBlindAlley(roadInfo.Previous_0, roadInfo.Next_0) ||
        //        ExistBlindAlley(roadInfo.Previous_1, roadInfo.Next_1);
        //}

        ///// <summary>
        ///// 是否任意一个为 Unknown ?
        ///// </summary>
        //bool ExistBlindAlley(HexDirections previous, HexDirections next)
        //{
        //    if (previous == HexDirections.Unknown && next != HexDirections.Unknown)
        //        return true;
        //    else if (previous != HexDirections.Unknown && next == HexDirections.Unknown)
        //        return true;
        //    else
        //        return false;
        //}

    }

}
