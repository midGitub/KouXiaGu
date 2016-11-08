using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Map.Navigation
{

    /// <summary>
    /// 导航节点接口;
    /// </summary>
    public interface INavNode<TMover>
    {

        /// <summary>
        /// 是否属于可以行走的节点;
        /// </summary>
        /// <param name="mover"></param>
        /// <returns></returns>
        bool IsOpenNode(TMover mover);

        /// <summary>
        /// 获取到这个物体的行走代价值;
        /// </summary>
        /// <param name="mover"></param>
        /// <returns></returns>
        float GetCost(TMover mover);

    }

}
