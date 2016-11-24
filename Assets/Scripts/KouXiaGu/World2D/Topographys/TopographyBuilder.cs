using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World2D.Map;
using UnityEngine;
using UniRx;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 维护场景地貌实例;
    /// </summary>
    public class TopographyBuilder : MonoBehaviour
    {
        protected TopographyBuilder() { }


        TopographiessData topographiessData;

        /// <summary>
        /// 记录已经实例化到场景的物体;
        /// </summary>
        MapCollection<Topography> activeWorldNode;
        MapCollection<Road> activeRoad;

        IMap<ShortVector2, WorldNode> WorldMap;

        void Awake()
        {
            activeWorldNode = new MapCollection<Topography>();
            activeRoad = new MapCollection<Road>();
        }

        void Start()
        {
            WorldBuilder.GetInstance.ObserveBuilderNode.Subscribe(UpdateScene);
            topographiessData = TopographiessData.GetInstance;
            WorldMap = WorldMapData.GetInstance.Map;
        }


        /// <summary>
        /// 更新场景内的实例;
        /// </summary>
        void UpdateScene(MapNodeState<WorldNode> nodeState)
        {
            switch (nodeState.EventType)
            {
                case ChangeType.Add:
                    BuildTopography(nodeState);
                    break;

                case ChangeType.Remove:
                    DestroyTopography(nodeState);
                    break;

                case ChangeType.Update:
                    UpdateTopography(nodeState);
                    break;
            }
        }


        /// <summary>
        /// 将地貌信息实例化到场景内对应位置;
        /// </summary>
        void BuildTopography(MapNodeState<WorldNode> nodeState)
        {
            ShortVector2 mapPoint = nodeState.MapPoint;
            BuildTopography(mapPoint, nodeState.WorldNode);
        }

        /// <summary>
        /// 将这个位置存在的地貌销毁;
        /// </summary>
        void DestroyTopography(MapNodeState<WorldNode> nodeState)
        {
            ShortVector2 mapPoint = nodeState.MapPoint;
            DestroyTopography(mapPoint);
        }

        /// <summary>
        /// 更新这个位置的地貌信息;
        /// </summary>
        void UpdateTopography(MapNodeState<WorldNode> nodeState)
        {
            ShortVector2 mapPoint = nodeState.MapPoint;
            UpdateTopography(mapPoint, nodeState.WorldNode);
        }



        void BuildTopography(ShortVector2 mapPoint, WorldNode worldNode)
        {
            int topographyID = worldNode.Topography;

            Topography topography = Instantiate(mapPoint, topographyID);
            BuildRoad(mapPoint, worldNode, topography);

            activeWorldNode.Add(mapPoint, topography);
        }

        void BuildRoad(ShortVector2 mapPoint, WorldNode worldNode, UnityEngine.Component instance)
        {
            if (worldNode.Road)
            {
                Road road = instance.GetComponent<Road>();
                if (road != null)
                {
                    activeRoad.Add(mapPoint, road);
                    UpdateRoadDirection(mapPoint, road);
                    UpdateAroundRoadDirection(mapPoint);
                }
            }
        }


        /// <summary>
        /// 销毁这个地貌;
        /// </summary>
        void DestroyTopography(ShortVector2 mapPoint)
        {
            Topography topography = activeWorldNode[mapPoint];
            DestroyTopography(mapPoint, topography);
        }
        /// <summary>
        /// 销毁这个地貌;
        /// </summary>
        void DestroyTopography(ShortVector2 mapPoint, Topography topography)
        {
            DestroyRoad(mapPoint);
            Destroy(topography);
            activeWorldNode.Remove(mapPoint);
        }
        /// <summary>
        /// 移除这个点的路径预制;
        /// </summary>
        void DestroyRoad(ShortVector2 mapPoint)
        {
            activeRoad.Remove(mapPoint);
            UpdateAroundRoadDirection(mapPoint);
        }


        void UpdateTopography(ShortVector2 mapPoint, WorldNode worldNode)
        {
            Topography original = activeWorldNode[mapPoint];
            int topographyID = worldNode.Topography;

            if (original.ID != topographyID)
            {
                DestroyTopography(mapPoint, original);
                BuildTopography(mapPoint, worldNode);
            }
            UpdateRoad(mapPoint, worldNode);
        }

        void UpdateRoad(ShortVector2 mapPoint, WorldNode worldNode)
        {
            if (activeRoad[mapPoint].HaveRoad != worldNode.Road)
            {
                UpdateAroundRoadDirection(mapPoint);
            }
        }


        /// <summary>
        /// 更新这个道路的信息;
        /// </summary>
        void UpdateRoadDirection(ShortVector2 mapPoint, Road instance)
        {
            HexDirection directionmask = WorldMap.GetAroundAndSelfMask(mapPoint, node => node.Road);
            instance.SetState(directionmask);
        }
        /// <summary>
        /// 更新这个点周围的信息;
        /// </summary>
        void UpdateAroundRoadDirection(ShortVector2 mapPoint)
        {
            foreach (var road in activeRoad.GetAround(mapPoint))
            {
                UpdateRoadDirection(road.Key, road.Value);
            }
        }


        /// <summary>
        /// 获取到地貌预制,若未定义则返回 编号为 0 的预制,并输出警告;
        /// </summary>
        Topography GetTopographyPrefab(int topographyID, ShortVector2 mapPoint)
        {
            Topography topographyPrefab;
            try
            {
                topographyPrefab = topographiessData.GetPrefabWithID(topographyID);
            }
            catch (KeyNotFoundException)
            {
                Debug.LogWarning(mapPoint.ToString() + "获取不存在的编号为 " + topographyID + " 的地貌信息;");
                topographyPrefab = topographiessData.GetPrefabWithID(0);
            }
            return topographyPrefab;
        }


        /// <summary>
        /// 实例化;
        /// </summary>
        Topography Instantiate(ShortVector2 mapPoint, int topographyID)
        {
            Topography topographyPrefab = GetTopographyPrefab(topographyID, mapPoint);
            Vector2 planePoint = WorldConvert.MapToHex(mapPoint);

            return Instantiate(topographyPrefab, planePoint);
        }
        /// <summary>
        /// 实例化;
        /// </summary>
        Topography Instantiate(Topography topographyPrefab, Vector2 position)
        {
            Topography topographyObject = GameObject.Instantiate(topographyPrefab, position, Quaternion.identity) as Topography;
            return topographyObject;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        void Destroy(Topography topographyObject)
        {
            GameObject.Destroy(topographyObject.gameObject);
        }

    }

}
