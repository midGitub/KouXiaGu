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
        public const uint InitialID = 0;

        public IdentifierGenerator() : this(InitialID)
        {
        }

        public IdentifierGenerator(uint lastReturnedID)
        {
            this.lastReturnedID = lastReturnedID;
        }

        public IdentifierGenerator(IdentifierGenerator item)
        {
            lastReturnedID = item.LastReturnedID;
        }

        [ProtoMember(1)]
        uint lastReturnedID;

        /// <summary>
        /// 最后返回的ID;
        /// </summary>
        public uint LastReturnedID
        {
            get { return lastReturnedID; }
        }

        /// <summary>
        /// 获取到一个唯一的有效ID;
        /// </summary>
        public uint GetNewEffectiveID()
        {
            return ++lastReturnedID;
        }

        /// <summary>
        /// 重置记录信息;
        /// </summary>
        internal void Reset()
        {
            lastReturnedID = InitialID;
        }
    }
}
