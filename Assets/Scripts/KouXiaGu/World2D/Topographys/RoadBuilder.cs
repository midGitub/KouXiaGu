using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World2D.Map;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 场景道路构建;
    /// </summary>
    public class RoadBuilder
    {
        public RoadBuilder(IMap<ShortVector2, WorldNode> map)
        {
            this.Map = map;
        }

        IMap<ShortVector2, WorldNode> Map { get; set; }
        /// <summary>
        /// 已经存在于场景中的道路实例;
        /// </summary>
        MapCollection<Road> activeRoad = new MapCollection<Road>();


        /// <summary>
        /// 在地貌实例上寻找道路组件并对其进行更新;
        /// </summary>
        /// <param name="topography"></param>
        public void UpdateRoad(ShortVector2 mapPoint, WorldNode worldNode, Road instance)
        {
            if (worldNode.Road)
            {
                HexDirection directionmask = Map.GetHexDirectionMask(mapPoint, Mask);
                instance.SetState(worldNode.Road, directionmask);
            }
        }

        bool Mask(WorldNode node)
        {
            return node.Road;
        }

    }

}
