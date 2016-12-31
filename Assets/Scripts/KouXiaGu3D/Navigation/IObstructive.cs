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
        /// 获取到人物走到这个地图节点的代价值
        /// </summary>
        /// <param name="currentPoint">当前物体所在的点</param>
        /// <param name="targetNode">行走到的节点</param>
        /// <param name="destination">寻路终点</param>
        /// <returns></returns>
        float GetCost(TPoint currentPoint, TNode targetNode, TPoint destination);

    }

}
