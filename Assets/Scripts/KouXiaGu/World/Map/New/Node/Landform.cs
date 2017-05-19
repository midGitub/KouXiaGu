using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 节点地貌信息;
    /// </summary>
    [ProtoContract]
    public struct LandformNode
    {
        /// <summary>
        /// 地形类型;
        /// </summary>
        [ProtoMember(1)]
        public int Type;

        /// <summary>
        /// 地形旋转角度;
        /// </summary>
        [ProtoMember(2)]
        public float Angle;
    }

    public static class MapLandformExtensions
    {


    }

}
