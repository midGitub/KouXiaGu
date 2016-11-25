using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D.Navigation
{

    /// <summary>
    /// 导航路径;将地图路径转换成导航路径提供单独实例使用;
    /// </summary>
    public class NavPath
    {
        public NavPath(LinkedList<ShortVector2> waypath)
        {
            this.WayPath = waypath;
        }

        /// <summary>
        /// 起点;
        /// </summary>
        public Vector2 StartingNode
        {
            get { return MapPointToPlanePoint(WayPath.First.Value); }
        }
        /// <summary>
        /// 寻路的终点;
        /// </summary>
        public Vector2 DestinationNode
        {
            get { return MapPointToPlanePoint(WayPath.Last.Value); }
        }
        /// <summary>
        /// 路径点合集;
        /// </summary>
        public LinkedList<ShortVector2> WayPath { get; private set; }

        /// <summary>
        /// 获取到下一步行走到的点和行走的速度;
        /// 获取成功返回true,否则返回false;
        /// </summary>
        public bool TryGoNext(out Vector2 planePoint, out float maxSpeed)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 地图坐标转换成平面坐标;
        /// </summary>
        Vector2 MapPointToPlanePoint(ShortVector2 mapPoint)
        {
            return WorldConvert.MapToHex(mapPoint);
        }

    }

}
