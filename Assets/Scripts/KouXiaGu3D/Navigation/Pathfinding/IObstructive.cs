using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Navigation
{

    /// <summary>
    /// 寻路代价值获取;
    /// </summary>
    public interface IObstructive<TPoint, TNode>
    {
        /// <summary>
        /// 这个点是否允许行走到;
        /// </summary>
        bool CanWalk(TNode item);

        /// <summary>
        /// 获取到代价值
        /// </summary>
        /// <param name="targetPoint">行走到的位置</param>
        /// <param name="targetNode">行走到的节点</param>
        /// <param name="destination">寻路终点</param>
        /// <returns></returns>
        float GetCost(TPoint targetPoint, TNode targetNode, TPoint destination);

    }

}
