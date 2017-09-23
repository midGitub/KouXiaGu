using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.Grids;
using JiongXiaGu.VoidableOperations;
using JiongXiaGu.World.Map;

namespace JiongXiaGu.Terrain3D.MapEditor
{

    /// <summary>
    /// 改变节点道路信息;
    /// </summary>
    public class ChangeRoadOnly : NodeEditer
    {
        public ChangeRoadOnly(WorldMap gameMap, int roadType) : base(gameMap)
        {
            RoadType = roadType;
        }

        public int RoadType { get; private set; }

        public override VoidableOperation Perform(CubicHexCoord position)
        {
            MapNode node;
            if (map.TryGetValue(position, out node))
            {
                RoadNode road = node.Road;
                if (road.Exist())
                {
                    node.Road = road.Destroy();
                }
                else
                {
                    node.Road = road.Update(Data, RoadType);
                }
            }
            return map.VoidableSetValue(position, node);
        }
    }
}
