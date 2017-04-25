//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using ProtoBuf;

//namespace KouXiaGu.Terrain3D
//{

//    /// <summary>
//    /// 地貌节点信息
//    /// </summary>
//    [ProtoContract]
//    public struct LandformNode : IEquatable<LandformNode>
//    {

//        internal const uint EMPTY_MARK = 0;

//        /// <summary>
//        /// 代表的地形ID;
//        /// </summary>
//        [ProtoMember(1)]
//        public int ID;

//        /// <summary>
//        /// 地形旋转角度;
//        /// </summary>
//        [ProtoMember(2)]
//        public float Angle;


//        /// <summary>
//        /// 是否设定了地貌?;
//        /// </summary>
//        public bool Exist()
//        {
//            return ID != EMPTY_MARK;
//        }

//        public bool Equals(LandformNode other)
//        {
//            return 
//                other.ID == ID && 
//                other.Angle == Angle;
//        }

//        public override bool Equals(object obj)
//        {
//            if (!(obj is LandformNode))
//                return false;
//            return this.Equals((LandformNode)obj);
//        }

//        public override int GetHashCode()
//        {
//            return ID.GetHashCode();
//        }

//        public override string ToString()
//        {
//            return "[ID:" + ID + ",Angle:" + Angle + "]";
//        }

//    }

//}
