//using System.Collections.Generic;
//using System.Linq;
//using KouXiaGu.Grids;
//using ProtoBuf;
//using UnityEngine;

//namespace KouXiaGu.Terrain3D
//{

//    /// <summary>
//    /// 道路信息;
//    /// </summary>
//    [ProtoContract]
//    public class RoadData
//    {

//        /// <summary>
//        /// 节点不存在道路时放置的标志;
//        /// </summary>
//        const uint EMPTY_ROAD_MARK = RoadNode.EmptyMark;

//        /// <summary>
//        /// 起始的有效ID;
//        /// </summary>
//        const uint INITATING_EFFECTIVE_ID = 5;


//        RoadData()
//        {
//        }

//        /// <summary>
//        /// 初始化基本信息;
//        /// </summary>
//        public RoadData(IDictionary<CubicHexCoord, MapNode> map)
//        {
//            this.Data = map;
//            EffectiveID = INITATING_EFFECTIVE_ID;
//        }


//        /// <summary>
//        /// 当前有效的ID;
//        /// </summary>
//        [ProtoMember(1)]
//        internal uint EffectiveID { get; private set; }

//        /// <summary>
//        /// 当前查询的数据;
//        /// </summary>
//        public IDictionary<CubicHexCoord, MapNode> Data { get; internal set; }


//        [ProtoAfterDeserialization]
//        void ProtoAfterDeserialization()
//        {
//            if (EffectiveID < INITATING_EFFECTIVE_ID)
//                EffectiveID = INITATING_EFFECTIVE_ID;
//        }

//        /// <summary>
//        /// 重置道路信息(仅在地图清空时调用);
//        /// </summary>
//        internal void Reset()
//        {
//            EffectiveID = INITATING_EFFECTIVE_ID;
//        }

//        /// <summary>
//        /// 向这个坐标添加道路标记,若已存在则返回 false;
//        /// </summary>
//        public bool Create(CubicHexCoord coord)
//        {
//            MapNode node = Data[coord];

//            if (!node.Road.ExistRoad())
//            {
//                node.Road.ID = GetNewID();
//                Data[coord] = node;
//                return true;
//            }
//            return false;
//        }

//        /// <summary>
//        /// 获取到一个新的ID;
//        /// </summary>
//        uint GetNewID()
//        {
//            return EffectiveID++;
//        }

//        /// <summary>
//        /// 清除这个坐标的道路标记,若不存在则返回 false;
//        /// </summary>
//        public bool Destroy(CubicHexCoord coord)
//        {
//            MapNode node = Data[coord];

//            if (node.Road.ExistRoad())
//            {
//                node.Road.ID = EMPTY_ROAD_MARK;
//                Data[coord] = node;
//                return true;
//            }
//            return false;
//        }


//        /// <summary>
//        /// 获取到这个点通向周围的像素路径;
//        /// </summary>
//        public IEnumerable<Vector3[]> FindPixelPaths(CubicHexCoord target)
//        {
//            IEnumerable<CubicHexCoord[]> paths = FindPaths(target);
//            return Convert(paths);
//        }

//        /// <summary>
//        /// 转换 地图坐标 到 像素坐标;
//        /// </summary>
//        public IEnumerable<Vector3[]> Convert(IEnumerable<CubicHexCoord[]> paths)
//        {
//            return paths.Select(delegate (CubicHexCoord[] path)
//            {
//                Vector3[] newPath = path.Select(coord => coord.GetTerrainPixel()).ToArray();
//                return newPath;
//            });
//        }

//        /// <summary>
//        /// 获取到这个点通向周围的路径,若不存在节点则不返回;
//        /// </summary>
//        public IEnumerable<CubicHexCoord[]> FindPaths(CubicHexCoord target)
//        {
//            MapNode node;
//            if (Data.TryGetValue(target, out node))
//            {
//                RoadNode targetRoadInfo = node.Road;

//                if (targetRoadInfo.ExistRoad())
//                {
//                    foreach (var neighbour in Data.GetNeighbours<CubicHexCoord, HexDirections, MapNode>(target))
//                    {
//                        RoadNode neighbourRoadInfo = neighbour.Item.Road;

//                        if (neighbourRoadInfo.ExistRoad() && neighbourRoadInfo.ID > targetRoadInfo.ID)
//                        {
//                            CubicHexCoord[] path = new CubicHexCoord[4];

//                            path[0] = MinNeighbour(target, neighbour.Point);
//                            path[1] = target;
//                            path[2] = neighbour.Point;
//                            path[3] = MinNeighbour(neighbour.Point, target);

//                            yield return path;
//                        }
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// 获取到ID值最小的邻居节点,若无法找到则返回 target;
//        /// </summary>
//        CubicHexCoord MinNeighbour(
//            CubicHexCoord target,
//            CubicHexCoord eliminate)
//        {
//            bool isFind = false;
//            uint minID = uint.MaxValue;
//            CubicHexCoord min = default(CubicHexCoord);

//            foreach (var neighbour in Data.GetNeighbours<CubicHexCoord, HexDirections, MapNode>(target))
//            {
//                RoadNode neighbourRoadInfo = neighbour.Item.Road;

//                if (neighbour.Point != eliminate &&
//                    neighbourRoadInfo.ExistRoad() &&
//                    neighbourRoadInfo.ID < minID)
//                {
//                    isFind = true;
//                    minID = neighbourRoadInfo.ID;
//                    min = neighbour.Point;
//                }
//            }

//            if (isFind)
//                return min;
//            else
//                return target;
//        }


//    }

//}
