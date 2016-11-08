using System;
using ProtoBuf;

namespace KouXiaGu.Map.HexMap
{

    /// <summary>
    /// 正六边形
    /// </summary>
    [Serializable, ProtoContract]
    public struct Hexagon
    {

        public Hexagon(float innerDiameter)
        {
            this.innerDiameter = innerDiameter;
        }

        private static readonly float cos30 = (float)Math.Cos(30 * (Math.PI / 180));

        [ProtoMember(1)]
        private float innerDiameter;

        public float InnerDiameter
        {
            get { return innerDiameter; }
            set { innerDiameter = value; }
        }

        public float OuterDiameter
        {
            get { return innerDiameter / cos30; }
            set { innerDiameter = value * cos30; }
        }

        /// <summary>
        /// 六边形单边长;
        /// </summary>
        public float Length
        {
            get { return OuterDiameter / 2; }
        }

        /// <summary>
        /// x轴方向六边形之间的间距;
        /// </summary>
        public float DistanceX
        {
            get{ return innerDiameter * cos30; }
        }

        /// <summary>
        /// y轴方向六边形之间的间距;
        /// </summary>
        public float DistanceY
        {
            get { return innerDiameter; }
        }

        public override string ToString()
        {
            string str = "";

            str = "内径 :" + innerDiameter +
                "\n外径 :" + OuterDiameter + 
                "\n边长 :" + Length + 
                "\nx轴距离 :" + DistanceX + 
                "\ny轴距离 :" + DistanceY;

            return str;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Hexagon))
                return false;
            Hexagon hexagon = (Hexagon)obj;
            return hexagon.innerDiameter == innerDiameter;
        }

        public override int GetHashCode()
        {
            return innerDiameter.GetHashCode();
        }

    }

}
