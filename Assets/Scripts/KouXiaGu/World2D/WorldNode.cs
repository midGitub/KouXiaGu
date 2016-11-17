using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 地图节点只读接口,提供只读节点接口继承;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnly<T> { }


    public interface IReadOnlyWorldNode : IReadOnly<WorldNode>
    {
        int Landform { get; }
        bool Road { get; }
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
        public int Landform { get; set; }

        /// <summary>
        /// 是否存在道路;
        /// </summary>
        [ProtoMember(2)]
        public bool Road { get; set; }

    }

}
