using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;

namespace KouXiaGu.World.HumanGeography
{

    /// <summary>
    /// 城镇基础信息;
    /// </summary>
    public class Town
    {
        public Town(IReadOnlyDictionary<CubicHexCoord, MapNode> map, CubicHexCoord position)
        {
            MapNode node;
            if (map.TryGetValue(position, out node))
            {
                TownNode townNode = node.Town;
                if (townNode.Exist())
                {
                    TownID = townNode.TownID;
                    BreadthTraversal(map, position);
                }
                else
                {
                    throw new ArgumentException("该位置不存在城镇;" + position);
                }
            }
            else
            {
                throw new ArgumentException("该位置不存在;" + position);
            }
        }

        /// <summary>
        /// 城镇唯一ID;
        /// </summary>
        public int TownID { get; private set; }

        /// <summary>
        /// 城镇范围;
        /// </summary>
        public HashSet<CubicHexCoord> TownCoverage { get; private set; }

        /// <summary>
        /// 接壤的城镇;
        /// </summary>
        public HashSet<int> NeighborTowns { get; private set; }

        /// <summary>
        /// 广度遍历获取到城镇范围;
        /// </summary>
        void BreadthTraversal(IReadOnlyDictionary<CubicHexCoord, MapNode> map, CubicHexCoord position)
        {
            TownCoverage = new HashSet<CubicHexCoord>();
            NeighborTowns = new HashSet<int>();
            HashSet<CubicHexCoord> close = new HashSet<CubicHexCoord>();
            Queue<CubicHexCoord> children = new Queue<CubicHexCoord>();
            TownCoverage.Add(position);
            close.Add(position);
            children.Enqueue(position);

            while (children.Count != 0)
            {
                var child = children.Dequeue();
                foreach (CubicHexCoord neighbor in child.GetNeighbours())
                {
                    if (!close.Contains(neighbor))
                    {
                        close.Add(child);
                        MapNode node;
                        if (map.TryGetValue(neighbor, out node))
                        {
                            int neighborNodeId = node.Town.TownID;
                            if (neighborNodeId == TownID)
                            {
                                TownCoverage.Add(child);
                                children.Enqueue(neighbor);
                            }
                            else
                            {
                                NeighborTowns.Add(neighborNodeId);
                            }
                        }
                    }
                }
            }
        }
    }
}
