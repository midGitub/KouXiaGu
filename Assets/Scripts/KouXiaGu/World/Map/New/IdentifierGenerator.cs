using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 记录,获取编号;
    /// </summary>
    [ProtoContract]
    public class IdentifierGenerator
    {
        /// <summary>
        /// 起始的有效ID;
        /// </summary>
        internal const uint InitialID = 1;

        public IdentifierGenerator() : this(InitialID)
        {
        }

        public IdentifierGenerator(uint effectiveID)
        {
            EffectiveID = effectiveID;
        }

        /// <summary>
        /// 当前有效的ID;
        /// </summary>
        [ProtoMember(1)]
        public uint EffectiveID { get; private set; }

        /// <summary>
        /// 获取到一个唯一的有效ID;
        /// </summary>
        public uint GetNewEffectiveID()
        {
            return EffectiveID++;
        }

        /// <summary>
        /// 重置记录信息;
        /// </summary>
        internal void Reset()
        {
            EffectiveID = InitialID;
        }
    }
}
