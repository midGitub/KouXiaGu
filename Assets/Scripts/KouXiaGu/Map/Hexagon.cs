using System;
using ProtoBuf;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 正六边形
    /// </summary>
    [Serializable, ProtoContract]
    public struct Hexagon
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerDiameter">内径</param>
        /// <param name="isRotate">是否旋转90度后的六边形</param>
        public Hexagon(float innerDiameter, bool isRotate = false)
        {
            this.innerDiameter = innerDiameter;
            this.isRotate = isRotate;
        }

        private static readonly float cos30 = (float)Math.Cos(30 * (Math.PI / 180));

        [ProtoMember(1)]
        private bool isRotate;

        [ProtoMember(2)]
        private float innerDiameter;

        /// <summary>
        /// 是否为旋转90度后的正六边形;
        /// </summary>
        public bool IsRotate
        {
            get { return isRotate; }
            set { isRotate = value; }
        }

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
            get{ return isRotate ? innerDiameter : innerDiameter * cos30; }
        }

        /// <summary>
        /// y轴方向六边形之间的间距;
        /// </summary>
        public float DistanceY
        {
            get { return isRotate ? innerDiameter * cos30 : innerDiameter; }
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

    }

}
