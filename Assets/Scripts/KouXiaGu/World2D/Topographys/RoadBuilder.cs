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
        /// 对新建的地貌物体进行构建;
        /// </summary>
        public void Build(ShortVector2 mapPoint, UnityEngine.Component instance)
        {
            Road road = instance.GetComponent<Road>();
            if (road != null)
            {
                UpdateRoadDirection(mapPoint, road);
                activeRoad.Add(mapPoint, road);
            }
        }

        public void Update(ShortVector2 mapPoint, bool changeTo)
        {
            if (activeRoad[mapPoint].HaveRoad != changeTo)
            {
                UpdateAroundRoad(mapPoint);
            }
        }

        /// <summary>
        /// 移除这个点的路径预制;
        /// </summary>
        public void Destroy(ShortVector2 mapPoint)
        {
            UpdateAroundRoad(mapPoint);
            activeRoad.Remove(mapPoint);
        }

        /// <summary>
        /// 更新这个道路的信息;
        /// </summary>
        void UpdateRoadDirection(ShortVector2 mapPoint, Road instance)
        {
            HexDirection directionmask = Map.GetAroundAndSelfMask(mapPoint, Mask);
            instance.SetState(directionmask);
        }

        /// <summary>
        /// 更新这个点周围的信息;
        /// </summary>
        void UpdateAroundRoad(ShortVector2 mapPoint)
        {
            foreach (var road in activeRoad.GetAround(mapPoint))
            {
                UpdateRoadDirection(road.Key, road.Value);
            }
        }

        bool Mask(WorldNode node)
        {
            return node.Road;
        }

    }

}
