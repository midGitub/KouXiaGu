using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 地图节点;
    /// </summary>
    [ProtoContract]
    public struct MapNode
    {
        [ProtoMember(1)]
        public LandformNode Landform;

        [ProtoMember(2)]
        public RoadNode Road;

        [ProtoMember(3)]
        public BuildingNode Building;

        [ProtoMember(4)]
        public TownNode Town;
    }
    
    /// <summary>
    /// 节点地貌信息;
    /// </summary>
    [ProtoContract]
    public struct LandformNode
    {
        /// <summary>
        /// 代表的地形ID;
        /// </summary>
        [ProtoMember(1)]
        public int LandformID;

        /// <summary>
        /// 地形旋转角度;
        /// </summary>
        [ProtoMember(2)]
        public float Angle;
    }

    /// <summary>
    /// 节点建筑信息;
    /// </summary>
    [ProtoContract]
    public struct BuildingNode
    {
        /// <summary>
        /// 建筑物类型编号;
        /// </summary>
        [ProtoMember(1)]
        public int BuildingID;

        /// <summary>
        /// 建筑物旋转的角度;
        /// </summary>
        [ProtoMember(2)]
        public float Angle;
    }

}
