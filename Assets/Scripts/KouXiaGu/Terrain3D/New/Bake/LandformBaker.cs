using System;
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
    public class LandformBaker : MonoBehaviour
    {
        public LandformBaker()
        {
            bakeQueue = new BakeQueue();
        }

        readonly BakeQueue bakeQueue;

        /// <summary>
        /// 请求烘培地图块,若已经存在请求,则返回存在的请求;
        /// </summary>
        public IBakingRequest Bake(RectCoord chunkCoord)
        {
            return bakeQueue.Bake(chunkCoord);
        }

        /// <summary>
        /// 请求取消地图块的烘培请求;
        /// </summary>
        public bool Cancel(RectCoord chunkCoord)
        {
            return bakeQueue.Cancel(chunkCoord);
        }

    }

}
