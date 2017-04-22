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

        BakingRequest Current
        {
            get { return requestQueue.First.Value; }
        }

        public bool IsEmpty
        {
            get { return requestQueue.Count == 0; }
        }

        void Awake()
        {
            SetInstance(this);
            requestQueue = new LinkedList<BakingRequest>();
        }

        /// <summary>
        /// 请求烘培地图块,若已经存在请求,则返回存在的请求;
        /// </summary>
        public IBakingRequest Bake(RectCoord chunkCoord)
        {
            var request = new BakingRequest(chunkCoord);
            requestQueue.AddLast(request);
            return request;
        }

        void Dequeue()
        {
            requestQueue.RemoveFirst();
        }

        void Update()
        {
            if (!IsEmpty)
            {
                runtimeStopwatch.Restart();

                while (!runtimeStopwatch.Await())
                {
                    Current.MoveNext();
                    if (Current.IsCompleted)
                    {
                        Dequeue();
                        break;
                    }
                }
            }
        }

    }

}
