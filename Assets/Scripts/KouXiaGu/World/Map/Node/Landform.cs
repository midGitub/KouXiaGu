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
        /// 编号,不存在则为0;
        /// </summary>
        [ProtoMember(0)]
        public uint ID;

        /// <summary>
        /// 地形类型;
        /// </summary>
        [ProtoMember(1)]
        public int LandformType;

        /// <summary>
        /// 地形旋转角度;
        /// </summary>
        [ProtoMember(2)]
        public float Angle;
    }

    public static class MapLandformExtensions
    {
        /// <summary>
        /// 不存在地形时放置的标志;
        /// </summary>
        public const int EmptyMark = 0;

        /// <summary>
        /// 是否存在地形?
        /// </summary>
        public static bool Exist(this LandformNode node)
        {
            return node.ID != EmptyMark;
        }

        /// <summary>
        /// 清除地形信息;
        /// </summary>
        public static LandformNode Destroy(this LandformNode node)
        {
            return default(LandformNode);
        }

        /// <summary>
        /// 更新建筑信息;
        /// </summary>
        public static LandformNode Update(this LandformNode node, MapData data, int landformType, float angle)
        {
            return Update(node, data.Landform, landformType, angle);
        }

        /// <summary>
        /// 更新建筑信息;
        /// </summary>
        public static LandformNode Update(this LandformNode node, IdentifierGenerator landformInfo, int landformType, float angle)
        {
            if (!node.Exist())
            {
                node.ID = landformInfo.GetNewEffectiveID();
            }
            node.LandformType = landformType;
            node.Angle = angle;
            return node;
        }
    }

}
