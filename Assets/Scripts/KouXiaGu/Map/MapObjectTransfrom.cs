using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using UnityEngine;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 位于世界中的物体;
    /// </summary>
    [Serializable, ProtoContract]
    public struct MapObjectTransfrom
    {
        public MapObjectTransfrom(string name, Vector2 position, IntVector2 MapPosition)
        {
            this.Name = name;
            this.Position = position;
            this.mapPosition = MapPosition;
        }

        [ProtoMember(1)]
        public string Name { get; private set; }

        /// <summary>
        /// 物体所在的位置;
        /// </summary>
        [ProtoMember(2)]
        public ProtoVector2 Position { get; private set; }

        /// <summary>
        /// 物所在的地图节点;
        /// </summary>
        [ProtoMember(3)]
        public IntVector2 mapPosition { get; private set; }
    }

}
