using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using ProtoBuf;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 矩形网格存在的方向;
    /// </summary>
    [Flags]
    public enum RecDirections
    {
        North = 1,
        Northeast = 2,
        East = 4,
        Southeast = 8,
        South = 16,
        Southwest = 32,
        West = 64,
        Northwest = 128,
        Self = 256,
    }


    /// <summary>
    /// Short类型的向量,保存在哈希表内键值不重复;
    /// </summary>
    [Serializable, ProtoContract]
    public struct ShortVector2 : IEquatable<ShortVector2>, IGridPoint, IGridPoint<ShortVector2, RecDirections>
    {

        /// <summary>
        /// 存在方向数;
        /// </summary>
        public const int DirectionsNumber = 9;

        const int maxDirectionMark = (int)RecDirections.Self;
        const int minDirectionMark = (int)RecDirections.North;

        static readonly ShortVector2 up = new ShortVector2(0, 1);
        public static ShortVector2 Up
        {
            get { return up; }
        }

        static readonly ShortVector2 down = new ShortVector2(0, -1);
        public static ShortVector2 Down
        {
            get { return down; }
        }

        static readonly ShortVector2 left = new ShortVector2(-1, 0);
        public static ShortVector2 Left
        {
            get { return left; }
        }

        static readonly ShortVector2 right = new ShortVector2(1, 0);
        public static ShortVector2 Right
        {
            get { return right; }
        }

        static readonly ShortVector2 zero = new ShortVector2(0, 0);
        public static ShortVector2 Zero
        {
            get { return zero; }
        }

        static readonly ShortVector2 one = new ShortVector2(1, 1);
        public static ShortVector2 One
        {
            get { return one; }
        }

        /// <summary>
        /// 方向偏移量;
        /// </summary>
        static Dictionary<int, ShortVector2> directionsVector = new Dictionary<int, ShortVector2>()
        {
            { (int)RecDirections.North, ShortVector2.Up },
            { (int)RecDirections.Northeast, ShortVector2.Up + ShortVector2.Right },
            { (int)RecDirections.East, ShortVector2.Right},
            { (int)RecDirections.Southeast, ShortVector2.Down + ShortVector2.Right},
            { (int)RecDirections.South, ShortVector2.Down},
            { (int)RecDirections.Southwest, ShortVector2.Down + ShortVector2.Left},
            { (int)RecDirections.West, ShortVector2.Left},
            { (int)RecDirections.Northwest, ShortVector2.Up + ShortVector2.Left},
            { (int)RecDirections.Self, ShortVector2.Zero},

        };

        /// <summary>
        /// 按标记为从 高位到低位 循序排列的数组;不包含本身
        /// </summary>
        static readonly RecDirections[] DirectionsArray = new RecDirections[]
        {
            RecDirections.Northwest,
            RecDirections.West,
            RecDirections.Southwest,
            RecDirections.South,
            RecDirections.Southeast,
            RecDirections.East,
            RecDirections.Northeast,
            RecDirections.North,
        };

        /// <summary>
        /// 按标记为从 高位到低位 循序排列的数组(存在本身方向,且在最高位);
        /// </summary>
        static readonly RecDirections[] DirectionsAndSelfArray = new RecDirections[]
        {
            RecDirections.Self,
            RecDirections.Northwest,
            RecDirections.West,
            RecDirections.Southwest,
            RecDirections.South,
            RecDirections.Southeast,
            RecDirections.East,
            RecDirections.Northeast,
            RecDirections.North,
        };


        [ProtoMember(1)]
        public short x { get; set; }

        [ProtoMember(2)]
        public short y { get; set; }

        public ShortVector2(short x, short y)
        {
            this.x = x;
            this.y = y;
        }

        public ShortVector2(int x, int y)
        {
            this.x = (short)x;
            this.y = (short)y;
        }

        /// <summary>
        /// 将x和y转换成正数;
        /// </summary>
        public ShortVector2 Abs()
        {
            short x = Math.Abs(this.x);
            short y = Math.Abs(this.y);
            return new ShortVector2(x, y);
        }



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
        /// 获取到这个范围所有的点;
        /// </summary>
        public static IEnumerable<ShortVector2> RecRange(ShortVector2 southwest, ShortVector2 northeast)
        {
            for (short x = southwest.x; x <= northeast.x; x++)
            {
                for (short y = southwest.y; y <= northeast.y; y++)
                {
                    yield return new ShortVector2(x, y);
                }
            }
        }


        /// <summary>
        /// 获取到方向偏移量;
        /// </summary>
        public ShortVector2 GetDirectionOffset(RecDirections direction)
        {
            return directionsVector[(int)direction];
        }

        /// <summary>
        /// 获取到这个方向的坐标;
        /// </summary>
        public ShortVector2 GetDirection(RecDirections direction)
        {
            return this + GetDirectionOffset(direction);
        }

        /// <summary>
        /// 按标记为从 高位到低位 循序返回的迭代结构;不包含本身
        /// </summary>
        public static IEnumerable<RecDirections> Directions
        {
            get { return DirectionsArray; }
        }

        /// <summary>
        /// 获取到从 高位到低位 顺序返回的迭代结构;(存在本身方向,且在最高位);
        /// </summary>
        public static IEnumerable<RecDirections> DirectionsAndSelf
        {
            get { return DirectionsAndSelfArray; }
        }

        /// <summary>
        /// 获取到方向集表示的所有方向;
        /// </summary>
        public static IEnumerable<RecDirections> GetDirections(RecDirections directions)
        {
            int mask = (int)directions;
            for (int intDirection = minDirectionMark; intDirection <= maxDirectionMark; intDirection <<= 1)
            {
                if ((intDirection & mask) == 1)
                {
                    yield return (RecDirections)intDirection;
                }
            }
        }


        /// <summary>
        /// 获取到所属的块编号;
        /// </summary>
        public ShortVector2 Block(int size)
        {
            short x = (short)Math.Round(this.x / (float)size);
            short y = (short)Math.Round(this.y / (float)size);
            return new ShortVector2(x, y);
        }

        /// <summary>
        /// 获取到目标点的邻居节点;
        /// </summary>
        public IEnumerable<ShortVector2> GetNeighbours()
        {
            foreach (var direction in Directions)
            {
                yield return this.GetDirection(direction);
            }
        }

        /// <summary>
        /// 获取到目标点的邻居节点;
        /// </summary>
        public IEnumerable<ShortVector2> GetNeighbours(RecDirections directions)
        {
            foreach (var direction in GetDirections(directions))
            {
                yield return this.GetDirection(direction);
            }
        }

        /// <summary>
        /// 获取到目标点的邻居节点,但是也返回自己本身;
        /// </summary>
        public IEnumerable<ShortVector2> GetNeighboursAndSelf()
        {
            foreach (var direction in DirectionsAndSelf)
            {
                yield return this.GetDirection(direction);
            }
        }



        IEnumerable<RecDirections> IGridPoint<ShortVector2, RecDirections>.Directions
        {
            get { return Directions; }
        }

        IEnumerable<RecDirections> IGridPoint<ShortVector2, RecDirections>.DirectionsAndSelf
        {
            get { return DirectionsAndSelf; }
        }

        IEnumerable<RecDirections> IGridPoint<ShortVector2, RecDirections>.GetDirections(RecDirections directions)
        {
            return GetDirections(directions);
        }

        IGridPoint IGridPoint<ShortVector2, RecDirections>.GetDirection(RecDirections direction)
        {
            return GetDirection(direction);
        }

        IEnumerable<IGridPoint> IGridPoint.GetNeighbours()
        {
            return GetNeighbours().Cast<IGridPoint>();
        }

        IEnumerable<IGridPoint> IGridPoint.GetNeighboursAndSelf()
        {
            return GetNeighboursAndSelf().Cast<IGridPoint>();
        }



        /// <summary>
        /// 将哈希值转换成坐标;
        /// </summary>
        public static ShortVector2 HashCodeToVector(int hashCode)
        {
            short x = (short)(hashCode >> 16);
            short y = (short)((hashCode & 0xFFFF) - short.MaxValue);
            return new ShortVector2(x, y);
        }

        /// <summary>
        /// 根据位置确定哈希值;
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = x << 16;
            hashCode += short.MaxValue + y;
            return hashCode;
        }


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
        /// 根据四舍五入进行转换(取 x 和 z 轴);
        /// </summary>
        public static explicit operator ShortVector2(Vector3 vector3)
        {
            return new ShortVector2(
                (short)Math.Round(vector3.x),
                (short)Math.Round(vector3.z));
        }

        public static explicit operator Vector2(ShortVector2 v)
        {
            return new Vector2(v.x, v.y);
        }

        /// <summary>
        /// 转换到向量;(设置 x 和 z 轴, y轴设为0);
        /// </summary>
        public static explicit operator Vector3(ShortVector2 v)
        {
            return new Vector3(v.x, 0, v.y);
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

    }
}
