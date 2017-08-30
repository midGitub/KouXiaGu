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
    public static class LandformInfo 
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
        /// 一半地形块宽度;
        /// </summary>
        public static readonly float ChunkHalfWidth = ChunkWidth / 2;

        /// <summary>
        /// 地形块高度;
        /// </summary>
        public static readonly float ChunkHeight = RectTerrainInfo.NodeHeight * NumberOfNodesInHeight;

        /// <summary>
        /// 一半地形块高度;
        /// </summary>
        public static readonly float ChunkHalfHeight = ChunkHeight / 2;

        /// <summary>
        /// 网格信息;
        /// </summary>
        public static readonly RectGrid Grid = new RectGrid(ChunkWidth, ChunkHeight);

        /// <summary>
        /// 将地形块坐标转换成地形块中心点像素坐标;
        /// </summary>
        public static Vector3 ToLandformChunkPixel(this RectCoord chunkPos)
        {
            return Grid.GetCenter(chunkPos);
        }

        /// <summary>
        /// 将像素坐标转换成地形块坐标;
        /// </summary>
        public static RectCoord ToLandformChunkRect(this Vector3 pos)
        {
            return Grid.GetCoord(pos);
        }


        static readonly RectCoord[] ChildOffsets = new RectCoord[]
            {
                new RectCoord(-1 , 1),
                new RectCoord(0, 1), 
                new RectCoord(1, 1),

                new RectCoord(-1, 0),
                new RectCoord(0, 0),
                new RectCoord(1, 0),

                new RectCoord(-1, -1),
                new RectCoord(0, -1),
                new RectCoord(1, -1),
            };

        /// <summary>
        /// 获取到块的所有子节点;
        /// </summary>
        public static IEnumerable<RectCoord> GetChildren(RectCoord chunkPos)
        {
            foreach (var offset in ChildOffsets)
            {
                yield return offset + chunkPos;
            }
        }
    }
}
