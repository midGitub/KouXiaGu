using System;
using System.Collections.Generic;

namespace JiongXiaGu.Grids
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

    public interface IGrid<TVector>
        where TVector : IGrid<TVector>
    {
        /// <summary>
        /// 获取到目标点的邻居节点;
        /// </summary>
        IEnumerable<TVector> GetNeighbours();

        /// <summary>
        /// 获取到目标点的邻居节点,但是也返回自己本身;
        /// </summary>
        IEnumerable<TVector> GetNeighboursAndSelf();
    }

    /// <summary>
    /// 带方向的网格结构;
    /// </summary>
    public interface IGrid<TVector, TDirection>
    {
        /// <summary>
        /// 获取到目标点的邻居节点;
        /// </summary>
        IEnumerable<CoordPack<TVector, TDirection>> GetNeighbours();

        /// <summary>
        /// 获取到目标点的邻居节点,但是也返回自己本身;
        /// </summary>
        IEnumerable<CoordPack<TVector, TDirection>> GetNeighboursAndSelf();
    }

}