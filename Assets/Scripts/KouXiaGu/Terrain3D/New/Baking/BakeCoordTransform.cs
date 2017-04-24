//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using KouXiaGu.Grids;
//using UnityEngine;

//namespace KouXiaGu.Terrain3D
//{

//    /// <summary>
//    /// 转换烘焙坐标;
//    /// </summary>
//    [Serializable]
//    class BakeCoordTransform
//    {
//        protected BakeCoordTransform()
//        {
//        }

//        /// <summary>
//        /// 预先定义的中心点,根据传入坐标位置转换到此中心点附近;
//        /// </summary>
//        [SerializeField]
//        CubicHexCoord center;

//        /// <summary>
//        /// 地图块坐标;
//        /// </summary>
//        public RectCoord Target { get; private set; }

//        /// <summary>
//        /// 地图块中心坐标;
//        /// </summary>
//        public CubicHexCoord TargetCenter { get; private set; }


//        /// <summary>
//        /// 设置地图块中心坐标;
//        /// </summary>
//        public void SetTarget(RectCoord chunkCoord)
//        {
//            Target = chunkCoord;
//            TargetCenter = chunkCoord.GetChunkHexCenter();
//        }

//        /// <summary>
//        /// 获取到转换后的坐标;
//        /// </summary>
//        public CubicHexCoord PositionConvert(CubicHexCoord terget)
//        {
//            CubicHexCoord coord = terget - TargetCenter;
//            return coord;
//        }

//        /// <summary>
//        /// 获取到转换后的坐标;
//        /// </summary>
//        public Vector3 PositionConvert(CubicHexCoord terget, float y)
//        {
//            CubicHexCoord coord = terget - TargetCenter;
//            return (coord + this.center).GetTerrainPixel(y);
//        }

//    }

//}
