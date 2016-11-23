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

        [SerializeField]
        TopographiessData topographiessData;

        /// <summary>
        /// 已经实例化到场景的物体;
        /// </summary>
        List<TopographyNode> activeTopographyObjectList;

        void Awake()
        {
            WorldBuilder.GetInstance.ObserveBuilderNode.Subscribe(UpdateScene);
        }

        void Start()
        {
            topographiessData.Awake();
        }

        void UpdateScene(MapNodeState<WorldNode> nodeState)
        {
            if (nodeState.EventType == ChangeType.Add)
            {
                //Instantiate(nodeState);

            }
            else if (nodeState.EventType == ChangeType.Remove)
            {

            }
            else if (nodeState.EventType == ChangeType.Update)
            {

            }
        }

        //void Instantiate(MapNodeState<WorldNode> nodeState)
        //{
        //    Vector2 planePoint = WorldConvert.MapToHex(nodeState.MapPoint);
        //    Transform topographyPrefab = topographiessData.GetPrefabWithID(nodeState.WorldNode.Topography);

        //    Transform sceneObject = Instantiate(topographyPrefab, planePoint);
        //    TopographyNode topographyObject = new TopographyNode(nodeState.MapPoint, sceneObject);
        //    activeTopographyObjectList.Add(topographyObject);
        //}

        Transform Instantiate(Transform topographyPrefab, Vector2 position)
        {
            Transform topographyObject = GameObject.Instantiate(topographyPrefab, position, Quaternion.identity) as Transform;
            return topographyObject;
        }

        void Destroy(Transform topographyObject)
        {
            GameObject.Destroy(topographyObject.gameObject);
        }

        //class TopographyObject
        //{
        //    public TopographyObject(IntVector2 mapPoint, Transform topographyObject)
        //    {
        //        this.mapPoint = mapPoint;
        //        this.topographyObject = topographyObject;
        //    }

        //    public IntVector2 mapPoint;
        //    public Transform topographyObject;
        //}

    }

}
