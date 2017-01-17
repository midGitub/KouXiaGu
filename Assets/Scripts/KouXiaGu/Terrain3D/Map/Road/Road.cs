using System.Collections.Generic;
using KouXiaGu.Grids;
using ProtoBuf;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 道路信息;
    /// </summary>
    [ProtoContract]
    public class Road
    {

        /// <summary>
        /// 节点不存在道路时放置的标志;
        /// </summary>
        const uint EMPTY_ROAD_MARK = 0;

        /// <summary>
        /// 起始的有效ID;
        /// </summary>
        const uint INITATING_EFFECTIVE_ID = 5;


        Road()
        {
        }

        /// <summary>
        /// 初始化基本信息;
        /// </summary>
        public Road(IDictionary<CubicHexCoord, TerrainNode> map)
        {
            this.Data = map;
            EffectiveID = INITATING_EFFECTIVE_ID;
        }


        /// <summary>
        /// 当前有效的ID;
        /// </summary>
        [ProtoMember(1)]
        internal uint EffectiveID { get; private set; }

        /// <summary>
        /// 进行编辑的地图;
        /// </summary>
        internal IDictionary<CubicHexCoord, TerrainNode> Data { get; set; }


        /// <summary>
        /// 向这个坐标添加道路标记;
        /// </summary>
        public void CreateRoad(CubicHexCoord coord)
        {
            TerrainNode node = Data[coord];

            if (!IsHaveRoad(node))
            {
                node.RoadInfo.ID = GetNewID();
                Data[coord] = node;
            }
        }

        /// <summary>
        /// 获取到一个新的ID;
        /// </summary>
        uint GetNewID()
        {
            return EffectiveID++;
        }

        /// <summary>
        /// 清除这个坐标的道路标记;
        /// </summary>
        public void DestroyRoad(CubicHexCoord coord)
        {
            TerrainNode node = Data[coord];

            if (IsHaveRoad(node))
            {
                node.RoadInfo.ID = EMPTY_ROAD_MARK;
                Data[coord] = node;
            }
        }

        /// <summary>
        /// 是否存在道路;
        /// </summary>
        public bool IsHaveRoad(RoadInfo road)
        {
            return road.ID != EMPTY_ROAD_MARK;
        }


        /// <summary>
        /// 获取到这个点通向周围的路径;
        /// </summary>
        public IEnumerable<CubicHexCoord[]> FindPaths(CubicHexCoord target)
        {
            RoadInfo targetRoadInfo = Data[target].RoadInfo;

            if(IsHaveRoad(targetRoadInfo))
            {
                foreach (var neighbour in Data.GetNeighbours<CubicHexCoord, HexDirections, TerrainNode>(target))
                {
                    RoadInfo neighbourRoadInfo = neighbour.Item.RoadInfo;

                    if (IsHaveRoad(neighbourRoadInfo) && neighbourRoadInfo.ID > targetRoadInfo.ID)
                    {
                        CubicHexCoord[] path = new CubicHexCoord[4];

                        path[0] = MinNeighbour(target, neighbour.Point);
                        path[1] = target;
                        path[2] = neighbour.Point;
                        path[4] = MinNeighbour(neighbour.Point, target);

                        yield return path;
                    }
                }
            }
        }

        /// <summary>
        /// 获取到ID值最小的邻居节点,若无法找到则返回 target;
        /// </summary>
        CubicHexCoord MinNeighbour(
            CubicHexCoord target,
            CubicHexCoord eliminate)
        {
            bool isFind = false;
            uint minID = uint.MaxValue;
            CubicHexCoord min = default(CubicHexCoord);

            foreach (var neighbour in Data.GetNeighbours<CubicHexCoord, HexDirections, TerrainNode>(target))
            {
                RoadInfo neighbourRoadInfo = neighbour.Item.RoadInfo;

                if (neighbour.Point != eliminate &&
                    IsHaveRoad(neighbourRoadInfo) &&
                    neighbourRoadInfo.ID < minID)
                {
                    isFind = true;
                    minID = neighbourRoadInfo.ID;
                    min = neighbour.Point;
                }
            }

            if (isFind)
                return min;
            else
                return target;
        }

    }

}
