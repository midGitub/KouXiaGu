//using KouXiaGu.Grids;
//using KouXiaGu.World;
//using KouXiaGu.World.Map;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace KouXiaGu.Terrain3D
//{
//    public interface IBuildingCreateRequest
//    {
//        bool IsCanceled { get; }
//        void AddQueue();
//        void OutQueue();
//    }

//    /// <summary>
//    /// 异步的创建建筑物到场景;
//    /// </summary>
//    public sealed class BuildingCreater : MonoBehaviour
//    {
//        BuildingCreater()
//        {
//        }

//        [SerializeField]
//        Stopwatch runtimeStopwatch;
//        Queue<IBuildingCreateRequest> requestQueue;
//        Coroutine createrCoroutine;

//        void Awake()
//        {
//            requestQueue = new Queue<IBuildingCreateRequest>();
//            createrCoroutine = new Coroutine(Coroutine());
//        }

//        void Update()
//        {
//            runtimeStopwatch.Restart();
//            createrCoroutine.MoveNext();
//        }

//        IEnumerator Coroutine()
//        {
//            while (true)
//            {
//                while (requestQueue.Count == 0)
//                    yield return null;

//                IBuildingCreateRequest request = requestQueue.Dequeue();

//                if (runtimeStopwatch.Await())
//                    yield return null;
//            }
//        }
//    }

//}
