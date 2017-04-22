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
        BakeQueue bakeQueue;
        IEnumerator bakeCoroutine;

        public Stopwatch RuntimeStopwatch
        {
            get { return runtimeStopwatch; }
            set { runtimeStopwatch = value; }
        }

        BakingRequest Current
        {
            get { return bakeQueue.First; }
        }

        void Awake()
        {
            SetInstance(this);
            bakeQueue = new BakeQueue();
            bakeCoroutine = BakeCoroutine();
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
            if (!bakeQueue.IsEmpty)
            {
                runtimeStopwatch.Restart();

                while(!runtimeStopwatch.Await())
                    bakeCoroutine.MoveNext();
            }
        }

        IEnumerator BakeCoroutine()
        {
            while (true)
            {
                yield return null;
            }
        }
    }

}
