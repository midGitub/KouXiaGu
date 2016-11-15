using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Map.Navigation;
using ProtoBuf;

namespace KouXiaGu.Map
{

    [ProtoContract]
    public class HexMapNode : INavNode<Navigator>
    {
        public float GetCost(Navigator mover)
        {
            throw new NotImplementedException();
        }

        public bool IsOpenNode(Navigator mover)
        {
            throw new NotImplementedException();
        }
    }

}
