using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{


    public struct RoadNode
    {

        /// <summary>
        /// 道路使用的贴图;
        /// </summary>
        public RoadRes Road { get; private set; }

        /// <summary>
        /// 贴图旋转角度;
        /// </summary>
        public float RotationY { get; private set; }

        /// <summary>
        /// 获取到这个地图节点应该放置的道路贴图;
        /// </summary>
        /// <param name="map">使用的地图</param>
        /// <param name="coord">所在的位置;</param>
        /// <returns></returns>
        public static RoadNode[] GetRoadTexs(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        {
            throw new NotImplementedException();
        }

    }

}
