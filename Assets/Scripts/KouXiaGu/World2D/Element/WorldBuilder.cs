using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using KouXiaGu.World2D.Map;
using System.Collections;
using KouXiaGu.Grids;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 监视地图变化,和初始化地图;
    /// </summary>
    public class WorldBuilder : UnitySingleton<WorldBuilder>, IStartGameEvent, IQuitGameEvent
    {

        [SerializeField]
        Transform target;
        [SerializeField]
        RectCoord buildUpdateRange = new RectCoord(20, 20);
        [SerializeField]
        Vector2 buildUpdateCheckRange = new Vector2(5, 5);

        IDisposable WorldNodeChangeEvent;
        IDisposable BuildEvent;
        WorldMapData worldMap;
        IHexMap<RectCoord, WorldNode> Map;
        HashSet<RectCoord> loadedPoint;
        NodeChangeReporter<WorldNode> nodeChangeReporter;
        Vector2 lastBuildUpdatePoint;

        [ShowOnlyProperty]
        public bool IsBuilding { get; private set; }

        Vector3 targetPlanePoint
        {
            get { return target.position; }
        }
        RectCoord targetMapPoint
        {
            get { return WorldConvert.PlaneToHexPair(targetPlanePoint); }
        }
        public IObservable<MapNodeState<WorldNode>> ObserveBuilderNode
        {
            get { return nodeChangeReporter; }
        }


        void Awake()
        {
            loadedPoint = new HashSet<RectCoord>();
            nodeChangeReporter = new NodeChangeReporter<WorldNode>();
        }

        void Start()
        {
            worldMap = WorldMapData.GetInstance;
            Map = worldMap.Map;
        }

        IEnumerator IConstruct2<BuildGameData>.Prepare(BuildGameData item)
        {
            yield break;
        }

        /// <summary>
        /// 当开始游戏时调用;
        /// </summary>
        IEnumerator IConstruct2<BuildGameData>.Construction(BuildGameData item)
        {
            lastBuildUpdatePoint = new Vector2(float.MaxValue, float.MaxValue);

            //当地图节点发生变化时调用;
            WorldNodeChangeEvent = WorldMapData.GetInstance.observeChanges.
                Where(nodeState => WithinRange(buildUpdateRange, targetMapPoint, nodeState.MapPoint)).
                Subscribe(WorldNodeChange);

            //当超出上次更新范围则更新建筑;
            BuildEvent = target.ObserveEveryValueChanged(_ => target.position).
                Where(_ => !WithinRange(buildUpdateCheckRange, lastBuildUpdatePoint, targetPlanePoint)).
                Subscribe(BuildUpdate);

            IsBuilding = true;
            yield break;
        }


        IEnumerator IConstruct1<QuitGameData>.Construction(QuitGameData item)
        {
            WorldNodeChangeEvent.Dispose();
            BuildEvent.Dispose();
            UnloadAll();
            yield break;
        }


        /// <summary>
        /// 这个点是否在读取的范围内?
        /// </summary>
        bool WithinRange(RectCoord range, RectCoord centerPoint, RectCoord current)
        {
            RectCoord southwestPoint = new RectCoord(centerPoint.x - range.x, centerPoint.y - range.y);
            RectCoord northeastPoint = new RectCoord(centerPoint.x + range.x, centerPoint.y + range.y);

            return current.x > southwestPoint.x && current.y > southwestPoint.y &&
                current.x < northeastPoint.x && current.y < northeastPoint.y;
        }

        /// <summary>
        /// 这个点是否在读取的范围内?
        /// </summary>
        bool WithinRange(Vector2 range, Vector2 centerPoint, Vector2 current)
        {
            Vector2 southwestPoint = new Vector2(centerPoint.x - range.x, centerPoint.y - range.y);
            Vector2 northeastPoint = new Vector2(centerPoint.x + range.x, centerPoint.y + range.y);

            return current.x > southwestPoint.x && current.y > southwestPoint.y &&
                current.x < northeastPoint.x && current.y < northeastPoint.y;
        }

        /// <summary>
        /// 当地图节点存在变化时调用;
        /// </summary>
        /// <param name="node"></param>
        void WorldNodeChange(MapNodeState<WorldNode> node)
        {
            nodeChangeReporter.NodeDataUpdate(node.EventType, node.MapPoint, node.WorldNode);
        }

        /// <summary>
        /// 当目标位置发生变化时调用;
        /// </summary>
        /// <param name="targetPlanePoint"></param>
        void BuildUpdate(Vector3 targetPlanePoint)
        {
            RectCoord targetMapPoint = WorldConvert.PlaneToHexPair(targetPlanePoint);

            worldMap.OnMapDataUpdate(targetPlanePoint, targetMapPoint); //更新主地图的信息;

            UpdateWorldRes(targetMapPoint);

            lastBuildUpdatePoint = targetPlanePoint;
        }

        /// <summary>
        /// 更新范围内需要更新的点;
        /// </summary>
        void UpdateWorldRes(RectCoord mapPoint)
        {
            HashSet<RectCoord> newPoints = new HashSet<RectCoord>(GetAllPoint(mapPoint));

            HashSet<RectCoord> loadPoints = new HashSet<RectCoord>(newPoints);
            loadPoints.ExceptWith(loadedPoint);

            loadedPoint.ExceptWith(newPoints);
            HashSet<RectCoord> unloadPoints = loadedPoint;

            loadedPoint = newPoints;

            Load(loadPoints);
            Unload(unloadPoints);
        }

        /// <summary>
        /// 卸载所有已经读取的资源;
        /// </summary>
        void UnloadAll()
        {
            Unload(loadedPoint.ToArray());
            loadedPoint.Clear();
        }

        /// <summary>
        /// 读取到这些位置的资源;
        /// </summary>
        void Load(IEnumerable<RectCoord> mapPoints)
        {
            foreach (var point in mapPoints)
            {
                NotifyObservers(ChangeType.Add, point);
                loadedPoint.Add(point);
            }
        }

        /// <summary>
        /// 卸载这些区域的资源;
        /// </summary>
        void Unload(IEnumerable<RectCoord> mapPoints)
        {
            foreach (var point in mapPoints)
            {
                NotifyObservers(ChangeType.Remove, point);
                loadedPoint.Remove(point);
            }
        }

        /// <summary>
        /// 通知观察程序更新信息;
        /// </summary>
        void NotifyObservers(ChangeType eventType, RectCoord mapPoint)
        {
            WorldNode worldNode;
            if (Map.TryGetValue(mapPoint, out worldNode))
            {
                nodeChangeReporter.NodeDataUpdate(eventType, mapPoint, worldNode);
            }
        }

        /// <summary>
        /// 获取到需要读取的点;
        /// </summary>
        IEnumerable<RectCoord> GetAllPoint(RectCoord centerPoint)
        {
            RectCoord southwestPoint = GetSouthwestPoint(centerPoint);
            RectCoord northeastPoint = GetNortheastPoint(centerPoint);

            return RectCoord.Range(southwestPoint, northeastPoint);
        }

        /// <summary>
        /// 获取到范围内最西南角的点;
        /// </summary>
        RectCoord GetSouthwestPoint(RectCoord centerPoint)
        {
            centerPoint.x -= buildUpdateRange.x;
            centerPoint.y -= buildUpdateRange.y;
            return centerPoint;
        }

        /// <summary>
        /// 获取到范围内最东北角的点;
        /// </summary>
        RectCoord GetNortheastPoint(RectCoord centerPoint)
        {
            centerPoint.x += buildUpdateRange.x;
            centerPoint.y += buildUpdateRange.y;
            return centerPoint;
        }
    }

}
