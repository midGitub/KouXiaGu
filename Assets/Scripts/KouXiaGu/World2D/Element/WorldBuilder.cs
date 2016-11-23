using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;
using KouXiaGu.World2D.Map;
using System.Collections;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 监视地图变化,和初始化地图;
    /// </summary>
    public class WorldBuilder : UnitySingleton<WorldBuilder>, IStartGameEvent
    {

        [SerializeField]
        Transform target;
        [SerializeField]
        ShortVector2 buildUpdateRange = new ShortVector2(20, 20);
        [SerializeField]
        Vector2 buildUpdateCheckRange = new Vector2(5, 5);

        IMap<ShortVector2, WorldNode> worldMap;
        List<ShortVector2> loadedPoint;
        NodeChangeReporter<WorldNode> nodeChangeReporter;
        Vector2 lastBuildUpdatePoint = new Vector2(float.MaxValue, float.MaxValue);

        [ShowOnlyProperty]
        public bool IsBuilding { get; private set; }

        Vector3 targetPlanePoint
        {
            get { return target.position; }
        }
        ShortVector2 targetMapPoint
        {
            get { return WorldConvert.PlaneToHexPair(targetPlanePoint); }
        }
        public IObservable<MapNodeState<WorldNode>> ObserveBuilderNode
        {
            get { return nodeChangeReporter; }
        }


        void Awake()
        {
            loadedPoint = new List<ShortVector2>();
            nodeChangeReporter = new NodeChangeReporter<WorldNode>();
        }

        void Start()
        {
            worldMap = WorldMap.GetInstance.Map;
        }

        IEnumerator IConstruct<BuildGameData>.Construction(BuildGameData item)
        {
            while (!WorldMap.GetInstance.IsReady)
            {
                yield return null;
            }

            //当地图节点发生变化时调用;
            WorldMap.GetInstance.observeChanges.
                Where(nodeState => WithinRange(buildUpdateRange, targetMapPoint, nodeState.MapPoint)).
                Subscribe(WorldNodeChange);

            //当超出上次更新范围则更新建筑;
            target.ObserveEveryValueChanged(_ => target.position).
                Where(_ => !WithinRange(buildUpdateCheckRange, lastBuildUpdatePoint, targetPlanePoint)).
                Subscribe(BuildUpdate);

            IsBuilding = true;
        }

        /// <summary>
        /// 这个点是否在读取的范围内?
        /// </summary>
        bool WithinRange(ShortVector2 range, ShortVector2 centerPoint, ShortVector2 current)
        {
            ShortVector2 southwestPoint = new ShortVector2(centerPoint.x - range.x, centerPoint.y - range.y);
            ShortVector2 northeastPoint = new ShortVector2(centerPoint.x + range.x, centerPoint.y + range.y);

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


        void WorldNodeChange(MapNodeState<WorldNode> node)
        {
            nodeChangeReporter.NodeDataUpdate(ChangeType.Update, node.MapPoint, node.WorldNode);
        }

        void BuildUpdate(Vector3 targetPlanePoint)
        {
            ShortVector2 targetMapPoint = WorldConvert.PlaneToHexPair(targetPlanePoint);
            WorldMap.GetInstance.OnMapDataUpdate(targetPlanePoint, targetMapPoint);
            UpdateWorldRes(targetMapPoint);

            lastBuildUpdatePoint = targetPlanePoint;
        }

        /// <summary>
        /// 更新范围内需要更新的点;
        /// </summary>
        void UpdateWorldRes(ShortVector2 mapPoint)
        {
            ShortVector2[] newBlock = GetAllPoint(mapPoint).ToArray();
            ShortVector2[] unloadPoints = loadedPoint.Except(newBlock).ToArray();
            ShortVector2[] loadPoints = newBlock.Except(loadedPoint).ToArray();

            Load(loadPoints);
            Unload(unloadPoints);
        }

        /// <summary>
        /// 读取到这些位置的资源;
        /// </summary>
        void Load(IEnumerable<ShortVector2> mapPoints)
        {
            foreach (var point in mapPoints)
            {
                UpdateBuildRes(ChangeType.Add, point);
                loadedPoint.Add(point);
            }
        }

        /// <summary>
        /// 卸载这些区域的资源;
        /// </summary>
        void Unload(IEnumerable<ShortVector2> mapPoints)
        {
            foreach (var point in mapPoints)
            {
                UpdateBuildRes(ChangeType.Remove, point);
                loadedPoint.Remove(point);
            }
        }

        void UpdateBuildRes(ChangeType eventType, ShortVector2 mapPoint)
        {
            WorldNode worldNode;
            if (worldMap.TryGetValue(mapPoint, out worldNode))
            {
                nodeChangeReporter.NodeDataUpdate(eventType, mapPoint, worldNode);
            }
        }

        /// <summary>
        /// 获取到需要读取的点;
        /// </summary>
        IEnumerable<ShortVector2> GetAllPoint(ShortVector2 centerPoint)
        {
            ShortVector2 southwestPoint = GetSouthwestPoint(centerPoint);
            ShortVector2 northeastPoint = GetNortheastPoint(centerPoint);

            return ShortVector2.Range(southwestPoint, northeastPoint);
        }

        /// <summary>
        /// 获取到范围内最西南角的点;
        /// </summary>
        ShortVector2 GetSouthwestPoint(ShortVector2 centerPoint)
        {
            centerPoint.x -= buildUpdateRange.x;
            centerPoint.y -= buildUpdateRange.y;
            return centerPoint;
        }

        /// <summary>
        /// 获取到范围内最东北角的点;
        /// </summary>
        ShortVector2 GetNortheastPoint(ShortVector2 centerPoint)
        {
            centerPoint.x += buildUpdateRange.x;
            centerPoint.y += buildUpdateRange.y;
            return centerPoint;
        }

    }

}
