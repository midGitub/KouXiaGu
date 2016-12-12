using System;
using System.Collections.Generic;

namespace KouXiaGu
{

    /// <summary>
    /// 网格的点结构;
    /// </summary>
    public interface IGridPoint
    {
        /// <summary>
        /// 获取到目标点的邻居节点;
        /// </summary>
        IEnumerable<IGridPoint> GetNeighbours();

        /// <summary>
        /// 获取到目标点的邻居节点,但是也返回自己本身;
        /// </summary>
        IEnumerable<IGridPoint> GetNeighboursAndSelf();
    }

    /// <summary>
    /// 带方向的网格点;
    /// </summary>
    public interface IGridPoint<TC, TD> : IGridPoint
        where TC : IGridPoint
    {

        /// <summary>
        /// 按标记为从 高位到低位 循序返回的迭代结构;不包含本身
        /// </summary>
        IEnumerable<TD> Directions { get; }

        /// <summary>
        /// 获取到从 高位到低位 顺序返回的迭代结构;(存在本身方向,且在最高位);
        /// </summary>
        IEnumerable<TD> DirectionsAndSelf { get; }

        /// <summary>
        /// 获取到方向集表示的所有方向;
        /// </summary>
        IEnumerable<TD> GetDirections(TD directions);
        /// <summary>
        /// 获取到这个方向的坐标;
        /// </summary>
        IGridPoint GetDirection(TD direction);

    }


}