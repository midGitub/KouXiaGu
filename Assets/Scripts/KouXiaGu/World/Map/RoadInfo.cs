using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.World.Map
{


    [ProtoContract]
    public class RoadInfo
    {

        /// <summary>
        /// 节点不存在道路时放置的标志;
        /// </summary>
        const uint EmptyRoadMark = 0;

        /// <summary>
        /// 起始的有效ID;
        /// </summary>
        const uint InitatingEffectiveID = 5;

        public RoadInfo() : this(InitatingEffectiveID)
        {
        }

        public RoadInfo(uint effectiveID)
        {
            EffectiveID = effectiveID;
        }

        /// <summary>
        /// 当前有效的ID;
        /// </summary>
        [ProtoMember(1)]
        public uint EffectiveID { get; set; }

    }

}
