using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Navigation
{

    public interface INavigationMap<T>
    {
        /// <summary>
        /// 获取到所有邻居节点;
        /// </summary>
        IEnumerable<T> GetNeighbors(T position);
    }

    public interface IWalker<T> : INavigationMap<T>
    {
        /// <summary>
        /// 该位置是否允许行走?
        /// </summary>
        bool IsWalkable(T position);

        /// <summary>
        /// 获取到导航点信息;
        /// </summary>
        NavigationNode<T> GetNavigationNode(T position, T destination);
    }
}
