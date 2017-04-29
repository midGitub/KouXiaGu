using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 坐标转换;
    /// </summary>
    [Serializable]
    public class CoordinateTransform
    {

        /// <summary>
        /// 中心点,根据传入坐标位置转换到此中心点附近;
        /// </summary>
        public CubicHexCoord center;

        /// <summary>
        /// 目标中心点;
        /// </summary>
        public CubicHexCoord targetCenter { get; private set; }


        public void SetTargetCenter(RectCoord chunkCoord)
        {
            targetCenter = chunkCoord.GetChunkHexCenter();
        }

        public void SetTargetCenter(CubicHexCoord coord)
        {
            targetCenter = coord;
        }


        /// <summary>
        /// 将坐标转换到中心坐标附近;
        /// </summary>
        Vector3 PositionConvert(CubicHexCoord terget, float height)
        {
            CubicHexCoord coord = PositionConvert(terget);
            return coord.GetTerrainPixel(height);
        }

        /// <summary>
        /// 将坐标转换到中心坐标附近;
        /// </summary>
        public CubicHexCoord PositionConvert(CubicHexCoord terget)
        {
            CubicHexCoord coord = terget - targetCenter;
            return coord + center;
        }

    }

}
