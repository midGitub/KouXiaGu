using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Map.Navigation
{

    /// <summary>
    /// 导航信息地图结构接口;
    /// </summary>
    public interface INavigationMap<TNode, TMover>
        where TNode : INavigationNode<TMover>
    {

        /// <summary>
        /// 获取到周围允许行走到的元素;
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        IEnumerable<TNode> GetPath(IntVector2 position);

    }

}
