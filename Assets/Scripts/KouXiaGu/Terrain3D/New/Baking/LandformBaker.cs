using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形烘焙;
    /// </summary>
    [DisallowMultipleComponent]
    public class LandformBaker : UnitySington<LandformBaker>
    {
        LandformBaker()
        {
        }

        [SerializeField]
        Stopwatch runtimeStopwatch;
        LinkedList<BakingRequest> requestQueue;

        public Stopwatch RuntimeStopwatch
        {
            get { return runtimeStopwatch; }
            set { runtimeStopwatch = value; }
        }

        public IBakingRequest Current
        {
            get { return requestQueue.First.Value; }
        }

        void Awake()
        {
            SetInstance(this);
            requestQueue = new LinkedList<BakingRequest>();
        }

        void Update()
        {
            if (requestQueue.Count != 0)
            {
                runtimeStopwatch.Restart();
                while (!runtimeStopwatch.Await())
                {
                    var current = requestQueue.First.Value;
                    current.MoveNext();
                    if (current.IsCompleted)
                    {
                        requestQueue.RemoveFirst();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 添加烘焙请求;
        /// </summary>
        public IBakingRequest AddRequest(RectCoord chunkCoord)
        {
            var request = new BakingRequest(chunkCoord);
            requestQueue.AddLast(request);
            return request;
        }

    }

}
