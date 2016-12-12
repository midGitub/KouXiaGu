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
        public TerrainNode(int id, float rotationAngle)
        {
            this.ID = id;
            this.RotationAngle = rotationAngle;
        }

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
    }

}
