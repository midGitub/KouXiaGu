using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Map.Navigation
{

    /// <summary>
    /// 导航信息地图结构接口;
    /// </summary>
    public interface INavMap<TNode, TMover>
        where TNode : INavNode<TMover>
    {

        /// <summary>
        /// 获取到这个点,若不存在则返回任意异常Exception;
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        TNode GetAt(IntVector2 position);

        /// <summary>
        /// 获取到周围存在的节点;
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        IEnumerable<KeyValuePair<IntVector2, TNode>> GetAround(IntVector2 position);



    }

}
