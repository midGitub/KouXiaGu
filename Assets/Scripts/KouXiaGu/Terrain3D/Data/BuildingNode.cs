//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using ProtoBuf;

//namespace KouXiaGu.Terrain3D
//{


//    [ProtoContract]
//    public struct BuildingNode : IEquatable<BuildingNode>
//    {

//        /// <summary>
//        /// 建筑物类型编号;
//        /// </summary>
//        [ProtoMember(1)]
//        public int ID;

//        /// <summary>
//        /// 建筑物旋转的角度;
//        /// </summary>
//        [ProtoMember(2)]
//        public float Angle;


//        public bool Equals(BuildingNode other)
//        {
//            return
//               other.ID == ID &&
//               other.Angle == Angle;
//        }

//        public override bool Equals(object obj)
//        {
//            if (!(obj is BuildingNode))
//                return false;
//            return this.Equals((BuildingNode)obj);
//        }

//        public override int GetHashCode()
//        {
//            return ID.GetHashCode() ^ Angle.GetHashCode();
//        }

//        public override string ToString()
//        {
//            return "[ID:" + ID + ",Angle:" + Angle + "]";
//        }

//    }

//}
