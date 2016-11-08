using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Map.Navigation
{

    /// <summary>
    /// 导航信息地图结构接口;
    /// </summary>
    public interface INavigationMap<TMover>
    {

        INavigationNode<TMover> GetAt(IntVector2 position);

        /// <summary>
        /// 获取到周围允许行走到的元素;
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        IEnumerable<KeyValuePair<IntVector2, INavigationNode<TMover>>> GetAround(IntVector2 position);



    }

}
