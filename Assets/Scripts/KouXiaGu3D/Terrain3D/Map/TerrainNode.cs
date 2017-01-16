using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 用于保存的地形节点结构;
    /// </summary>
    [ProtoContract]
    public struct TerrainNode : IEquatable<TerrainNode>
    {

        #region 地貌;

        /// <summary>
        /// 代表的地形ID((0,-1作为保留);
        /// </summary>
        [ProtoMember(1)]
        public int Landform;

        /// <summary>
        /// 地形旋转角度;
        /// </summary>
        [ProtoMember(2)]
        public float LandformAngle;

        /// <summary>
        /// 存在地貌信息?
        /// </summary>
        public bool ExistLandform
        {
            get { return Landform != 0; }
        }

        /// <summary>
        /// 与此节点的地貌信息是否相同?
        /// </summary>
        public bool EqualsLandform(TerrainNode other)
        {
            return 
                other.Landform == Landform &&
                other.LandformAngle == LandformAngle;
        }

        #endregion;


        #region 道路;

        /// <summary>
        /// 道路类型编号?不存在则为0,否则为道路类型编号;
        /// </summary>
        [ProtoMember(3)]
        public int Road;

        /// <summary>
        /// 存在道路?
        /// </summary>
        public bool ExistRoad
        {
            get { return Road != 0; }
        }

        /// <summary>
        /// 与此节点的道路信息是否相同?
        /// </summary>
        public bool EqualsRoad(TerrainNode other)
        {
            return
                  other.Road == Road;
        }

        [ProtoMember(10)]
        public RoadNode RoadInfo;

        #endregion

        /// <summary>
        /// 建筑物类型编号;
        /// </summary>
        [ProtoMember(4)]
        public int Building;

        /// <summary>
        /// 建筑物旋转的角度;
        /// </summary>
        [ProtoMember(5)]
        public float BuildingAngle;

        /// <summary>
        /// 存在建筑物?
        /// </summary>
        public bool ExistBuild
        {
            get { return Building != 0; }
        }

        /// <summary>
        /// 与此节点的建筑信息是否相同?
        /// </summary>
        public bool EqualsBuild(TerrainNode other)
        {
            return
                  other.Building == Building &&
                other.BuildingAngle == BuildingAngle;
        }


        /// <summary>
        /// 与此节点的信息是否完全相同?
        /// </summary>
        public bool Equals(TerrainNode other)
        {
            return
                EqualsLandform(other) &&
                EqualsRoad(other) &&
                EqualsBuild(other);
        }

    }

}
