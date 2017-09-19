using KouXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.RectTerrain
{


    public static class RectTerrainInfo
    {
        /// <summary>
        /// 地图节点宽度;
        /// </summary>
        public static readonly float NodeWidth = 1;

        /// <summary>
        /// 地图节点的高度;
        /// </summary>
        public static readonly float NodeHeight = 1;

        /// <summary>
        /// 网格信息;
        /// </summary>
        public static readonly RectGrid Grid = new RectGrid(NodeWidth, NodeHeight);

        /// <summary>
        /// 将地图节点坐标转换为地图节点中心像素坐标;
        /// </summary>
        public static Vector3 ToRectTerrainPixel(this RectCoord pos)
        {
            return Grid.GetCenter(pos);
        }

        /// <summary>
        /// 将地图节点坐标转换为地图节点中心像素坐标;
        /// </summary>
        public static Vector3 ToRectTerrainPixel(this RectCoord pos, float y)
        {
            Vector3 pixel = Grid.GetCenter(pos);
            pixel.y = y;
            return pixel;
        }

        /// <summary>
        /// 将像素坐标转换成地图节点坐标;
        /// </summary>
        public static RectCoord ToRectTerrainRect(this Vector3 pos)
        {
            return Grid.GetCoord(pos);
        }
    }
}
