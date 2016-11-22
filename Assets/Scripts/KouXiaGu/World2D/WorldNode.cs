using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 世界节点;
    /// </summary>
    [ProtoContract]
    public struct WorldNode
    {
        /// <summary>
        /// 地貌;
        /// </summary>
        [ProtoMember(1)]
        public int Topography { get; set; }

        /// <summary>
        /// 是否存在道路;
        /// </summary>
        [ProtoMember(2)]
        public bool Road { get; set; }

    }

}
