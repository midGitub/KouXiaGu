using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World2D.Map;
using UniRx;
using UnityEngine;

namespace KouXiaGu.World2D
{

    [DisallowMultipleComponent]
    public class TopographyMap : UnitySingleton<TopographyMap>
    {

        /// <summary>
        /// 地貌信息;
        /// </summary>
        [SerializeField]
        TopographiessData topographiessData;

        /// <summary>
        /// 记录已经实例化到场景的物体;
        /// </summary>
        Dictionary<ShortVector2, TopographyNode> activeWorldNode;

        public TopographiessData TopographiessData
        {
            get { return topographiessData; }
        }

        void Awake()
        {
            activeWorldNode = new Dictionary<ShortVector2, TopographyNode>();
        }

        void Start()
        {
            WorldBuilder.GetInstance.ObserveBuilderNode.Subscribe(UpdateScene);
            topographiessData.Start();
        }

        void UpdateScene(MapNodeState<WorldNode> nodeState)
        {
            if (nodeState.EventType == ChangeType.Add)
            {
                BuildTopography(nodeState);
            }
            else if (nodeState.EventType == ChangeType.Remove)
            {
                DestroyTopography(nodeState);
            }
            else if (nodeState.EventType == ChangeType.Update)
            {
                UpdateTopography(nodeState);
            }
        }

        /// <summary>
        /// 将地貌信息实例化到场景内对应位置;
        /// </summary>
        void BuildTopography(MapNodeState<WorldNode> nodeState)
        {
            TopographyNode topography = InitTopographyNode(nodeState);
            activeWorldNode.Add(nodeState.MapPoint, topography);
        }

        /// <summary>
        /// 将这个位置存在的地貌销毁;
        /// </summary>
        void DestroyTopography(MapNodeState<WorldNode> nodeState)
        {
            TopographyNode topography = activeWorldNode[nodeState.MapPoint];
            DestroyTopographyNode(topography);
            activeWorldNode.Remove(nodeState.MapPoint);
        }

        /// <summary>
        /// 更新这个位置的地貌信息;
        /// </summary>
        void UpdateTopography(MapNodeState<WorldNode> nodeState)
        {
            ShortVector2 mapPoint = nodeState.MapPoint;
            TopographyNode topography;
            if (activeWorldNode.TryGetValue(mapPoint, out topography))
            {
                if (topography.ID != nodeState.WorldNode.Topography)
                {
                    DestroyTopographyNode(topography);
                    topography = InitTopographyNode(nodeState);
                    activeWorldNode[mapPoint] = topography;
                }
            }
            else
            {
                BuildTopography(nodeState);
            }
        }

        /// <summary>
        /// 向这个位置创建一个地形,并且返回创建内容;
        /// </summary>
        TopographyNode InitTopographyNode(MapNodeState<WorldNode> nodeState)
        {
            int topographyID = nodeState.WorldNode.Topography;
            ShortVector2 mapPoint = nodeState.MapPoint;
            return InitTopographyNode(topographyID, mapPoint);
        }
        /// <summary>
        /// 向这个位置创建一个地形,并且返回创建内容;
        /// </summary>
        TopographyNode InitTopographyNode(int topographyID, ShortVector2 mapPoint)
        {
            Vector2 planePoint = WorldConvert.MapToHex(mapPoint);
            Transform topographyPrefab = GetTopographyPrefab(topographyID, mapPoint);

            Transform sceneObject = Instantiate(topographyPrefab, planePoint);

            return new TopographyNode(topographyID, sceneObject);
        }

        /// <summary>
        /// 销毁这个地貌;
        /// </summary>
        void DestroyTopographyNode(TopographyNode topographyNode)
        {
            Destroy(topographyNode.topographyObject);
        }
        
        /// <summary>
        /// 获取到地貌预制,若未定义则返回 编号为 0 的预制,并输出警告;
        /// </summary>
        Transform GetTopographyPrefab(int topographyID, ShortVector2 mapPoint)
        {
            Transform topographyPrefab;
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
        /// 实例化
        /// </summary>
        Transform Instantiate(Transform topographyPrefab, Vector2 position)
        {
            Transform topographyObject = GameObject.Instantiate(topographyPrefab, position, Quaternion.identity) as Transform;
            return topographyObject;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        void Destroy(Transform topographyObject)
        {
            GameObject.Destroy(topographyObject.gameObject);
        }

        struct TopographyNode
        {
            public TopographyNode(int topographyID, Transform topographyObject)
            {
                this.ID = topographyID;
                this.topographyObject = topographyObject;
            }

            /// <summary>
            /// 当前保存的地貌;
            /// </summary>
            public int ID;

            /// <summary>
            /// 实例化的物体;
            /// </summary>
            public Transform topographyObject;
        }

    }

}
