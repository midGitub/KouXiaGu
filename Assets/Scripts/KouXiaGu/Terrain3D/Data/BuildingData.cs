using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{


    public class BuildingData
    {
        /// <summary>
        /// 节点不存在道路时放置的标志;
        /// </summary>
        const int EMPTY_MARK = 0;


        BuildingData()
        {
        }

        public BuildingData(IDictionary<CubicHexCoord, TerrainNode> data)
        {
            this.Data = data;
        }

        /// <summary>
        /// 当前查询的数据;
        /// </summary>
        public IDictionary<CubicHexCoord, TerrainNode> Data { get; internal set; }


        /// <summary>
        /// 更新这个节点的内容;
        /// </summary>
        public bool Update(CubicHexCoord coord, int id)
        {
            TerrainNode node;
            if (Data.TryGetValue(coord, out node))
            {
                if (node.Building.ID != id)
                {
                    node.Building.ID = id;
                    Data[coord] = node;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 清除这个坐标的建筑物信息;
        /// </summary>
        public bool Destroy(CubicHexCoord coord)
        {
            TerrainNode node;
            if (Data.TryGetValue(coord, out node))
            {
                if (node.Building.ID != EMPTY_MARK)
                {
                    node.Building.ID = EMPTY_MARK;
                    Data[coord] = node;
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 是否存在建筑物?
        /// </summary>
        public bool Exist(CubicHexCoord coord)
        {
            TerrainNode node;
            if (Data.TryGetValue(coord, out node))
            {
                return Exist(node.Building);
            }
            return false;
        }

        /// <summary>
        /// 是否存在建筑物?
        /// </summary>
        public bool Exist(BuildingNode building)
        {
            return building.ID != EMPTY_MARK;
        }

        /// <summary>
        /// 尝试获取到这个位置的道路信息;
        /// </summary>
        public bool TryGetValue(CubicHexCoord coord, out BuildingNode building)
        {
            TerrainNode node;
            if (Data.TryGetValue(coord, out node))
            {
                building = node.Building;
                if (Exist(building))
                    return true;
            }
            building = default(BuildingNode);
            return false;
        }

    }

}
