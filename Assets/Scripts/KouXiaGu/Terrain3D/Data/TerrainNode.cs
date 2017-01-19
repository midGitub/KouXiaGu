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

        [ProtoMember(10)]
        public LandformNode Landform;


        [ProtoMember(20)]
        public RoadNode Road;


        [ProtoMember(30)]
        public BuildingNode Building;


        public bool Equals(TerrainNode other)
        {
            return
                Landform.Equals(other.Landform) &&
                Road.Equals(other.Road) &&
                Building.Equals(other.Building);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TerrainNode))
                return false;
            return this.Equals((TerrainNode)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return
                "Landform:" + Landform.ToString()
                + "\nRoad:" + Road.ToString()
                + "\nBuilding:" + Building.ToString();
        }

    }

}
