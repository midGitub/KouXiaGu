using System;
using ProtoBuf;

namespace KouXiaGu
{

    /// <summary>
    /// 六边形立方体坐标;
    /// </summary>
    [ProtoContract]
    public struct CubicHexCoord
    {

        [ProtoMember(1)]
        public short X { get; private set; }
        [ProtoMember(2)]
        public short Y { get; private set; }
        public short Z { get; private set; }

        /// <summary>
        /// (0,0,0)
        /// </summary>
        public static readonly CubicHexCoord Zero = new CubicHexCoord(0, 0, 0);

        public CubicHexCoord(short x, short y, short z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public CubicHexCoord(short x, short y)
        {
            this.X = x;
            this.Y = y;
            this.Z = (short)(-x - y);
        }

        public CubicHexCoord(int x, int y, int z)
        {
            this.X = (short)x;
            this.Y = (short)y;
            this.Z = (short)z;
        }

        public CubicHexCoord(float x, float y, float z)
        {
            int intQ = (short)(Math.Round(x));
            int intR = (short)(Math.Round(y));
            int intS = (short)(Math.Round(z));
            double q_diff = Math.Abs(intQ - x);
            double r_diff = Math.Abs(intR - y);
            double s_diff = Math.Abs(intS - z);
            if (q_diff > r_diff && q_diff > s_diff)
            {
                intQ = -intR - intS;
            }
            else
            {
                if (r_diff > s_diff)
                {
                    intR = -intQ - intS;
                }
                else
                {
                    intS = -intQ - intR;
                }
            }
            this.X = (short)intQ;
            this.Y = (short)intR;
            this.Z = (short)intS;
        }

        /// <summary>
        /// 在反序列化后调用;
        /// </summary>
        [ProtoAfterDeserialization]
        void ProtoAfterDeserialization()
        {
            this.Z = (short)(-this.X - this.Y);
        }

        /// <summary>
        /// 将哈希值转换成坐标;
        /// </summary>
        public static CubicHexCoord HashCodeToCoord(int hashCode)
        {
            short x = (short)(hashCode >> 16);
            short y = (short)((hashCode & 0xFFFF) - short.MaxValue);
            return new CubicHexCoord(x, y);
        }

        public override int GetHashCode()
        {
            int hashCode = X << 16;
            hashCode += short.MaxValue + Y;
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CubicHexCoord))
                return false;
            return (CubicHexCoord)obj == this;
        }

        public override string ToString()
        {
            return string.Concat("(", X, ",", Y, ",", Z, ")");
        }

        public static CubicHexCoord operator +(CubicHexCoord a, CubicHexCoord b)
        {
            return new CubicHexCoord(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static CubicHexCoord operator -(CubicHexCoord a, CubicHexCoord b)
        {
            return new CubicHexCoord(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static CubicHexCoord operator *(CubicHexCoord a, int k)
        {
            return new CubicHexCoord(a.X * k, a.Y * k, a.Z * k);
        }

        public static CubicHexCoord operator /(CubicHexCoord a, int k)
        {
            return new CubicHexCoord(a.X / k, a.Y / k, a.Z / k);
        }

        public static bool operator ==(CubicHexCoord a, CubicHexCoord b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(CubicHexCoord a, CubicHexCoord b)
        {
            return !(a == b);
        }

    }


}
