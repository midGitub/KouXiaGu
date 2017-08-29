using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.RectTerrain
{

    /// <summary>
    /// 地形块信息;
    /// </summary>
    public static class LandformChunkInfo 
    {
        /// <summary>
        /// 宽度存在的地图节点数目;
        /// </summary>
        public static readonly int NumberOfNodesInWidth = 3;

        /// <summary>
        /// 高度存在的地图节点数目;
        /// </summary>
        public static readonly int NumberOfNodesInHeight = 3;

        /// <summary>
        /// 地形块宽度;
        /// </summary>
        public static readonly float ChunkWidth = RectTerrainInfo.NodeWidth * NumberOfNodesInWidth;

        /// <summary>
        /// 地形块高度;
        /// </summary>
        public static readonly float ChunkHeight = RectTerrainInfo.NodeHeight * NumberOfNodesInHeight;

        /// <summary>
        /// 网格信息;
        /// </summary>
        public static readonly RectGrid Grid = new RectGrid(ChunkWidth, ChunkHeight);

        /// <summary>
        /// 将地形块坐标转换成地形块中心点像素坐标;
        /// </summary>
        public static Vector3 ToLandformChunkPixel(this RectCoord pos)
        {
            return Grid.GetCenter(pos);
        }

        /// <summary>
        /// 将像素坐标转换成地形块坐标;
        /// </summary>
        public static RectCoord ToLandformChunkRect(this Vector3 pos)
        {
            return Grid.GetCoord(pos);
        }
    }
}
