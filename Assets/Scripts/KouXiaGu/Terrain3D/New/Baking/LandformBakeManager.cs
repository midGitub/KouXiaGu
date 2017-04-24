using System;
using System.Collections;
using System.Collections.Generic;
using KouXiaGu.Grids;
using KouXiaGu.World;
using UniRx;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形烘焙管理;
    /// </summary>
    [DisallowMultipleComponent]
    public class LandformBakeManager : MonoBehaviour
    {
        public static LandformBakeManager Initialise(IWorldData worldData)
        {
            var item = SceneObject.GetObject<LandformBakeManager>();
            item.WorldData = worldData;
            return item;
        }

        LandformBakeManager()
        {
        }

        public IWorldData WorldData { get; private set; }
        [SerializeField]
        Stopwatch runtimeStopwatch;
        CoroutineQueue<IBakingRequest> requestQueue;
        [SerializeField]
        LandformBaker baker;

        public Stopwatch RuntimeStopwatch
        {
            get { return runtimeStopwatch; }
            set { runtimeStopwatch = value; }
        }

        public CoroutineQueue<IBakingRequest> RequestQueue
        {
            get { return requestQueue; }
            private set { requestQueue = value; }
        }

        public LandformBaker Baker
        {
            get { return baker; }
        }

        void Awake()
        {
            requestQueue = new CoroutineQueue<IBakingRequest>(runtimeStopwatch);
        }

        void Update()
        {
            requestQueue.Next();
        }

        /// <summary>
        /// 取消所有请求;
        /// </summary>
        public void CanceleAll()
        {
            foreach (var request in requestQueue)
            {
                request.Cancel();
            }
            requestQueue.Clear();
        }

    }

}
