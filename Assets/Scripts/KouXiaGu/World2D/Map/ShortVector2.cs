using System;
using ProtoBuf;
using UnityEngine;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// Short类型的向量,保存在哈希表内键值不重复;
    /// </summary>
    [Serializable, ProtoContract]
    public struct ShortVector2 : IEquatable<ShortVector2>
    {

        public ShortVector2(short x, short y)
        {
            this.x = x;
            this.y = y;
        }

        [ProtoMember(1)]
        public short x;

        [ProtoMember(2)]
        public short y;

        /// <summary>
        /// 获取这两个点的距离;
        /// </summary>
        public static float Distance(ShortVector2 v1, ShortVector2 v2)
        {
            float distance = (float)Math.Sqrt(Math.Pow((v1.x - v2.x), 2) + Math.Pow((v1.y - v2.y), 2));
            return distance;
        }

        /// <summary>
        /// 获取这两个点的曼哈顿距离;
        /// </summary>
        public static int ManhattanDistance(ShortVector2 v1, ShortVector2 v2)
        {
            int distance = Math.Abs(v1.x - v2.x) + Math.Abs(v1.y - v2.y);
            return distance;
        }

        /// <summary>
        /// 将x和y转换成正数;
        /// </summary>
        public static ShortVector2 Abs(ShortVector2 v1)
        {
            short x = Math.Abs(v1.x);
            short y = Math.Abs(v1.y);
            return new ShortVector2(x, y);
        }



        #region 重载,对比,转换;

        public override string ToString()
        {
            return String.Concat("(", x, " , ", y, ")");
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ShortVector2))
                return false;
            return Equals((ShortVector2)obj);
        }

        public bool Equals(ShortVector2 other)
        {
            return x == other.x && y == other.y;
        }

        /// <summary>
        /// 根据位置确定哈希值;
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = 0;

            hashCode += x * ushort.MaxValue;
            hashCode += short.MaxValue + y;

            return hashCode;
        }

        ///// <summary>
        ///// 转换成int,x放在前16位,y放在后16位;
        ///// </summary>
        //private int GetHashCode(short x, short y)
        //{
        //    int hashCode;

        //    byte[] shortX = BitConverter.GetBytes(x);
        //    byte[] shortY = BitConverter.GetBytes(y);

        //    byte[] hashArray = new byte[4];

        //    shortX.CopyTo(hashArray, 0);
        //    shortY.CopyTo(hashArray, 2);

        //    hashCode = BitConverter.ToInt32(hashArray, 0);

        //    return hashCode;
        //}

        /// <summary>
        /// 根据四舍五入进行转换;
        /// </summary>
        public static explicit operator ShortVector2(Vector2 vector2)
        {
            return new ShortVector2(
                (short)Math.Round(vector2.x),
                (short)Math.Round(vector2.y));
        }

        /// <summary>
        /// 根据四舍五入进行转换;
        /// </summary>
        public static explicit operator ShortVector2(Vector3 vector2)
        {
            return new ShortVector2(
                (short)Math.Round(vector2.x),
                (short)Math.Round(vector2.y));
        }

        public static implicit operator IntVector2(ShortVector2 v)
        {
            return new IntVector2(v.x, v.y);
        }

        public static explicit operator ShortVector2(IntVector2 v)
        {
            return new ShortVector2((short)v.x, (short)v.y);
        }

        public static explicit operator Vector2(ShortVector2 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static explicit operator Vector3(ShortVector2 v)
        {
            return new Vector3(v.x, v.y);
        }

        public static bool operator ==(ShortVector2 point1, ShortVector2 point2)
        {
            bool sameX = point1.x == point2.x;
            bool sameY = point1.y == point2.y;
            return sameX & sameY;
        }

        public static bool operator !=(ShortVector2 point1, ShortVector2 point2)
        {
            bool sameX = point1.x == point2.x;
            bool sameY = point1.y == point2.y;
            return !(sameX & sameY);
        }

        public static ShortVector2 operator -(ShortVector2 point1, ShortVector2 point2)
        {
            point1.x -= point2.x;
            point1.y -= point2.y;
            return point1;
        }

        public static ShortVector2 operator +(ShortVector2 point1, ShortVector2 point2)
        {
            point1.x += point2.x;
            point1.y += point2.y;
            return point1;
        }

        public static ShortVector2 operator *(ShortVector2 point1, short n)
        {
            point1.x *= n;
            point1.y *= n;
            return point1;
        }

        public static ShortVector2 operator /(ShortVector2 point1, short n)
        {
            point1.x /= n;
            point1.y /= n;
            return point1;
        }

        public static ShortVector2 operator +(ShortVector2 point1, short n)
        {
            point1.x += n;
            point1.y += n;
            return point1;
        }

        public static ShortVector2 operator -(ShortVector2 point1, short n)
        {
            point1.x -= n;
            point1.y -= n;
            return point1;
        }

        #endregion


    }
}
