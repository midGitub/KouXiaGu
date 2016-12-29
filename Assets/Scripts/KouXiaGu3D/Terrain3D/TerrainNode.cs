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
    public struct TerrainNode
    {

        /// <summary>
        /// 代表的地形ID((0,-1作为保留);
        /// </summary>
        [ProtoMember(1)]
        public int ID;

        /// <summary>
        /// 地形旋转角度;
        /// </summary>
        [ProtoMember(2)]
        public float RotationAngle;

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

    }

}
