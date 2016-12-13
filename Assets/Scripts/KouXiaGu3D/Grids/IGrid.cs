using System;
using System.Collections.Generic;

namespace KouXiaGu.Grids
{

    /// <summary>
    /// 网格结构;
    /// </summary>
    public interface IGrid
    {
        /// <summary>
        /// 获取到目标点的邻居节点;
        /// </summary>
        IEnumerable<IGrid> GetNeighbours();

        /// <summary>
        /// 获取到目标点的邻居节点,但是也返回自己本身;
        /// </summary>
        IEnumerable<IGrid> GetNeighboursAndSelf();
    }

    /// <summary>
    /// 带方向的网格结构;
    /// </summary>
    public interface IGrid<T>
    {
        /// <summary>
        /// 获取到目标点的邻居节点;
        /// </summary>
        IEnumerable<CoordPack<IGrid<T>, T>> GetNeighbours();

        /// <summary>
        /// 获取到目标点的邻居节点,但是也返回自己本身;
        /// </summary>
        IEnumerable<CoordPack<IGrid<T>, T>> GetNeighboursAndSelf();
    }

}