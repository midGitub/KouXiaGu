using System;
using System.Collections;
using System.Collections.Generic;
using KouXiaGu.Grids;
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
        LandformBakeManager()
        {
        }

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

        void Awake()
        {
            requestQueue = new CoroutineQueue<IBakingRequest>(runtimeStopwatch);
        }

        void Update()
        {
            requestQueue.Next();
        }

        /// <summary>
        /// 添加烘焙请求;
        /// </summary>
        public IBakingRequest AddRequest(RectCoord chunkCoord)
        {
            throw new NotImplementedException();
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
