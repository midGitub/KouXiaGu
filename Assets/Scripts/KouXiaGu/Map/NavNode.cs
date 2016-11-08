using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Map.Navigation;
using ProtoBuf;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 为游戏实现的导航节点;
    /// </summary>
    [ProtoContract]
    public class NavNode : INavNode<Navigator>
    {

        float INavNode<Navigator>.GetCost(Navigator mover)
        {
            throw new NotImplementedException();
        }

        bool INavNode<Navigator>.IsOpenNode(Navigator mover)
        {
            throw new NotImplementedException();
        }
    }

}
