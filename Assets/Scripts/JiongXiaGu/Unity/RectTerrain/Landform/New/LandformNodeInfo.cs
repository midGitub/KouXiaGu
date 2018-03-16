using JiongXiaGu.Grids;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{
    /// <summary>
    /// 地形块信息;
    /// </summary>
    public static class LandformNodeInfo
    {
        /// <summary>
        /// 地图节点宽度;
        /// </summary>
        public const float Width = 1;

        /// <summary>
        /// 地图节点的高度;
        /// </summary>
        public const float Height = 1;

        /// <summary>
        /// 网格信息;
        /// </summary>
        public static readonly RectGrid Grid = new RectGrid(Width, Height);

        /// <summary>
        /// 将地图节点坐标转换为地图节点中心像素坐标;
        /// </summary>
        public static Vector3 ToLandformPixel(this RectCoord pos)
        {
            return Grid.GetCenter(pos);
        }

        /// <summary>
        /// 将地图节点坐标转换为地图节点中心像素坐标;
        /// </summary>
        public static Vector3 ToLandformPixel(this RectCoord pos, float y)
        {
            Vector3 pixel = Grid.GetCenter(pos);
            pixel.y = y;
            return pixel;
        }

        /// <summary>
        /// 将像素坐标转换成地图节点坐标;
        /// </summary>
        public static RectCoord ToLandformRect(this Vector3 pos)
        {
            return Grid.GetCoord(pos);
        }
    }


}
