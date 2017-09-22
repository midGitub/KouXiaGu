using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.Grids;
using JiongXiaGu.World.Map;

namespace JiongXiaGu.World.HumanGeography
{

    /// <summary>
    /// 城镇范围信息;
    /// </summary>
    public class TownRange
    {
        public TownRange(int townID, MapData map)
        {
            TownID = townID;
            this.map = map;
            TownCoverage = new HashSet<CubicHexCoord>();
            NeighborTowns = new HashSet<int>();
            Reacquire();
        }

        public int TownID { get; private set; }
        MapData map;

        /// <summary>
        /// 城镇范围(只读);
        /// </summary>
        public HashSet<CubicHexCoord> TownCoverage { get; private set; }

        /// <summary>
        /// 接壤的城镇(只读);
        /// </summary>
        public HashSet<int> NeighborTowns { get; private set; }

        /// <summary>
        /// 重新获取到城镇范围;
        /// </summary>
        public void Reacquire()
        {
            CubicHexCoord position = map.TownCorePositions[TownID];
            BreadthTraversal(map.Data, position);
        }

        /// <summary>
        /// 广度遍历获取到城镇范围;
        /// </summary>
        void BreadthTraversal(IDictionary<CubicHexCoord, MapNode> map, CubicHexCoord position)
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
