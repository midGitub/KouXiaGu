using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.World2D
{

    public interface IReadOnlyWorldNode
    {
        
    }

    /// <summary>
    /// 世界节点;
    /// </summary>
    [ProtoContract]
    public class WorldNode : IReadOnlyWorldNode
    {
        /// <summary>
        /// 地貌;
        /// </summary>
        [ProtoMember(1)]
        private int landform;

        /// <summary>
        /// 存在道路的;
        /// </summary>
        [ProtoMember(2)]
        private bool Road;

    }

}
