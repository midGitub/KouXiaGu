using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D.Navigation
{

    /// <summary>
    /// 代价值获取;
    /// </summary>
    public interface IObstructive<TNode, TMover>
    {
        /// <summary>
        /// 这个点是否允许行走到;
        /// </summary>
        bool CanWalk(TMover mover, TNode item);

        /// <summary>
        /// 获取到人物走到这个地图节点的代价值,和距离终点的代价值总和(作为寻路依据);
        /// </summary>
        float GetCost(TMover mover, ShortVector2 currentPoint, TNode currentNode, ShortVector2 destination);
    }

}
