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
        bool isOccupiedBaker;
        [SerializeField]
        LandformBaker baker;
        [SerializeField]
        Stopwatch runtimeStopwatch;
        CoroutineQueue<ChunkRequest> requestQueue;

        public Stopwatch RuntimeStopwatch
        {
            get { return runtimeStopwatch; }
            set { runtimeStopwatch = value; }
        }

        void Awake()
        {
            requestQueue = new CoroutineQueue<ChunkRequest>();
        }

        void Update()
        {
            requestQueue.Next();
        }

        /// <summary>
        /// 获取到烘焙方法类,并且锁定,若已经被锁定,则返回异常;
        /// </summary>
        /// <returns>取消锁定处理器;</returns>
        public IDisposable GetBakerAndLock(out LandformBaker baker)
        {
            if (isOccupiedBaker)
            {
                throw new ArgumentException("LandformBaker 已被锁定;");
            }
            else
            {
                baker = this.baker;
                isOccupiedBaker = true;
                return new Unsubscriber(() => isOccupiedBaker = false);
            }
        }

        public void AddRequest(ChunkRequest request)
        {
            requestQueue.Add(request);
        }

        /// <summary>
        /// 取消所有请求;
        /// </summary>
        public void CanceleAll()
        {
            foreach (var request in requestQueue)
            {
                request.Dispose();
            }
            requestQueue.Clear();
        }

    }

}
