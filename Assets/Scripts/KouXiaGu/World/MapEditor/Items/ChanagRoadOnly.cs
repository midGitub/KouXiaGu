using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.OperationRecord;
using KouXiaGu.World.Map;

namespace KouXiaGu.World.MapEditor
{

    /// <summary>
    /// 改变节点道路信息;
    /// </summary>
    public class ChangeRoadOnly : IEditOperation
    {
        public ChangeRoadOnly(GameMap gameMap, int roadType)
        {
            this.Data = gameMap;
        }

        public GameMap Data { get; private set; }
        public int RoadType { get; private set; }

        IDictionary<CubicHexCoord, MapNode> map
        {
            get { return Data.Map; }
        }

        public void PositionUpdate(CubicHexCoord position)
        {
        }

        public IVoidable Perform(CubicHexCoord position)
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
