using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Grids;
using KouXiaGu.Collections;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 道路创建；
    /// </summary>
    public class RoadBuilder : MonoBehaviour
    {
        RoadBuilder() { }


        /// <summary>
        /// 最多两种道路重叠，组成 丁字路口，Y字路口，十字路口，道路;
        /// </summary>
        public const int MAX_ROAD_COUNT = 2;


        public IDictionary<CubicHexCoord, TerrainNode> Map { get; private set; }

        Dictionary<CubicHexCoord, List<int>> roadRecords;


        RoadNode GetRoad(CubicHexCoord coord)
        {
            return Map[coord].RoadInfo;
        }

        bool Contains(CubicHexCoord coord)
        {
            return Map.ContainsKey(coord);
        }

        void Awake()
        {
            roadRecords = new Dictionary<CubicHexCoord, List<int>>();
        }

        void Clear()
        {
            Map.Clear();
            roadRecords.Clear();
        }


        /// <summary>
        /// 获取到地图内所以的道路路径点；
        /// </summary>
        public IEnumerable<LinkedList<CubicHexCoord>> Create(IDictionary<CubicHexCoord, TerrainNode> map)
        {
            this.Map = map;

            foreach (var pair in map)
            {
                if (pair.Value.RoadInfo.Exist)
                {
                    foreach (var road in pair.Value.RoadInfo.GetRoadInfos())
                    {
                        List<int> builtRoad;
                        if (roadRecords.TryGetValue(pair.Key, out builtRoad))
                        {
                            if (builtRoad.Contains(id => id == road.ID))
                                break;
                        }

                        var path = CreatePath(pair.Key, road);
                        foreach (var item in path)
                        {
                            roadRecords.Add(item, road.ID, MAX_ROAD_COUNT);
                        }

                    }
                }
            }

            Clear();
            throw new NotImplementedException();
        }


        LinkedList<CubicHexCoord> CreatePath(CubicHexCoord targetPoint, Road targetRoad)
        {
            if (targetRoad.Next == HexDirections.Unknown && targetRoad.Previous == HexDirections.Unknown)
                throw new ArgumentException("空的道路节点;");

            LinkedList<CubicHexCoord> path = new LinkedList<CubicHexCoord>();
            path.AddFirst(targetPoint);

            Road tempRoad = targetRoad;
            while (true)
            {
                if (tempRoad.Next == HexDirections.Unknown)
                {
                    targetPoint = targetPoint.GetOppositeDirection(tempRoad.Previous);
                    path.AddLast(targetPoint);
                    break;
                }
                else
                {
                    targetPoint = targetPoint.GetDirection(tempRoad.Next);
                    path.AddLast(targetPoint);
                }

                if (!Contains(targetPoint))
                {
                    break;
                }
                if (!GetRoad(targetPoint).TryGetValue(targetRoad.ID, out tempRoad))
                {
                    throw new Exception("道路不以 Unknown 结尾;");
                }
            }

            targetPoint = path.First.Value;
            while (true)
            {
                if (tempRoad.Previous == HexDirections.Unknown)
                {
                    targetPoint = targetPoint.GetOppositeDirection(tempRoad.Next);
                    path.AddFirst(targetPoint);
                    break;
                }
                else
                {
                    targetPoint = targetPoint.GetDirection(tempRoad.Previous);
                    path.AddFirst(targetPoint);
                }

                if (!Contains(targetPoint))
                {
                    break;
                }
                if (!GetRoad(targetPoint).TryGetValue(targetRoad.ID, out tempRoad))
                {
                    throw new Exception("道路不以 Unknown 结尾;");
                }
            }

            return path;
        }


        class RoadMap
        {

            public IDictionary<CubicHexCoord, TerrainNode> Map { get; set; }

            Dictionary<CubicHexCoord, List<int>> roadRecords;


            public RoadNode this[CubicHexCoord coord]
            {
                get { return Map[coord].RoadInfo; }
            }

            public bool Contains(CubicHexCoord coord)
            {
                return Map.ContainsKey(coord);
            }

            public void Clear()
            {
                Map.Clear();
            }

        }

    }

}
