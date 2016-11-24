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
        Dictionary<ShortVector2, Topography> activeWorldNode;

        void Awake()
        {
            activeWorldNode = new Dictionary<ShortVector2, Topography>();
        }

        void Start()
        {
            WorldBuilder.GetInstance.ObserveBuilderNode.Subscribe(UpdateScene);
            topographiessData = TopographiessData.GetInstance;
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
            int topographyID = nodeState.WorldNode.Topography;
            ShortVector2 mapPoint = nodeState.MapPoint;

            Topography topography = Build(topographyID, mapPoint);

            activeWorldNode.Add(nodeState.MapPoint, topography);
        }

        /// <summary>
        /// 将这个位置存在的地貌销毁;
        /// </summary>
        void DestroyTopography(MapNodeState<WorldNode> nodeState)
        {
            ShortVector2 mapPoint = nodeState.MapPoint;

            Topography topography = activeWorldNode[mapPoint];
            Destroy(topography);
            activeWorldNode.Remove(nodeState.MapPoint);
        }

        /// <summary>
        /// 更新这个位置的地貌信息;
        /// </summary>
        void UpdateTopography(MapNodeState<WorldNode> nodeState)
        {
            int topographyID = nodeState.WorldNode.Topography;
            ShortVector2 mapPoint = nodeState.MapPoint;

            Topography topography = activeWorldNode[mapPoint];
            activeWorldNode[mapPoint] = UpdateTopography(topography, topographyID);
        }



        /// <summary>
        /// 向这个位置创建一个地形,并且返回创建内容;
        /// </summary>
        Topography Build(int topographyID, ShortVector2 mapPoint)
        {
            Vector2 planePoint = WorldConvert.MapToHex(mapPoint);
            Topography topographyPrefab = GetTopographyPrefab(topographyID, mapPoint);

            Topography topography = Instantiate(topographyPrefab, planePoint);

            return topography;
        }

        /// <summary>
        /// 更新这个地貌到目标地貌,并且返回正确的地貌;
        /// </summary>
        Topography UpdateTopography(Topography original, int targetID)
        {
            if (original.ID != targetID)
            {
                ShortVector2 mapPoint = original.MapPoint;
                Destroy(original);
                Build(targetID, mapPoint);
            }
            return original;
        }

        /// <summary>
        /// 销毁这个地貌;
        /// </summary>
        public void Destroy(Topography topography)
        {
            GameObject.Destroy(topography.gameObject);
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
        /// 实例化
        /// </summary>
        Topography Instantiate(Topography topographyPrefab, Vector2 position)
        {
            Topography topographyObject = GameObject.Instantiate(topographyPrefab, position, Quaternion.identity) as Topography;
            return topographyObject;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        void Destroy(Transform topographyObject)
        {
            GameObject.Destroy(topographyObject.gameObject);
        }

    }

}
