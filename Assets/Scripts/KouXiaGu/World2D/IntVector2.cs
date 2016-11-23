//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using ProtoBuf;
//using UnityEngine;

//namespace KouXiaGu.World2D
//{

//    /// <summary>
//    /// Int类型的向量,不作为哈希表的键,因为哈希值不够分;
//    /// </summary>
//    [Serializable, ProtoContract]
//    public struct ShortVector2 : IEquatable<ShortVector2>
//    {

//        public ShortVector2(int x, int y)
//        {
//            this.x = x;
//            this.y = y;
//        }

//        public ShortVector2(short x, short y)
//        {
//            this.x = x;
//            this.y = y;
//        }

//        [ProtoMember(1)]
//        public int x;

//        [ProtoMember(2)]
//        public int y;


//        /// <summary>
//        /// 获取这两个点的距离;
//        /// </summary>
//        /// <param name="v1"></param>
//        /// <param name="v2"></param>
//        /// <returns></returns>
//        public static float Distance(ShortVector2 v1, ShortVector2 v2)
//        {
//            float distance = (float)Math.Sqrt(Math.Pow((v1.x - v2.x), 2) + Math.Pow((v1.y - v2.y), 2));
//            return distance;
//        }

//        /// <summary>
//        /// 获取这两个点的曼哈顿距离;
//        /// </summary>
//        /// <param name="v1"></param>
//        /// <param name="v2"></param>
//        /// <returns></returns>
//        public static int ManhattanDistance(ShortVector2 v1, ShortVector2 v2)
//        {
//            int distance = Math.Abs(v1.x - v2.x) + Math.Abs(v1.y - v2.y);
//            return distance;
//        }

//        /// <summary>
//        /// 获取到这个范围所有的点;
//        /// </summary>
//        public static IEnumerable<ShortVector2> Range(ShortVector2 southwest, ShortVector2 northeast)
//        {
//            for (int x = southwest.x; x <= northeast.x; x++)
//            {
//                for (int y = southwest.y; y <= northeast.y; y++)
//                {
//                    yield return new ShortVector2(x, y);
//                }
//            }
//        }



//        #region 重载,对比,转换;

//        public override string ToString()
//        {
//            return String.Concat("(", x, " , ", y, ")");
//        }

//        public override bool Equals(object obj)
//        {
//            if (!(obj is ShortVector2))
//                return false;
//            return Equals((ShortVector2)obj);
//        }

//        public bool Equals(ShortVector2 other)
//        {
//            return x == other.x && y == other.y;
//        }

//        public override int GetHashCode()
//        {
//            int hashCode = 0;

//            hashCode += x * ushort.MaxValue;
//            hashCode += short.MaxValue + y;

//            return hashCode;
//        }

//        /// <summary>
//        /// 根据四舍五入进行转换;
//        /// </summary>
//        public static explicit operator ShortVector2(Vector2 vector2)
//        {
//            return new ShortVector2(
//                (int)Math.Round(vector2.x),
//                (int)Math.Round(vector2.y));
//        }

//        /// <summary>
//        /// 根据四舍五入进行转换;
//        /// </summary>
//        public static explicit operator ShortVector2(Vector3 vector2)
//        {
//            return new ShortVector2(
//                (int)Math.Round(vector2.x),
//                (int)Math.Round(vector2.y));
//        }

//        public static explicit operator Vector2(ShortVector2 v)
//        {
//            return new Vector2(v.x, v.y);
//        }

//        public static explicit operator Vector3(ShortVector2 v)
//        {
//            return new Vector3(v.x, v.y);
//        }

//        public static bool operator ==(ShortVector2 point1, ShortVector2 point2)
//        {
//            bool sameX = point1.x == point2.x;
//            bool sameY = point1.y == point2.y;
//            return sameX & sameY;
//        }

//        public static bool operator !=(ShortVector2 point1, ShortVector2 point2)
//        {
//            bool sameX = point1.x == point2.x;
//            bool sameY = point1.y == point2.y;
//            return !(sameX & sameY);
//        }

//        public static ShortVector2 operator -(ShortVector2 point1, ShortVector2 point2)
//        {
//            point1.x -= point2.x;
//            point1.y -= point2.y;
//            return point1;
//        }

//        public static ShortVector2 operator +(ShortVector2 point1, ShortVector2 point2)
//        {
//            point1.x += point2.x;
//            point1.y += point2.y;
//            return point1;
//        }

//        public static ShortVector2 operator *(ShortVector2 point1, short n)
//        {
//            point1.x *= n;
//            point1.y *= n;
//            return point1;
//        }

//        public static ShortVector2 operator /(ShortVector2 point1, short n)
//        {
//            point1.x /= n;
//            point1.y /= n;
//            return point1;
//        }

//        public static ShortVector2 operator +(ShortVector2 point1, short n)
//        {
//            point1.x += n;
//            point1.y += n;
//            return point1;
//        }

//        public static ShortVector2 operator -(ShortVector2 point1, short n)
//        {
//            point1.x -= n;
//            point1.y -= n;
//            return point1;
//        }

//        #endregion


//    }

//}
