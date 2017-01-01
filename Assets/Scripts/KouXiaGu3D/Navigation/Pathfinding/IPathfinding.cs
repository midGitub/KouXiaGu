using System.Collections.Generic;
using KouXiaGu.Grids;

namespace KouXiaGu.Navigation
{


    public interface IPathfinding<TPoint, TNode>
        where TPoint : IGrid
    {

        /// <summary>
        /// 开始寻路;若无法找到目标点则返回异常;
        /// </summary>
        LinkedList<TPoint> Search(
            IDictionary<TPoint, TNode> map,
            IObstructive<TPoint, TNode> obstructive,
            IRange<TPoint> range,
            TPoint starting,
            TPoint destination);

    }

}
