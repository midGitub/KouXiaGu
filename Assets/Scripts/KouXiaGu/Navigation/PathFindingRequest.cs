using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using System.Threading;

namespace KouXiaGu.Navigation
{

    /// <summary>
    /// 寻路请求;
    /// </summary>
    public class PathFindingRequest<TPoint, TNode>
    {

        /// <summary>
        /// 起点;
        /// </summary>
        public TPoint Starting { get; private set; }

        /// <summary>
        /// 寻路的终点;
        /// </summary>
        public TPoint Destination { get; private set; }

        /// <summary>
        /// 使用的地图;
        /// </summary>
        public IDictionary<TPoint, TNode> Map { get; private set; }

        /// <summary>
        /// 代价值获取;
        /// </summary>
        public IPathFindingCost<TPoint, TNode> Obstructive { get; private set; }

        /// <summary>
        /// 寻路范围限制;
        /// </summary>
        public IRange<TPoint> Range { get; private set; }


        /// <summary>
        /// 导航路径;
        /// </summary>
        public LinkedList<TPoint> Path { get; private set; }

        /// <summary>
        /// 是否已完成;
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// 是否由于未经处理异常的原因而完成;
        /// </summary>
        public bool IsFaulted { get; private set; }

        /// <summary>
        /// 导致结束的异常;
        /// </summary>
        public Exception Exception { get; private set; }

    }

}
