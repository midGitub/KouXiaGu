using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形块信息;
    /// </summary>
    public static class ChunkInfo
    {
        /// <summary>
        /// 地图节点所使用的六边形参数;
        /// </summary>
        static Hexagon MapHexagon
        {
            get { return LandformConvert.hexagon; }
        }

        /// <summary>
        /// 地形块大小(需要大于或等于2),不能随意变更;
        /// </summary>
        public static readonly int ChunkSize = 3;

        /// <summary>
        /// 地形块宽度;
        /// </summary>
        public static readonly float ChunkWidth = (float)(MapHexagon.OuterDiameters * 1.5f * (ChunkSize - 1));
        public static readonly float ChunkHalfWidth = ChunkWidth / 2;

        /// <summary>
        /// 地形块高度;
        /// </summary>
        public static readonly float ChunkHeight = (float)MapHexagon.InnerDiameters * ChunkSize;
        public static readonly float ChunkHalfHeight = ChunkHeight / 2;


        /// <summary>
        /// 矩形网格结构,用于地形块的排列;
        /// </summary>
        public static readonly RectGrid ChunkGrid = new RectGrid(ChunkWidth, ChunkHeight);

        /// <summary>
        /// 地形块坐标 获取到其中心的六边形坐标;
        /// </summary>
        public static CubicHexCoord GetChunkHexCenter(this RectCoord coord)
        {
            Vector3 pixelCenter = ChunkGrid.GetCenter(coord);
            return LandformConvert.Grid.GetCubic(pixelCenter);
        }


        ///// <summary>
        ///// 获取到地图节点所属的地形块;
        ///// </summary>
        //public static RectCoord[] GetBelongChunks(CubicHexCoord coord)
        //{
        //    RectCoord[] chunks = new RectCoord[2];
        //    GetBelongChunks(coord, ref chunks);
        //    return chunks;
        //}

        ///// <summary>
        ///// 获取到地图节点所属的地形块;
        ///// 传入数组容量需要大于或者等于2,所属的地形块编号放置在 0 和 1 下标处;
        ///// </summary>
        //public static void GetBelongChunks(CubicHexCoord coord, ref RectCoord[] chunks)
        //{
        //    Vector3 point = LandformConvert.Grid.GetPixel(coord);
        //    GetBelongChunks(point, ref chunks);
        //}

        ///// <summary>
        ///// 获取到地图节点所属的地形块;
        ///// 传入数组容量需要大于或者等于2,所属的地形块编号放置在 0 和 1 下标处;
        ///// </summary>
        //public static void GetBelongChunks(Vector3 pointCenter, ref RectCoord[] chunks)
        //{
        //    try
        //    {
        //        Vector3 point1 = pointCenter + CheckBelongChunkPoint;
        //        chunks[0] = ChunkGrid.GetCoord(point1);

        //        Vector3 point2 = pointCenter - CheckBelongChunkPoint;
        //        chunks[1] = ChunkGrid.GetCoord(point2);
        //    }
        //    catch (ArgumentOutOfRangeException)
        //    {
        //        return;
        //    }
        //}



        /// <summary>
        /// 获取到地图节点所属的地形块;
        /// </summary>
        public static IEnumerable<RectCoord> GetBelongChunks(CubicHexCoord coord)
        {
            Vector3 point = LandformConvert.Grid.GetPixel(coord);
            return GetBelongChunks(point);
        }

        static readonly Vector3 CheckBelongChunkPoint =
            new Vector3((float)MapHexagon.OuterRadius / 2, 0, (float)MapHexagon.InnerRadius / 2);

        /// <summary>
        /// 获取到地图节点所属的地形块;
        /// </summary>
        public static IEnumerable<RectCoord> GetBelongChunks(Vector3 pointCenter)
        {
            Vector3 point1 = pointCenter + CheckBelongChunkPoint;
            RectCoord chunk1 = ChunkGrid.GetCoord(point1);

            Vector3 point2 = pointCenter - CheckBelongChunkPoint;
            RectCoord chunk2 = ChunkGrid.GetCoord(point2);

            if (chunk1 == chunk2)
            {
                yield return chunk1;
            }
            else
            {
                yield return chunk1;
                yield return chunk2;
            }
        }

    }

}
