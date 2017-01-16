using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using UnityEngine;

namespace KouXiaGu.Grids
{

    /// <summary>
    /// 平顶六边形边对应的方向;
    /// </summary>
    [Flags]
    public enum HexDirections
    {
        Unknown = 0,
        North = 1,
        Northeast = 2,
        Southeast = 4,
        South = 8,
        Southwest = 16,
        Northwest = 32,
        Self = 64,
    }

    /// <summary>
    /// 平顶六边形对应的对角线方向;
    /// </summary>
    [Flags]
    public enum HexDiagonals
    {
        Unknown = 0,
        Northeast = 1,
        East = 2,
        Southeast = 4,
        Southwest = 8,
        West = 16,
        Northwest = 32,
        Self = 64,
    }

    /// <summary>
    /// 六边形立方体坐标;
    /// 所有有效坐标都满足 X + Y + Z = 0;
    /// </summary>
    [ProtoContract, Serializable]
    public struct CubicHexCoord : IEquatable<CubicHexCoord>, IGrid, IGrid<HexDirections>
    {

        /// <summary>
        /// 存在方向数,包括本身;
        /// </summary>
        public const int DirectionsNumber = 7;

        /// <summary>
        /// 存在的对角线方向数目;
        /// </summary>
        public const int DiagonalsNumber = 7;

        const int maxDirectionMark = (int)HexDirections.Self;
        const int minDirectionMark = (int)HexDirections.North;

        /// <summary>
        /// (0,0,0)
        /// </summary>
        public static readonly CubicHexCoord Self = new CubicHexCoord(0, 0, 0);

        //(平顶)存在的方向偏移量;
        public static readonly CubicHexCoord DIR_North = new CubicHexCoord(0, 1, -1);
        public static readonly CubicHexCoord DIR_Northeast = new CubicHexCoord(1, 0, -1);
        public static readonly CubicHexCoord DIR_Southeast = new CubicHexCoord(1, -1, 0);
        public static readonly CubicHexCoord DIR_South = new CubicHexCoord(0, -1, 1);
        public static readonly CubicHexCoord DIR_Southwest = new CubicHexCoord(-1, 0, 1);
        public static readonly CubicHexCoord DIR_Northwest = new CubicHexCoord(-1, 1, 0);

        //(平顶)存在的对角线方向偏移量;
        public static readonly CubicHexCoord DIA_Northeast = new CubicHexCoord(1, 1, -2);
        public static readonly CubicHexCoord DIA_East = new CubicHexCoord(2, -1, -1);
        public static readonly CubicHexCoord DIA_Southeast = new CubicHexCoord(1, -2, 1);
        public static readonly CubicHexCoord DIA_Southwest = new CubicHexCoord(-1, -1, 2);
        public static readonly CubicHexCoord DIA_West = new CubicHexCoord(-2, 1, 1);
        public static readonly CubicHexCoord DIA_Northwest = new CubicHexCoord(-1, 2, -1);

        /// <summary>
        /// 方向偏移量;
        /// </summary>
        static Dictionary<int, CubicHexCoord> directionVectors = new Dictionary<int, CubicHexCoord>()
        {
            { (int)HexDirections.North, DIR_North },
            { (int)HexDirections.Northeast, DIR_Northeast },
            { (int)HexDirections.Southeast, DIR_Southeast },
            { (int)HexDirections.South, DIR_South },
            { (int)HexDirections.Southwest, DIR_Southwest },
            { (int)HexDirections.Northwest, DIR_Northwest },
            { (int)HexDirections.Self, Self },
        };

        /// <summary>
        /// 对角线偏移量;
        /// </summary>
        static Dictionary<int, CubicHexCoord> diagonalVectors = new Dictionary<int, CubicHexCoord>()
        {
            { (int)HexDiagonals.Northeast, DIA_Northeast },
            { (int)HexDiagonals.East, DIA_East },
            { (int)HexDiagonals.Southeast, DIA_Southeast },
            { (int)HexDiagonals.Southwest, DIA_Southwest },
            { (int)HexDiagonals.West, DIA_West },
            { (int)HexDiagonals.Northwest, DIA_Northwest },
            { (int)HexDiagonals.Self, Self },
        };

        /// <summary>
        /// 按标记为从 高位到低位 循序排列的数组;不包含本身
        /// </summary>
        static readonly HexDirections[] DirectionsArray = new HexDirections[]
        {
            HexDirections.Northwest,
            HexDirections.Southwest,
            HexDirections.South,
            HexDirections.Southeast,
            HexDirections.Northeast,
            HexDirections.North,
        };

        /// <summary>
        /// 按标记为从 高位到低位 循序排列的数组;
        /// </summary>
        static readonly HexDirections[] DirectionsAndSelfArray = new HexDirections[]
        {
            HexDirections.Self,
            HexDirections.Northwest,
            HexDirections.Southwest,
            HexDirections.South,
            HexDirections.Southeast,
            HexDirections.Northeast,
            HexDirections.North,
        };

        [ProtoMember(1), SerializeField]
        short x;
        [ProtoMember(2), SerializeField]
        short y;
        [SerializeField]
        short z;

        public short X
        {
            get { return x; }
            private set { x = value; }
        }

        public short Y
        {
            get { return y; }
            private set { y = value; }
        }

        /// <summary>
        /// Z = - X - Y;
        /// </summary>
        public short Z
        {
            get { return z; }
            private set { z = value; }
        }

        public CubicHexCoord(short x, short y, short z)
        {
            OutOfRangeException(x, y, z);

            this.x = x;
            this.y = y;
            this.z = z;
        }

        public CubicHexCoord(short x, short y) : this()
        {
            SetValue(x, y);
        }

        public CubicHexCoord(int x, int y, int z)
        {
            OutOfRangeException((short)x, (short)y, (short)z);
            this.x = (short)x;
            this.y = (short)y;
            this.z = (short)z;
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

            OutOfRangeException((short)intQ, (short)intR, (short)intS);

            this.x = (short)intQ;
            this.y = (short)intR;
            this.z = (short)intS;
        }

        /// <summary>
        /// 在反序列化后调用;
        /// </summary>
        [ProtoAfterDeserialization]
        void ProtoAfterDeserialization()
        {
            this.z = (short)(-this.x - this.y);
        }

        public void SetValue(short x, short y)
        {
            this.X = x;
            this.Y = y;
            this.Z = (short)(-x - y);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CubicHexCoord))
                return false;
            return (CubicHexCoord)obj == this;
        }

        public bool Equals(CubicHexCoord other)
        {
            return other == this;
        }

        public override int GetHashCode()
        {
            int hashCode = X << 16;
            hashCode += short.MaxValue + Y;
            return hashCode;
        }

        public override string ToString()
        {
            return string.Concat("(", X, ",", Y, ",", Z, ")");
        }

        /// <summary>
        /// 获取到这个方向的坐标;
        /// </summary>
        public CubicHexCoord GetDirection(HexDirections direction)
        {
            return this + GetDirectionOffset(direction);
        }

        /// <summary>
        /// 获取到这个对角线的偏移量;
        /// </summary>
        public CubicHexCoord GetDiagonal(HexDiagonals diagonal)
        {
            return this + GetDiagonalOffset(diagonal);
        }

        /// <summary>
        /// 获取到这个点周围的方向和坐标;从 HexDirection 高位标记开始返回;
        /// </summary>
        public IEnumerable<CoordPack<CubicHexCoord, HexDirections>> GetNeighbours()
        {
            foreach (var direction in Directions)
            {
                CubicHexCoord point = this.GetDirection(direction);
                yield return new CoordPack<CubicHexCoord, HexDirections>(point, direction);
            }
        }

        /// <summary>
        /// 获取到目标点的邻居节点;
        /// </summary>
        public IEnumerable<CoordPack<CubicHexCoord, HexDirections>> GetNeighbours(HexDirections directions)
        {
            foreach (var direction in GetDirections(directions))
            {
                CubicHexCoord point = this.GetDirection(direction);
                yield return new CoordPack<CubicHexCoord, HexDirections>(point, direction);
            }
        }

        /// <summary>
        /// 获取到目标点的邻居节点,但是也返回自己本身;
        /// </summary>
        public IEnumerable<CoordPack<CubicHexCoord, HexDirections>> GetNeighboursAndSelf()
        {
            foreach (var direction in DirectionsAndSelf)
            {
                CubicHexCoord point = this.GetDirection(direction);
                yield return new CoordPack<CubicHexCoord, HexDirections>(point, direction);
            }
        }


        IEnumerable<IGrid> IGrid.GetNeighbours()
        {
            return GetNeighbours().Select(coord => coord.Point).Cast<IGrid>();
        }

        IEnumerable<IGrid> IGrid.GetNeighboursAndSelf()
        {
            return GetNeighboursAndSelf().Select(coord => coord.Point).Cast<IGrid>();
        }

        IEnumerable<CoordPack<IGrid<HexDirections>, HexDirections>> IGrid<HexDirections>.GetNeighbours()
        {
            return GetNeighbours().Select(coord => new CoordPack<IGrid<HexDirections>, HexDirections>(coord.Point, coord.Direction));
        }

        IEnumerable<CoordPack<IGrid<HexDirections>, HexDirections>> IGrid<HexDirections>.GetNeighboursAndSelf()
        {
            return GetNeighboursAndSelf().Select(coord => new CoordPack<IGrid<HexDirections>, HexDirections>(coord.Point, coord.Direction));
        }


        /// <summary>
        /// 定义的值是否超出定义范围?
        /// </summary>
        static bool IsOutOfRange(short x, short y, short z)
        {
            if ((x + y + z) != 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 超出定义范围返回异常;
        /// </summary>
        static void OutOfRangeException(short x, short y, short z)
        {
            if ((x + y + z) != 0)
                throw new ArgumentOutOfRangeException("坐标必须满足 (x + y + z) == 0");
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


        /// <summary>
        /// 使其根据(0,0,0)坐标进行翻转\类似镜像,一般对方向变量使用,获取到其相反方向;
        /// </summary>
        public static CubicHexCoord Invert(CubicHexCoord coord)
        {
            return coord * -1;
        }


        /// <summary>
        /// 曼哈顿距离;
        /// </summary>
        public static int ManhattanDistances(CubicHexCoord a, CubicHexCoord b)
        {
            CubicHexCoord hex = a - b;
            return (int)((Math.Abs(hex.X) + Math.Abs(hex.Y) + Math.Abs(hex.Z)) / 2);
        }


        /// <summary>
        /// 按标记为从 高位到低位 循序返回的迭代结构;不包含本身
        /// </summary>
        public static IEnumerable<HexDirections> Directions
        {
            get { return DirectionsArray; }
        }

        /// <summary>
        /// 获取到从 高位到低位 顺序返回的迭代结构;包括本身;
        /// </summary>
        public static IEnumerable<HexDirections> DirectionsAndSelf
        {
            get { return DirectionsAndSelfArray; }
        }

        /// <summary>
        /// 获取到方向偏移量;
        /// </summary>
        public static CubicHexCoord GetDirectionOffset(HexDirections direction)
        {
            return directionVectors[(int)direction];
        }

        /// <summary>
        /// 获取到对角线偏移量;
        /// </summary>
        public static CubicHexCoord GetDiagonalOffset(HexDiagonals diagonal)
        {
            return diagonalVectors[(int)diagonal];
        }

        /// <summary>
        /// 获取到方向集表示的所有方向;
        /// </summary>
        public static IEnumerable<HexDirections> GetDirections(HexDirections directions)
        {
            int mask = (int)directions;
            for (int intDirection = minDirectionMark; intDirection <= maxDirectionMark; intDirection <<= 1)
            {
                if ((intDirection & mask) != 0)
                {
                    yield return (HexDirections)intDirection;
                }
            }
        }


        /// <summary>
        /// 获取到目标所在的半径;
        /// </summary>
        public static int GetRadius(CubicHexCoord center, CubicHexCoord target)
        {
            var cube = target - center;
            return GetRadius(cube);
        }

        /// <summary>
        /// 获取到目标所在 (0,0,0) 的半径;
        /// </summary>
        public static int GetRadius(CubicHexCoord target)
        {
            return Mathf.Max(Mathf.Abs(target.X), Mathf.Abs(target.Y), Mathf.Abs(target.Z));
        }

        /// <summary>
        /// 获取到六边形的范围;
        /// 半径覆盖到的节点;
        /// </summary>
        public static IEnumerable<CubicHexCoord> Range(CubicHexCoord target, int step)
        {
            if (step < 0)
                throw new ArgumentOutOfRangeException();

            for (int x = -step; x <= step; x++)
            {
                for (int y = Math.Max(-step, -x - step); y <= Math.Min(step, -x + step); y++)
                {
                    int z = -x - y;
                    yield return new CubicHexCoord(x, y, z) + target;
                }
            }
        }

        /// <summary>
        ///  按 环状 返回点;
        /// </summary>
        /// <param name="radius">需要大于0</param>
        public static IEnumerable<CubicHexCoord> Ring(CubicHexCoord center, int radius)
        {
            var cube = center + (CubicHexCoord.GetDirectionOffset(HexDirections.Northeast) * radius);

            foreach (var direction in Directions)
            {
                for (int j = 0; j < radius; j++)
                {
                    yield return cube;
                    cube = cube.GetDirection(direction);
                }
            }
        }

        /// <summary>
        /// 按 螺旋 形状返回点;
        /// </summary>
        /// <param name="radius">需要大于0</param>
        public static List<CubicHexCoord> Spiral(CubicHexCoord center, int radius)
        {
            List<CubicHexCoord> coords = new List<CubicHexCoord>();
            for (int k = 1; k <= radius; k++)
            {
                coords.AddRange(Ring(center, k));
            }
            return coords;
        }


        public static bool operator ==(CubicHexCoord a, CubicHexCoord b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(CubicHexCoord a, CubicHexCoord b)
        {
            return !(a == b);
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

    }


}
