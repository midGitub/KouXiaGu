﻿using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 节点建筑信息;
    /// </summary>
    [ProtoContract]
    public struct BuildingNode
    {
        /// <summary>
        /// 编号,不存在建筑则为0;
        /// </summary>
        [ProtoMember(0)]
        public uint ID;

        /// <summary>
        /// 建筑物类型编号;
        /// </summary>
        [ProtoMember(1)]
        public int Type;

        /// <summary>
        /// 建筑物旋转的角度;
        /// </summary>
        [ProtoMember(2)]
        public float Angle;
    }

    public static class MapBuildingExtensions
    {
        /// <summary>
        /// 不存在建筑时放置的标志;
        /// </summary>
        public const int EmptyMark = 0;

        /// <summary>
        /// 是否存在建筑物?
        /// </summary>
        public static bool Exist(this BuildingNode node)
        {
            return node.ID != EmptyMark;
        }

        /// <summary>
        /// 清除建筑信息;
        /// </summary>
        public static BuildingNode Destroy(this BuildingNode node)
        {
            return default(BuildingNode);
        }
    }

}
