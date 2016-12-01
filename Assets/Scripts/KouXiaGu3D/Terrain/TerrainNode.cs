using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.Terrain
{

    /// <summary>
    /// 用于保存的地形节点结构;
    /// </summary>
    [ProtoContract]
    public struct TerrainNode
    {

        [ProtoMember(1)]
        public ShortVector2 position;

        [ProtoMember(2)]
        public int terrainTypeId;

        [ProtoMember(3)]
        public float rotationAngle;

    }

}
