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
        BakingRequestQueue bakeQueue;

        public Stopwatch RuntimeStopwatch
        {
            get { return runtimeStopwatch; }
            set { runtimeStopwatch = value; }
        }

        BakingRequest Current
        {
            get { return bakeQueue.First; }
        }

        public IReadOnlyCollection<RectCoord> Requests
        {
            get { return bakeQueue.Requests; ; }
        }

        void Awake()
        {
            SetInstance(this);
            bakeQueue = new BakingRequestQueue();
        }

        /// <summary>
        /// 请求烘培地图块,若已经存在请求,则返回存在的请求;
        /// </summary>
        public IBakingRequest Bake(RectCoord chunkCoord)
        {
            return bakeQueue.Enqueue(chunkCoord);
        }

        /// <summary>
        /// 请求取消地图块的烘培请求;
        /// </summary>
        public bool Cancel(RectCoord chunkCoord)
        {
            return bakeQueue.Cancel(chunkCoord);
        }

        void Update()
        {
            if (Current != null)
            {
                runtimeStopwatch.Restart();

                while (!runtimeStopwatch.Await())
                {
                    Current.MoveNext();
                    if (Current.IsCompleted)
                    {
                        bakeQueue.Dequeue();
                        break;
                    }
                }
            }
        }

    }

}
